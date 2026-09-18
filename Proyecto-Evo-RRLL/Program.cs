using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecto_Evo_RRLL.Filtros;

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

// Capa Datos (76)
builder.Services.AddScoped<AFPData>();
builder.Services.AddScoped<AguaVivData>();
builder.Services.AddScoped<BitacoraData>();
builder.Services.AddScoped<AlumbradoVivData>();
builder.Services.AddScoped<AsistenciaData>();
builder.Services.AddScoped<CapacitacionData>();
builder.Services.AddScoped<CargoData>();
builder.Services.AddScoped<ComportamientoData>();
builder.Services.AddScoped<ConservacionVivData>();
builder.Services.AddScoped<DiscapacidadData>();
builder.Services.AddScoped<EmpleadoData>();
builder.Services.AddScoped<Empleado_AreaData>();
builder.Services.AddScoped<EstadoCivilData>();
builder.Services.AddScoped<EstadoEstudioData>();
builder.Services.AddScoped<EstructOrganizData>();
builder.Services.AddScoped<EstudiosRealizadoData>();
builder.Services.AddScoped<ExcretasVivData>();
builder.Services.AddScoped<ExpLaboralData>();
builder.Services.AddScoped<FamiliaData>();
builder.Services.AddScoped<FamiliarData>();
builder.Services.AddScoped<HorarioData>();
builder.Services.AddScoped<HorarioTemporalData>();
builder.Services.AddScoped<InconvenienteData>();
builder.Services.AddScoped<InstitucionData>();
builder.Services.AddScoped<LimitacionData>();
builder.Services.AddScoped<LocalData>();
builder.Services.AddScoped<MarcacionData>();
builder.Services.AddScoped<MaterialVivData>();
builder.Services.AddScoped<MotivoBajaData>();
builder.Services.AddScoped<Motivo_PermData>();
builder.Services.AddScoped<NacionalidadData>();
builder.Services.AddScoped<PeriodoLaboralData>();
builder.Services.AddScoped<PermisoData>();
builder.Services.AddScoped<PersonaData>();
builder.Services.AddScoped<RegAsisDiarioData>();
builder.Services.AddScoped<RegimenPensionData>();
builder.Services.AddScoped<ResolucionData>();
builder.Services.AddScoped<ResponsableXUOData>();
builder.Services.AddScoped<RolData>();
builder.Services.AddScoped<Rol_AccesoData>();
builder.Services.AddScoped<RotacionData>();
builder.Services.AddScoped<RRHH_AcudeEnfermData>();
builder.Services.AddScoped<RRHH_AseguradoData>();
builder.Services.AddScoped<RRHH_AspSocioData>();
builder.Services.AddScoped<RRHH_DinamicaFamiliarData>();
builder.Services.AddScoped<RRHH_FeriadoData>();
builder.Services.AddScoped<RRHH_FuncFamData>();
builder.Services.AddScoped<RRHH_RelacHermanoData>();
builder.Services.AddScoped<RRHH_RelacParejaData>();
builder.Services.AddScoped<RRHH_RelacPHData>();
builder.Services.AddScoped<RRHH_SaludData>();
builder.Services.AddScoped<RRHH_TipoFamiliaData>();
builder.Services.AddScoped<SistemaOpcionData>();
builder.Services.AddScoped<TenenciaVivData>();
builder.Services.AddScoped<TipoComportamientoData>();
builder.Services.AddScoped<TipoDocData>();
builder.Services.AddScoped<TipoDocIDData>();
builder.Services.AddScoped<TipoEstudioData>();
builder.Services.AddScoped<TipoFamiliarData>();
builder.Services.AddScoped<TipoInstitucionData>();
builder.Services.AddScoped<TipoMonedaData>();
builder.Services.AddScoped<TipoMovimientoData>();
builder.Services.AddScoped<TipoPermisoData>();
builder.Services.AddScoped<TipoRecursoData>();
builder.Services.AddScoped<TipoResolucionData>();
builder.Services.AddScoped<TipoSangreData>();
builder.Services.AddScoped<TipoTrabajadorData>();
builder.Services.AddScoped<TipoTransaccionData>();
builder.Services.AddScoped<TipoVivData>();
builder.Services.AddScoped<TitulosEmpleadoData>();
builder.Services.AddScoped<UbicacionVivData>();
builder.Services.AddScoped<UbigeoData>();
builder.Services.AddScoped<UnidadMedidaData>();
builder.Services.AddScoped<UsuarioData>();
builder.Services.AddScoped<Usuario_RolData>();
builder.Services.AddScoped<ViveConData>();
builder.Services.AddScoped<ViviendaData>();

// Repositorio generico de catalogos
builder.Services.AddScoped<CatalogoRepositorio>();
builder.Services.AddScoped<SecuenciaService>();
builder.Services.AddScoped<Proyecto_Evo_RRLL.Services.EmpleadoServicio>();

var app = builder.Build();

// Seed: usuario administrador inicial en BD existente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EvoRRLDbContext>();
    var adminClave = builder.Configuration["Seguridad:AdminClave"] ?? "Admin.2026";
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

    var rol = await db.Set<Rol>().FirstOrDefaultAsync(r => r.Descripcion != null && r.Descripcion.Trim().Equals("Administrador"));
    if (rol is null)
    {
        var maxId = await db.Set<Rol>().AnyAsync() ? await db.Set<Rol>().MaxAsync(r => r.IdRol) + 1 : 1;
        rol = new Rol { IdRol = maxId, Descripcion = "Administrador", IdSistema = "GRLL" };
        db.Set<Rol>().Add(rol);
    }

    db.Set<Usuario>().Add(admin);
    await db.SaveChangesAsync();
    db.Set<Usuario_Rol>().Add(new Usuario_Rol { IdUsuario = admin.IdUsuario, IdRol = rol.IdRol });
    await db.SaveChangesAsync();
}