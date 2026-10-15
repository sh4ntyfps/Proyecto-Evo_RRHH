using Capa_Datos;
using Capa_Entidades;
using Capa_Logica;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Proyecto_Evo_RRLL.Controllers;
using Proyecto_Evo_RRLL.Filtros;
using Proyecto_Evo_RRLL.Utilidades;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options => options.Filters.Add<RegistrarExcepcionFilter>());

// Capa Datos: unica pieza ligada a la base de datos. Para usar otra BD se cambia esa capa
// (y la cadena de conexion); la logica, los controllers y las vistas no se tocan.
builder.Services.AgregarCapaDatos(builder.Configuration.GetConnectionString("RRHHNuevo")!);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Seguridad/Login";
        options.AccessDeniedPath = "/Seguridad/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdministradores", p => p.RequireRole("Administrador"));
});

// Capa de logica y servicios propios de la aplicacion
builder.Services.AgregarServiciosDeAplicacion();

var app = builder.Build();

// Seed: usuario administrador inicial en BD existente
using (var scope = app.Services.CreateScope())
{
    // La clave no va en el codigo ni en appsettings.json: en desarrollo viene de appsettings.Development.json
    // y en produccion de la variable de entorno Seguridad__AdminClave.
    var adminClave = builder.Configuration["Seguridad:AdminClave"];
    if (string.IsNullOrWhiteSpace(adminClave))
        app.Logger.LogWarning("Seguridad:AdminClave no esta configurada: no se crea el usuario admin inicial.");
    else
        await SeedAdminAsync(scope.ServiceProvider, adminClave);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStatusCodePagesWithReExecute("/Home/PaginaNoEncontrada", "?statusCode={0}");

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

static async Task SeedAdminAsync(IServiceProvider servicios, string adminClave)
{
    const string LoginAdmin = "admin";
    var usuarioLogica = servicios.GetRequiredService<UsuarioLogica>();
    if (await usuarioLogica.ObtenerPorLogin(LoginAdmin) is not null)
        return;

    // Rol administrador del sistema RRHH (SistemaOpcion '0500000'); en los datos legados es el rol 13
    var rolLogica = servicios.GetRequiredService<RolLogica>();
    var rol = (await rolLogica.Listar()).FirstOrDefault(r => r.IdSistema?.Trim() == SeguridadController.SistemaRRHH
        && r.Descripcion != null && r.Descripcion.Trim().ToUpper() == "ADMINISTRADOR");
    if (rol is null)
    {
        rol = new Rol { Descripcion = "Administrador", IdSistema = SeguridadController.SistemaRRHH };
        await rolLogica.Crear(rol);
    }

    var hasher = new PasswordHasher<Usuario>();
    var admin = new Usuario
    {
        Login = LoginAdmin,
        Descripcion = "Administrador del Sistema",
        Fecha = DateTime.Now,
        Estado = true,
        PasswordHash = hasher.HashPassword(new Usuario(), adminClave)
    };
    await usuarioLogica.Crear(admin);
    await servicios.GetRequiredService<Usuario_RolLogica>().Crear(new Usuario_Rol { IdUsuario = admin.IdUsuario, IdRol = rol.IdRol });
}
