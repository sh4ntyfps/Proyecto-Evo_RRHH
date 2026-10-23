using Microsoft.EntityFrameworkCore;
using Capa_Entidades;
namespace Capa_Datos;
public class EvoRRLDbContext : DbContext
{
    public EvoRRLDbContext(DbContextOptions<EvoRRLDbContext> options) : base(options) { }
        public DbSet<AFP> AFP { get; set; } = null!;
        public DbSet<Bitacora> Bitacora { get; set; } = null!;
        public DbSet<AguaViv> AguaViv { get; set; } = null!;
        public DbSet<AlumbradoViv> AlumbradoViv { get; set; } = null!;
        public DbSet<Asistencia> Asistencia { get; set; } = null!;
        public DbSet<Capacitacion> Capacitacion { get; set; } = null!;
        public DbSet<Cargo> Cargo { get; set; } = null!;
        public DbSet<Comportamiento> Comportamiento { get; set; } = null!;
        public DbSet<ConservacionViv> ConservacionViv { get; set; } = null!;
        public DbSet<Discapacidad> Discapacidad { get; set; } = null!;
        public DbSet<Empleado> Empleado { get; set; } = null!;
        public DbSet<Empleado_Area> Empleado_Area { get; set; } = null!;
        public DbSet<EstadoCivil> EstadoCivil { get; set; } = null!;
        public DbSet<EstadoEstudio> EstadoEstudio { get; set; } = null!;
        public DbSet<EstructOrganiz> EstructOrganiz { get; set; } = null!;
        public DbSet<EstudiosRealizado> EstudiosRealizado { get; set; } = null!;
        public DbSet<ExcretasViv> ExcretasViv { get; set; } = null!;
        public DbSet<ExpLaboral> ExpLaboral { get; set; } = null!;
        public DbSet<Familia> Familia { get; set; } = null!;
        public DbSet<Familiar> Familiar { get; set; } = null!;
        public DbSet<Horario> Horario { get; set; } = null!;
        public DbSet<HorarioTemporal> HorarioTemporal { get; set; } = null!;
        public DbSet<Inconveniente> Inconveniente { get; set; } = null!;
        public DbSet<Institucion> Institucion { get; set; } = null!;
        public DbSet<Limitacion> Limitacion { get; set; } = null!;
        public DbSet<Local> Local { get; set; } = null!;
        public DbSet<Marcacion> Marcacion { get; set; } = null!;
        public DbSet<MaterialViv> MaterialViv { get; set; } = null!;
        public DbSet<Motivo_Perm> Motivo_Perm { get; set; } = null!;
        public DbSet<MotivoBaja> MotivoBaja { get; set; } = null!;
        public DbSet<Nacionalidad> Nacionalidad { get; set; } = null!;
        public DbSet<PeriodoLaboral> PeriodoLaboral { get; set; } = null!;
        public DbSet<Permiso> Permiso { get; set; } = null!;
        public DbSet<Persona> Persona { get; set; } = null!;
        public DbSet<RegAsisDiario> RegAsisDiario { get; set; } = null!;
        public DbSet<RegimenPension> RegimenPension { get; set; } = null!;
        public DbSet<Resolucion> Resolucion { get; set; } = null!;
        public DbSet<ResponsableXUO> ResponsableXUO { get; set; } = null!;
        public DbSet<Rol> Rol { get; set; } = null!;
        public DbSet<Rol_Acceso> Rol_Acceso { get; set; } = null!;
        public DbSet<Rotacion> Rotacion { get; set; } = null!;
        public DbSet<RRHH_AcudeEnferm> RRHH_AcudeEnferm { get; set; } = null!;
        public DbSet<RRHH_Asegurado> RRHH_Asegurado { get; set; } = null!;
        public DbSet<RRHH_AspSocio> RRHH_AspSocio { get; set; } = null!;
        public DbSet<RRHH_DinamicaFamiliar> RRHH_DinamicaFamiliar { get; set; } = null!;
        public DbSet<RRHH_Feriado> RRHH_Feriado { get; set; } = null!;
        public DbSet<RRHH_RelacHermano> RRHH_RelacHermano { get; set; } = null!;
        public DbSet<RRHH_RelacPareja> RRHH_RelacPareja { get; set; } = null!;
        public DbSet<RRHH_RelacPH> RRHH_RelacPH { get; set; } = null!;
        public DbSet<RRHH_Salud> RRHH_Salud { get; set; } = null!;
        public DbSet<RRHH_TipoFamilia> RRHH_TipoFamilia { get; set; } = null!;
        public DbSet<SistemaOpcion> SistemaOpcion { get; set; } = null!;
        public DbSet<TenenciaViv> TenenciaViv { get; set; } = null!;
        public DbSet<TipoComportamiento> TipoComportamiento { get; set; } = null!;
        public DbSet<TipoDoc> TipoDoc { get; set; } = null!;
        public DbSet<TipoDocID> TipoDocID { get; set; } = null!;
        public DbSet<TipoEstudio> TipoEstudio { get; set; } = null!;
        public DbSet<TipoFamiliar> TipoFamiliar { get; set; } = null!;
        public DbSet<TipoInstitucion> TipoInstitucion { get; set; } = null!;
        public DbSet<TipoMoneda> TipoMoneda { get; set; } = null!;
        public DbSet<TipoMovimiento> TipoMovimiento { get; set; } = null!;
        public DbSet<TipoPermiso> TipoPermiso { get; set; } = null!;
        public DbSet<TipoRecurso> TipoRecurso { get; set; } = null!;
        public DbSet<TipoResolucion> TipoResolucion { get; set; } = null!;
        public DbSet<TipoSangre> TipoSangre { get; set; } = null!;
        public DbSet<TipoTrabajador> TipoTrabajador { get; set; } = null!;
        public DbSet<TipoTransaccion> TipoTransaccion { get; set; } = null!;
        public DbSet<TipoViv> TipoViv { get; set; } = null!;
        public DbSet<TitulosEmpleado> TitulosEmpleado { get; set; } = null!;
        public DbSet<UbicacionViv> UbicacionViv { get; set; } = null!;
        public DbSet<Ubigeo> Ubigeo { get; set; } = null!;
        public DbSet<UnidadMedida> UnidadMedida { get; set; } = null!;
        public DbSet<Usuario> Usuario { get; set; } = null!;
        public DbSet<Usuario_Rol> Usuario_Rol { get; set; } = null!;
        public DbSet<ViveCon> ViveCon { get; set; } = null!;
        public DbSet<Vivienda> Vivienda { get; set; } = null!;
    // Columnas de Persona sin propiedad en la entidad (los datos personales que antes estaban
    // en Empleado y Familiar). Las usan EmpleadoData y FamiliarData.
    internal static readonly (string Columna, Type Tipo)[] ColumnasPersona =
    {
        ("Sexo", typeof(string)), ("IdEstadoCivil", typeof(int?)), ("IdNacionalidad", typeof(int?)),
        ("IdTipoSangre", typeof(int?)), ("IdDiscapacidad", typeof(int?)), ("NumAutogenSalud", typeof(string)),
        ("NumRUC", typeof(string)), ("Brevete", typeof(string)), ("NumLibretaMilitar", typeof(string)),
        ("GradoInstruccion", typeof(string)), ("Profesion", typeof(string)), ("Ocupacion", typeof(string)),
        ("LugarTrabajo", typeof(string)), ("Procedencia", typeof(string)), ("Enfermedad", typeof(string)),
        ("TiempoEnfermedad", typeof(string)), ("Foto", typeof(byte[]))
    };

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AFP>(entity =>
            {
                entity.ToTable("AFP");
                entity.HasKey(e => e.IdAFP);
            });
            modelBuilder.Entity<Bitacora>(entity =>
            {
                entity.ToTable("Bitacora");
                entity.HasKey(e => e.IdBitacora);
            });
            modelBuilder.Entity<AguaViv>(entity =>
            {
                entity.ToTable("AguaViv");
                entity.HasKey(e => e.IdAgua);
            });
            modelBuilder.Entity<AlumbradoViv>(entity =>
            {
                entity.ToTable("AlumbradoViv");
                entity.HasKey(e => e.IdAlumbrado);
            });
            modelBuilder.Entity<Asistencia>(entity =>
            {
                entity.ToTable("Asistencia");
                entity.HasKey(e => new { e.Fecha, e.IdEmpleado });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.idHorario);
                entity.Property(e => e.Hor_Ent).HasConversion(v => Conversiones.AHora(v), v => Conversiones.DeHora(v));
                entity.Property(e => e.Hor_Sal).HasConversion(v => Conversiones.AHora(v), v => Conversiones.DeHora(v));
                entity.Property(e => e.Alm_Sal).HasConversion(v => Conversiones.AHora(v), v => Conversiones.DeHora(v));
                entity.Property(e => e.Alm_Ent).HasConversion(v => Conversiones.AHora(v), v => Conversiones.DeHora(v));
            });
            modelBuilder.Entity<Capacitacion>(entity =>
            {
                entity.ToTable("Capacitacion");
                entity.HasKey(e => e.IdCapacitacion);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.Property(e => e.NumHoras).HasConversion(v => Conversiones.AShort(v), v => Conversiones.DeNumero(v));
            });
            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.ToTable("Cargo");
                entity.HasKey(e => e.IdCargo);
                entity.Property(e => e.Descripcion).HasColumnName("Cargo");
            });
            modelBuilder.Entity<Comportamiento>(entity =>
            {
                entity.ToTable("Comportamiento");
                entity.HasKey(e => e.idComportamiento);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<TipoComportamiento>().WithMany().HasForeignKey(e => e.idTipoComportamiento);
            });
            modelBuilder.Entity<ConservacionViv>(entity =>
            {
                entity.ToTable("ConservacionViv");
                entity.HasKey(e => e.IdConservacion);
            });
            modelBuilder.Entity<Discapacidad>(entity =>
            {
                entity.ToTable("Discapacidad");
                entity.HasKey(e => e.IdDiscapacidad);
            });
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.ToTable("Empleado");
                entity.HasKey(e => e.IdEmpleado);
                entity.Property(e => e.Estado).HasConversion(v => Conversiones.AEstado(v), v => Conversiones.DeEstado(v));
                entity.Property(e => e.CargaFam).HasConversion(v => Conversiones.AByte(v), v => Conversiones.DeNumero(v));
                // En RRHH_Normalizado estos datos viven en otras tablas: los traduce EmpleadoData.
                //   Persona: datos personales | Empleado_Area: area actual | RRHH_AspSocio: aspectos sociales
                //   Resolucion: ingreso al Estado / a la institucion | vw_EmpleadoResumenFamiliar: NumHijos
                entity.Ignore(e => e.Year);
                entity.Ignore(e => e.idAreaOrganiz);
                foreach (var propiedad in EmpleadoData.DatosDePersona.Keys)
                    entity.Ignore(propiedad);
                foreach (var propiedad in EmpleadoData.DatosDeAspSocio)
                    entity.Ignore(propiedad);
                entity.Ignore(e => e.NumHijos);
                entity.Ignore(e => e.NumResoIngreEsta);
                entity.Ignore(e => e.FechaIngreEstado);
                entity.Ignore(e => e.FechaResolIngreEsta);
                entity.Ignore(e => e.NumResoIngreInstitu);
                entity.Ignore(e => e.FechaResoIngreInstitu);
                // Sin datos en la BD legada; no se migraron.
                entity.Ignore(e => e.IdSeguro);
                entity.Ignore(e => e.NroSeguro);
                entity.Ignore(e => e.NumResolCese);
                entity.Ignore(e => e.FechaResolCese);
                entity.Ignore(e => e.NumResol20530);
                entity.Ignore(e => e.FechaResol20530);
                entity.Ignore(e => e.NumResolPenMen);
                entity.Ignore(e => e.FechaResolPenMen);
                entity.Ignore(e => e.Userid);
                entity.HasOne<Persona>().WithMany().HasForeignKey(e => e.IdPersona);
                entity.HasOne<TipoTrabajador>().WithMany().HasForeignKey(e => e.IdTipoTrabajador);
                entity.HasOne<ViveCon>().WithMany().HasForeignKey(e => e.IdViveCon);
                entity.HasOne<RegimenPension>().WithMany().HasForeignKey(e => e.IdRegimenPen);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.idHorario);
                entity.HasOne<Local>().WithMany().HasForeignKey(e => e.idLocal);
                entity.HasOne<AFP>().WithMany().HasForeignKey(e => e.IdAFP);
                entity.HasOne<Cargo>().WithMany().HasForeignKey(e => e.IdCargo);
            });
            modelBuilder.Entity<Empleado_Area>(entity =>
            {
                entity.ToTable("Empleado_Area");
                entity.HasKey(e => new { e.Year, e.idAreaOrganiz, e.IdEmpleado });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<EstructOrganiz>().WithMany().HasForeignKey(e => new { e.Year, e.idAreaOrganiz });
            });
            modelBuilder.Entity<EstadoCivil>(entity =>
            {
                entity.ToTable("EstadoCivil");
                entity.HasKey(e => e.IdEstadoCivil);
            });
            modelBuilder.Entity<EstadoEstudio>(entity =>
            {
                entity.ToTable("EstadoEstudio");
                entity.HasKey(e => e.IdEstadoEstudio);
            });
            modelBuilder.Entity<EstructOrganiz>(entity =>
            {
                entity.ToTable("EstructOrganiz");
                entity.HasKey(e => new { e.Year, e.idAreaOrganiz });
                entity.HasOne<EstructOrganiz>().WithMany().HasPrincipalKey(e => new { e.Year, e.idAreaOrganiz }).HasForeignKey(e => new { e.YearDe, e.idDependeDe });
                // Presupuesto: sin datos en la BD legada, no se migro. NroTrabajaUO se calcula (vw_EstructOrganizDotacion).
                entity.Ignore(e => e.PresupAnual);
                entity.Ignore(e => e.PromedMensSueld);
                entity.Ignore(e => e.IdPresupuesto);
                entity.Ignore(e => e.InversPublic);
                entity.Ignore(e => e.ApruebaPedido);
                entity.Ignore(e => e.MontoAprobacion);
                entity.Ignore(e => e.PresupCompra);
                entity.Ignore(e => e.PresupUtilizado);
                entity.Ignore(e => e.NroTrabajaUO);
            });
            modelBuilder.Entity<EstudiosRealizado>(entity =>
            {
                entity.ToTable("EstudiosRealizado");
                entity.HasKey(e => new { e.IdTipoEstudios, e.IdEmpleado, e.Correlativo });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<EstadoEstudio>().WithMany().HasForeignKey(e => e.IdEstadoEstudio);
                entity.Property(e => e.AñoInicio).HasColumnName("FechaInicio");
                entity.Property(e => e.AñoTermino).HasColumnName("FechaTermino");
            });
            modelBuilder.Entity<ExcretasViv>(entity =>
            {
                entity.ToTable("ExcretasViv");
                entity.HasKey(e => e.IdExcretas);
            });
            modelBuilder.Entity<ExpLaboral>(entity =>
            {
                entity.ToTable("ExpLaboral");
                entity.HasKey(e => new { e.IdEmpleado, e.IdExpLab });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<TipoInstitucion>().WithMany().HasForeignKey(e => e.IdTipoInstitucion);
            });
            modelBuilder.Entity<Familia>(entity =>
            {
                entity.ToTable("Familia");
                entity.HasKey(e => new { e.IdFamilia, e.IdClase, e.IdGrupo });
            });
            modelBuilder.Entity<Familiar>(entity =>
            {
                entity.ToTable("Familiar");
                entity.HasKey(e => new { e.IdEmpleado, e.IdPersona });
                // Los datos personales del familiar viven en Persona y LaboraInstitucion se calcula
                // (vw_FamiliarDetalle): los traduce FamiliarData.
                foreach (var propiedad in FamiliarData.DatosDePersona.Keys)
                    entity.Ignore(propiedad);
                entity.Ignore(e => e.LaboraInstitucion);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<Persona>().WithMany().HasForeignKey(e => e.IdPersona);
            });
            modelBuilder.Entity<Horario>(entity =>
            {
                entity.ToTable("Horario");
                entity.HasKey(e => e.idHorario);
                entity.Property(e => e.Ingreso1).HasConversion(v => Conversiones.AHora(v), v => Conversiones.DeHora(v));
                entity.Property(e => e.Salida1).HasConversion(v => Conversiones.AHora(v), v => Conversiones.DeHora(v));
                entity.Property(e => e.Ingreso2).HasConversion(v => Conversiones.AHora(v), v => Conversiones.DeHora(v));
                entity.Property(e => e.Salida2).HasConversion(v => Conversiones.AHora(v), v => Conversiones.DeHora(v));
            });
            modelBuilder.Entity<HorarioTemporal>(entity =>
            {
                entity.ToTable("HorarioTemporal");
                entity.HasKey(e => new { e.idEmpleado, e.N });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.HorarioAsignado);
            });
            modelBuilder.Entity<Inconveniente>(entity =>
            {
                entity.ToTable("Inconveniente");
                entity.Property(e => e.Descripcion).HasColumnName("Inconveniente");
                entity.HasKey(e => new { e.IdObjetivo, e.Id_Actividad, e.IdInconveniente });
            });
            modelBuilder.Entity<Institucion>(entity =>
            {
                entity.ToTable("Institucion");
                entity.HasKey(e => e.IdInstitucion);
                entity.Property(e => e.IdInstitucion).ValueGeneratedNever();
            });
            modelBuilder.Entity<Limitacion>(entity =>
            {
                entity.ToTable("Limitacion");
                entity.HasKey(e => e.IdLimitacion);
                entity.Property(e => e.IdLimitacion).ValueGeneratedNever();
            });
            modelBuilder.Entity<Local>(entity =>
            {
                entity.ToTable("Local");
                entity.HasKey(e => e.idLocal);
            });
            modelBuilder.Entity<Marcacion>(entity =>
            {
                entity.ToTable("Marcacion");
                entity.HasKey(e => new { e.Fecha, e.IdEmpleado });
                // Estado '1N' se guarda separado en NroMarcacion + TipoMarcacion (lo traduce MarcacionData).
                entity.Ignore(e => e.Estado);
                entity.Property<byte>("NroMarcacion");
                entity.Property<string>("TipoMarcacion").HasMaxLength(1);
                entity.Property(e => e.Lugar).HasColumnName("CodDispositivo");
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.idHorario);
            });
            modelBuilder.Entity<MaterialViv>(entity =>
            {
                entity.ToTable("MaterialViv");
                entity.HasKey(e => e.IdMaterial);
            });
            modelBuilder.Entity<Motivo_Perm>(entity =>
            {
                entity.ToTable("Motivo_Perm");
                entity.HasKey(e => e.idMotivo);
                entity.HasOne<TipoPermiso>().WithMany().HasForeignKey(e => e.idTipoPermiso);
            });
            modelBuilder.Entity<MotivoBaja>(entity =>
            {
                entity.ToTable("MotivoBaja");
                entity.HasKey(e => e.idMotivoBaja);
            });
            modelBuilder.Entity<Nacionalidad>(entity =>
            {
                entity.ToTable("Nacionalidad");
                entity.HasKey(e => e.IdNacionalidad);
            });
            modelBuilder.Entity<PeriodoLaboral>(entity =>
            {
                entity.ToTable("PeriodoLaboral");
                entity.HasKey(e => new { e.IdEmpleado, e.NroPeriodo });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.ToTable("Permiso");
                entity.HasKey(e => new { e.IdEmpleado, e.NPermiso });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<Motivo_Perm>().WithMany().HasForeignKey(e => e.idMotivo);
                entity.Property(e => e.Hora_Sal).HasConversion(v => Conversiones.AHora(v), v => Conversiones.DeHora(v));
                entity.Property(e => e.Hora_Ret).HasConversion(v => Conversiones.AHora(v), v => Conversiones.DeHora(v));
            });
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("Persona");
                entity.HasKey(e => e.IdPersona);
                // Sin datos en la BD legada; no se migraron.
                // Obligatoria en RRHH_Normalizado: si la app no la indica, SQL Server pone 'N' (natural).
                entity.Property(e => e.TipoPersona).HasDefaultValue("N");
                entity.Ignore(e => e.Fax);
                entity.Ignore(e => e.FonoCentroLab);
                // Columnas que en RRHH_Nuevo estaban repetidas en Empleado y Familiar: aqui no tienen
                // propiedad en la entidad (shadow) y las leen/escriben EmpleadoData y FamiliarData.
                foreach (var (columna, tipo) in ColumnasPersona)
                    entity.Property(tipo, columna);
                entity.HasOne<TipoDocID>().WithMany().HasForeignKey(e => e.TipoDocID);
            });
            modelBuilder.Entity<RegAsisDiario>(entity =>
            {
                entity.ToTable("RegAsisDiario");
                entity.HasKey(e => new { e.Fecha, e.IdEmpleado });
                entity.Property(e => e.MinNormales).HasConversion(v => Conversiones.AShort(v), v => Conversiones.DeNumero(v));
                entity.Property(e => e.MinTarde).HasColumnName("SegundosTarde")
                    .HasConversion(v => Conversiones.ASegundos(v), v => Conversiones.DeSegundos(v));
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RegimenPension>(entity =>
            {
                entity.ToTable("RegimenPension");
                entity.HasKey(e => e.IdRegimenPen);
            });
            modelBuilder.Entity<Resolucion>(entity =>
            {
                entity.ToTable("Resolucion");
                entity.HasKey(e => new { e.idEmpleado, e.idResolucion });
                entity.HasOne<TipoResolucion>().WithMany().HasForeignKey(e => e.idTipoResolucion);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
                entity.Property<DateOnly?>("FechaVigencia");
            });
            modelBuilder.Entity<ResponsableXUO>(entity =>
            {
                entity.ToTable("ResponsableXUO");
                entity.HasKey(e => e.IdResponsable);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<EstructOrganiz>().WithMany().HasForeignKey(e => new { e.Year, e.idAreaOrganiz });
                // Se derivan del area (EstructOrganiz): los llena ResponsableXUOData.
                entity.Ignore(e => e.AreaOrganizacional);
                entity.Ignore(e => e.idDependeDe);
            });
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Rol");
                entity.HasKey(e => e.IdRol);
                entity.HasOne<SistemaOpcion>().WithMany().HasForeignKey(e => e.IdSistema);
            });
            modelBuilder.Entity<Rol_Acceso>(entity =>
            {
                entity.ToTable("Rol_Acceso");
                entity.HasKey(e => new { e.IdRol, e.IdSistemaOpcion });
                entity.HasOne<Rol>().WithMany().HasForeignKey(e => e.IdRol);
                entity.Ignore(e => e.Permiso);
            });
            modelBuilder.Entity<Rotacion>(entity =>
            {
                entity.ToTable("Rotacion");
                entity.HasKey(e => new { e.idEmpleado, e.idRotacion });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
                entity.HasOne<EstructOrganiz>().WithMany().HasForeignKey(e => new { e.Year, e.idAreaOrganiz });
            });
            modelBuilder.Entity<RRHH_AcudeEnferm>(entity =>
            {
                entity.ToTable("RRHH_AcudeEnferm");
                entity.HasKey(e => e.idAcudeEnferm);
            });
            modelBuilder.Entity<RRHH_Asegurado>(entity =>
            {
                entity.ToTable("RRHH_Asegurado");
                entity.HasKey(e => new { e.idEmpleado, e.NroBeneficiario });
                // Sexo y estado civil vienen de la Persona asegurada; VinculoFamiliar es el catalogo
                // VinculoAsegurado. Los traduce RRHH_AseguradoData.
                entity.Ignore(e => e.Sexo);
                entity.Ignore(e => e.idEstadoCivil);
                entity.Ignore(e => e.VinculoFamiliar);
                entity.Property<int?>("IdVinculoAsegurado");
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
                entity.HasOne<MotivoBaja>().WithMany().HasForeignKey(e => e.idMotivoBaja);
                entity.HasOne<Persona>().WithMany().HasForeignKey(e => e.idPersona);
            });
            modelBuilder.Entity<RRHH_AspSocio>(entity =>
            {
                entity.ToTable("RRHH_AspSocio");
                entity.HasKey(e => e.idAspSocio);
                // idPregunta1..3 no tenian catalogo de preguntas: no se migraron.
                entity.Ignore(e => e.idPregunta1);
                entity.Ignore(e => e.idPregunta2);
                entity.Ignore(e => e.idPregunta3);
                // Aspectos sociales del empleado (antes en Empleado): los escribe EmpleadoData.
                foreach (var columna in EmpleadoData.DatosDeAspSocio)
                    entity.Property<string>(columna);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RRHH_DinamicaFamiliar>(entity =>
            {
                entity.ToTable("RRHH_DinamicaFamiliar");
                entity.HasKey(e => e.idDinamica);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RRHH_Feriado>(entity =>
            {
                entity.ToTable("RRHH_Feriado");
                entity.HasKey(e => e.idFeriado);
            });
            modelBuilder.Entity<RRHH_RelacHermano>(entity =>
            {
                entity.ToTable("RRHH_RelacHermano");
                entity.HasKey(e => e.idRelacHermano);
            });
            modelBuilder.Entity<RRHH_RelacPareja>(entity =>
            {
                entity.ToTable("RRHH_RelacPareja");
                entity.HasKey(e => e.idRelacPareja);
            });
            modelBuilder.Entity<RRHH_RelacPH>(entity =>
            {
                entity.ToTable("RRHH_RelacPH");
                entity.HasKey(e => e.idRelacPH);
            });
            modelBuilder.Entity<RRHH_Salud>(entity =>
            {
                entity.ToTable("RRHH_Salud");
                entity.HasKey(e => e.idSalud);
                entity.HasOne<RRHH_AcudeEnferm>().WithMany().HasForeignKey(e => e.idAcudeEnferm);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RRHH_TipoFamilia>(entity =>
            {
                entity.ToTable("RRHH_TipoFamilia");
                entity.HasKey(e => e.idTipoFam);
            });
            modelBuilder.Entity<SistemaOpcion>(entity =>
            {
                entity.ToTable("SistemaOpcion");
                entity.HasKey(e => e.IdSistemaOpcion);
                entity.Ignore(e => e.ConClave);
                entity.Ignore(e => e.Clave);
            });
            modelBuilder.Entity<TenenciaViv>(entity =>
            {
                entity.ToTable("TenenciaViv");
                entity.HasKey(e => e.IdTenencia);
            });
            modelBuilder.Entity<TipoComportamiento>(entity =>
            {
                entity.ToTable("TipoComportamiento");
                entity.HasKey(e => e.idTipoComportamiento);
            });
            modelBuilder.Entity<TipoDoc>(entity =>
            {
                entity.ToTable("TipoDoc");
                entity.HasKey(e => new { e.IdTipodoc, e.Anio });
            });
            modelBuilder.Entity<TipoDocID>(entity =>
            {
                entity.ToTable("TipoDocID");
                entity.HasKey(e => e.Nombre);
                entity.Property(e => e.Nombre).HasColumnName("TipoDocID");
            });
            modelBuilder.Entity<TipoEstudio>(entity =>
            {
                entity.ToTable("TipoEstudio");
                entity.HasKey(e => e.IdTipoEstudios);
            });
            modelBuilder.Entity<TipoFamiliar>(entity =>
            {
                entity.ToTable("TipoFamiliar");
                entity.HasKey(e => e.IdTipoFam);
            });
            modelBuilder.Entity<TipoInstitucion>(entity =>
            {
                entity.ToTable("TipoInstitucion");
                entity.HasKey(e => e.IdTipoInstitucion);
            });
            modelBuilder.Entity<TipoMoneda>(entity =>
            {
                entity.ToTable("TipoMoneda");
                entity.HasKey(e => e.IdTipoMoneda);
                entity.Property(e => e.IdTipoMoneda).ValueGeneratedNever();
            });
            modelBuilder.Entity<TipoMovimiento>(entity =>
            {
                entity.ToTable("TipoMovimiento");
                entity.HasKey(e => new { e.IdTipoMovimiento, e.IdTipodoc });
            });
            modelBuilder.Entity<TipoPermiso>(entity =>
            {
                entity.ToTable("TipoPermiso");
                entity.HasKey(e => e.idTipoPermiso);
            });
            modelBuilder.Entity<TipoRecurso>(entity =>
            {
                entity.ToTable("TipoRecurso");
                entity.HasKey(e => e.IdTipoRecurso);
                entity.Property(e => e.IdTipoRecurso).ValueGeneratedNever();
            });
            modelBuilder.Entity<TipoResolucion>(entity =>
            {
                entity.ToTable("TipoResolucion");
                entity.HasKey(e => e.idTipoResolucion);
            });
            modelBuilder.Entity<TipoSangre>(entity =>
            {
                entity.ToTable("TipoSangre");
                entity.HasKey(e => e.IdTipoSangre);
            });
            modelBuilder.Entity<TipoTrabajador>(entity =>
            {
                entity.ToTable("TipoTrabajador");
                entity.HasKey(e => e.IdTipoTrabajador);
            });
            modelBuilder.Entity<TipoTransaccion>(entity =>
            {
                entity.ToTable("TipoTransaccion");
                entity.HasKey(e => new { e.IdTipoTransaccion, e.Anio });
            });
            modelBuilder.Entity<TipoViv>(entity =>
            {
                entity.ToTable("TipoViv");
                entity.HasKey(e => e.IdTipo);
            });
            modelBuilder.Entity<TitulosEmpleado>(entity =>
            {
                entity.ToTable("TitulosEmpleado");
                // La clave es IdTitulo (IDENTITY), sin propiedad en la entidad; la app sigue
                // identificando el titulo por NumColegiatura (TitulosEmpleadoData).
                entity.Property<int>("IdTitulo");
                entity.HasKey("IdTitulo");
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<UbicacionViv>(entity =>
            {
                entity.ToTable("UbicacionViv");
                entity.HasKey(e => e.IdUbicacion);
            });
            modelBuilder.Entity<Ubigeo>(entity =>
            {
                // Ubigeo se normalizo en Departamento / Provincia / Distrito; se lee de la vista.
                entity.ToView("vw_Ubigeo");
                entity.HasKey(e => e.IdUbigeo);
            });
            modelBuilder.Entity<UnidadMedida>(entity =>
            {
                entity.ToTable("UnidadMedida");
                entity.HasKey(e => e.IdUnidadMedida);
                entity.Property(e => e.IdUnidadMedida).ValueGeneratedNever();
            });
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");
                entity.HasKey(e => e.IdUsuario);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<Usuario_Rol>(entity =>
            {
                entity.ToTable("Usuario_Rol");
                entity.HasKey(e => new { e.IdUsuario, e.IdRol });
                entity.HasOne<Rol>().WithMany().HasForeignKey(e => e.IdRol);
                entity.HasOne<Usuario>().WithMany().HasForeignKey(e => e.IdUsuario);
            });
            modelBuilder.Entity<ViveCon>(entity =>
            {
                entity.ToTable("ViveCon");
                entity.HasKey(e => e.IdViveCon);
            });
            modelBuilder.Entity<Vivienda>(entity =>
            {
                entity.ToTable("Vivienda");
                entity.HasKey(e => e.IdVivienda);
                entity.Property(e => e.NumDormitorio).HasConversion(v => Conversiones.AByte(v), v => Conversiones.DeNumero(v));
                entity.Property(e => e.NumServHig).HasConversion(v => Conversiones.AByte(v), v => Conversiones.DeNumero(v));
                entity.Property(e => e.NumCompart).HasConversion(v => Conversiones.AByte(v), v => Conversiones.DeNumero(v));
                entity.Property(e => e.NumPerDorm).HasConversion(v => Conversiones.AByte(v), v => Conversiones.DeNumero(v));
                // Telefono, Cable, Internet y Otros son filas de ViviendaServicio (las traduce ViviendaData).
                foreach (var propiedad in ViviendaData.Servicios.Keys)
                    entity.Ignore(propiedad);
                entity.HasOne<AguaViv>().WithMany().HasForeignKey(e => e.IdAgua);
                entity.HasOne<AlumbradoViv>().WithMany().HasForeignKey(e => e.IdAlumbrado);
                entity.HasOne<ConservacionViv>().WithMany().HasForeignKey(e => e.IdConservacion);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<ExcretasViv>().WithMany().HasForeignKey(e => e.IdExcretas);
                entity.HasOne<MaterialViv>().WithMany().HasForeignKey(e => e.IdMaterial);
                entity.HasOne<TenenciaViv>().WithMany().HasForeignKey(e => e.IdTenencia);
                entity.HasOne<TipoViv>().WithMany().HasForeignKey(e => e.IdTipo);
                entity.HasOne<UbicacionViv>().WithMany().HasForeignKey(e => e.IdUbicacion);
            });

        // Tablas y vistas de RRHH_Normalizado sin entidad propia (ver Tablas.cs).
        modelBuilder.Entity<VinculoAseguradoFila>(entity =>
        {
            entity.ToTable("VinculoAsegurado");
            entity.HasKey(e => e.IdVinculoAsegurado);
        });
        modelBuilder.Entity<ViviendaServicioFila>(entity =>
        {
            entity.ToTable("ViviendaServicio");
            entity.HasKey(e => new { e.IdVivienda, e.IdServicio });
        });
        modelBuilder.Entity<FuncFamPreguntaFila>(entity =>
        {
            entity.ToTable("RRHH_FuncFamPregunta");
            entity.HasKey(e => e.IdPregunta);
            entity.Property(e => e.IdPregunta).ValueGeneratedNever();
        });
        modelBuilder.Entity<FuncFamRespuestaFila>(entity =>
        {
            entity.ToTable("RRHH_FuncFamRespuesta");
            entity.HasKey(e => new { e.IdEmpleado, e.IdPregunta });
        });
    }
}
