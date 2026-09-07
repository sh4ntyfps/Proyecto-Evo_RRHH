using Capa_Datos;
using Capa_Entidades;
using Capa_Logica;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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

builder.Services.AddAuthorization();

// Capa Datos (76)
builder.Services.AddScoped<AFPData>();
builder.Services.AddScoped<AguaVivData>();
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

// Capa Logica (76)
builder.Services.AddScoped<AFPLogica>();
builder.Services.AddScoped<AguaVivLogica>();
builder.Services.AddScoped<AlumbradoVivLogica>();
builder.Services.AddScoped<AsistenciaLogica>();
builder.Services.AddScoped<CapacitacionLogica>();
builder.Services.AddScoped<CargoLogica>();
builder.Services.AddScoped<ComportamientoLogica>();
builder.Services.AddScoped<ConservacionVivLogica>();
builder.Services.AddScoped<DiscapacidadLogica>();
builder.Services.AddScoped<EmpleadoLogica>();
builder.Services.AddScoped<Empleado_AreaLogica>();
builder.Services.AddScoped<EstadoCivilLogica>();
builder.Services.AddScoped<EstadoEstudioLogica>();
builder.Services.AddScoped<EstructOrganizLogica>();
builder.Services.AddScoped<EstudiosRealizadoLogica>();
builder.Services.AddScoped<ExcretasVivLogica>();
builder.Services.AddScoped<ExpLaboralLogica>();
builder.Services.AddScoped<FamiliaLogica>();
builder.Services.AddScoped<FamiliarLogica>();
builder.Services.AddScoped<HorarioLogica>();
builder.Services.AddScoped<HorarioTemporalLogica>();
builder.Services.AddScoped<InconvenienteLogica>();
builder.Services.AddScoped<InstitucionLogica>();
builder.Services.AddScoped<LimitacionLogica>();
builder.Services.AddScoped<LocalLogica>();
builder.Services.AddScoped<MarcacionLogica>();
builder.Services.AddScoped<MaterialVivLogica>();
builder.Services.AddScoped<MotivoBajaLogica>();
builder.Services.AddScoped<Motivo_PermLogica>();
builder.Services.AddScoped<NacionalidadLogica>();
builder.Services.AddScoped<PeriodoLaboralLogica>();
builder.Services.AddScoped<PermisoLogica>();
builder.Services.AddScoped<PersonaLogica>();
builder.Services.AddScoped<RegAsisDiarioLogica>();
builder.Services.AddScoped<RegimenPensionLogica>();
builder.Services.AddScoped<ResolucionLogica>();
builder.Services.AddScoped<ResponsableXUOLogica>();
builder.Services.AddScoped<RolLogica>();
builder.Services.AddScoped<Rol_AccesoLogica>();
builder.Services.AddScoped<RotacionLogica>();
builder.Services.AddScoped<RRHH_AcudeEnfermLogica>();
builder.Services.AddScoped<RRHH_AseguradoLogica>();
builder.Services.AddScoped<RRHH_AspSocioLogica>();
builder.Services.AddScoped<RRHH_DinamicaFamiliarLogica>();
builder.Services.AddScoped<RRHH_FeriadoLogica>();
builder.Services.AddScoped<RRHH_FuncFamLogica>();
builder.Services.AddScoped<RRHH_RelacHermanoLogica>();
builder.Services.AddScoped<RRHH_RelacParejaLogica>();
builder.Services.AddScoped<RRHH_RelacPHLogica>();
builder.Services.AddScoped<RRHH_SaludLogica>();
builder.Services.AddScoped<RRHH_TipoFamiliaLogica>();
builder.Services.AddScoped<SistemaOpcionLogica>();
builder.Services.AddScoped<TenenciaVivLogica>();
builder.Services.AddScoped<TipoComportamientoLogica>();
builder.Services.AddScoped<TipoDocLogica>();
builder.Services.AddScoped<TipoDocIDLogica>();
builder.Services.AddScoped<TipoEstudioLogica>();
builder.Services.AddScoped<TipoFamiliarLogica>();
builder.Services.AddScoped<TipoInstitucionLogica>();
builder.Services.AddScoped<TipoMonedaLogica>();
builder.Services.AddScoped<TipoMovimientoLogica>();
builder.Services.AddScoped<TipoPermisoLogica>();
builder.Services.AddScoped<TipoRecursoLogica>();
builder.Services.AddScoped<TipoResolucionLogica>();
builder.Services.AddScoped<TipoSangreLogica>();
builder.Services.AddScoped<TipoTrabajadorLogica>();
builder.Services.AddScoped<TipoTransaccionLogica>();
builder.Services.AddScoped<TipoVivLogica>();
builder.Services.AddScoped<TitulosEmpleadoLogica>();
builder.Services.AddScoped<UbicacionVivLogica>();
builder.Services.AddScoped<UbigeoLogica>();
builder.Services.AddScoped<UnidadMedidaLogica>();
builder.Services.AddScoped<UsuarioLogica>();
builder.Services.AddScoped<Usuario_RolLogica>();
builder.Services.AddScoped<ViveConLogica>();
builder.Services.AddScoped<ViviendaLogica>();

var app = builder.Build();

// Seed: usuario administrador inicial en BD existente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EvoRRLDbContext>();
    await SeedAdminAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

static async Task SeedAdminAsync(EvoRRLDbContext db)
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
        PasswordHash = hasher.HashPassword(new Usuario(), "Admin.2026")
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