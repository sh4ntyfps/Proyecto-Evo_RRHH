using System.Threading.RateLimiting;
using Capa_Datos;
using Capa_Entidades;
using Capa_Entidades.Constantes;
using Capa_Logica;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Proyecto_Evo_RRLL.Controllers;
using Microsoft.AspNetCore.RateLimiting;
using Proyecto_Evo_RRLL.Filtros;
using Proyecto_Evo_RRLL.Utilidades;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options => options.Filters.Add<RegistrarExcepcionFilter>());

// Capa Datos: unica pieza ligada a la base de datos. Para usar otra BD se cambia esa capa
// (y la cadena de conexion); la logica, los controllers y las vistas no se tocan.
builder.Services.AgregarCapaDatos(builder.Configuration.GetConnectionString("RRHHNormalizado")!);

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
    options.AddPolicy("SoloAdministradores", p => p.RequireRole(ConstantesSeguridad.RolAdministrador));
});

// Capa de logica y servicios propios de la aplicacion
builder.Services.AgregarServiciosDeAplicacion();

// Limite de peticiones: evita que un usuario (o una IP) sature el servidor con demasiadas
// peticiones seguidas. Global: 100 por minuto por usuario/IP; login: 5 por minuto (anti fuerza bruta).
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(contexto =>
    {
        var clave = contexto.User.Identity?.IsAuthenticated == true
            ? "u:" + contexto.User.Identity!.Name
            : "ip:" + (contexto.Connection.RemoteIpAddress?.ToString() ?? "anon");

        return RateLimitPartition.GetFixedWindowLimiter(clave, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 100,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        });
    });

    options.AddFixedWindowLimiter("login", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.QueueLimit = 0;
    });
});

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

app.UseRateLimiter();

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

    // Rol administrador del sistema RRHH; en los datos legados es el rol 13 ("ADMINISTRADOR").
    var rolLogica = servicios.GetRequiredService<RolLogica>();
    var rol = (await rolLogica.Listar()).FirstOrDefault(r => r.IdSistema?.Trim() == ConstantesSeguridad.SistemaRRHH
        && string.Equals(r.Descripcion?.Trim(), ConstantesSeguridad.RolAdministrador, StringComparison.OrdinalIgnoreCase));
    if (rol is null)
    {
        rol = new Rol { Descripcion = ConstantesSeguridad.RolAdministrador, IdSistema = ConstantesSeguridad.SistemaRRHH };
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
