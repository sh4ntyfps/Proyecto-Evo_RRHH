using Capa_Datos;
using Capa_Entidades;
using Capa_Logica;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecto_Evo_RRLL.Controllers;
using Proyecto_Evo_RRLL.Filtros;
using Proyecto_Evo_RRLL.Utilidades;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options => options.Filters.Add<RegistrarExcepcionFilter>());

builder.Services.AddDbContext<EvoRRLDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RRHHNuevo")));

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

// Capas de datos y logica, y servicios propios de la aplicacion
builder.Services.AgregarServiciosDeAplicacion();

var app = builder.Build();

// Seed: usuario administrador inicial en BD existente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EvoRRLDbContext>();
    // La clave no va en el codigo ni en appsettings.json: en desarrollo viene de appsettings.Development.json
    // y en produccion de la variable de entorno Seguridad__AdminClave.
    var adminClave = builder.Configuration["Seguridad:AdminClave"];
    if (string.IsNullOrWhiteSpace(adminClave))
        app.Logger.LogWarning("Seguridad:AdminClave no esta configurada: no se crea el usuario admin inicial.");
    else
        await SeedAdminAsync(db, adminClave);
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

static async Task SeedAdminAsync(EvoRRLDbContext db, string adminClave)
{
    const string LoginAdmin = "admin";
    if (await db.Set<Usuario>().AnyAsync(u => u.Login != null && u.Login.Trim() == LoginAdmin))
        return;

    var hasher = new PasswordHasher<Usuario>();
    var maxIdUsuario = await db.Set<Usuario>().AnyAsync() ? await db.Set<Usuario>().MaxAsync(u => u.IdUsuario) + 1 : 1;
    var admin = new Usuario
    {
        IdUsuario = maxIdUsuario,
        Login = LoginAdmin,
        Descripcion = "Administrador del Sistema",
        Fecha = DateTime.Now,
        Estado = true,
        PasswordHash = hasher.HashPassword(new Usuario(), adminClave)
    };

    // Rol administrador del sistema RRHH (SistemaOpcion '0500000'); en los datos legados es el rol 13
    var rol = await db.Set<Rol>().FirstOrDefaultAsync(r => r.IdSistema == SeguridadController.SistemaRRHH
        && r.Descripcion != null && r.Descripcion.Trim().ToUpper() == "ADMINISTRADOR");
    if (rol is null)
    {
        var maxId = await db.Set<Rol>().AnyAsync() ? await db.Set<Rol>().MaxAsync(r => r.IdRol) + 1 : 1;
        rol = new Rol { IdRol = maxId, Descripcion = "Administrador", IdSistema = SeguridadController.SistemaRRHH };
        db.Set<Rol>().Add(rol);
    }

    db.Set<Usuario>().Add(admin);
    await db.SaveChangesAsync();
    db.Set<Usuario_Rol>().Add(new Usuario_Rol { IdUsuario = admin.IdUsuario, IdRol = rol.IdRol });
    await db.SaveChangesAsync();
}
