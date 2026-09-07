using Microsoft.EntityFrameworkCore;
using Capa_Entidades;
namespace Capa_Datos;
public class EvoRRLDbContext : DbContext
{
    public EvoRRLDbContext(DbContextOptions<EvoRRLDbContext> options) : base(options) { }
        public DbSet<AFP> AFP { get; set; } = null!;
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
        public DbSet<RRHH_FuncFam> RRHH_FuncFam { get; set; } = null!;
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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AFP>(entity =>
            {
            });
            modelBuilder.Entity<AguaViv>(entity =>
            {
            });
            modelBuilder.Entity<AlumbradoViv>(entity =>
            {
            });
            modelBuilder.Entity<Asistencia>(entity =>
            {
                entity.HasKey(e => new { e.Fecha, e.IdEmpleado });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.idHorario);
            });
            modelBuilder.Entity<Capacitacion>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<Cargo>(entity =>
            {
            });
            modelBuilder.Entity<Comportamiento>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<TipoComportamiento>().WithMany().HasForeignKey(e => e.idTipoComportamiento);
            });
            modelBuilder.Entity<ConservacionViv>(entity =>
            {
            });
            modelBuilder.Entity<Discapacidad>(entity =>
            {
            });
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasOne<TipoSangre>().WithMany().HasForeignKey(e => e.IdTipoSangre);
                entity.HasOne<TipoTrabajador>().WithMany().HasForeignKey(e => e.IdTipoTrabajador);
                entity.HasOne<Nacionalidad>().WithMany().HasForeignKey(e => e.IdNacionalidad);
                entity.HasOne<ViveCon>().WithMany().HasForeignKey(e => e.IdViveCon);
                entity.HasOne<RegimenPension>().WithMany().HasForeignKey(e => e.IdRegimenPen);
                entity.HasOne<EstadoCivil>().WithMany().HasForeignKey(e => e.IdEstadoCivil);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.idHorario);
                entity.HasOne<Local>().WithMany().HasForeignKey(e => e.idLocal);
                entity.HasOne<EstructOrganiz>().WithMany().HasForeignKey(e => new { e.Year, e.idAreaOrganiz });
                entity.HasOne<AFP>().WithMany().HasForeignKey(e => e.IdAFP);
                entity.HasOne<Cargo>().WithMany().HasForeignKey(e => e.IdCargo);
                entity.HasOne<Discapacidad>().WithMany().HasForeignKey(e => e.IdDiscapacidad);
            });
            modelBuilder.Entity<Empleado_Area>(entity =>
            {
                entity.HasKey(e => new { e.Year, e.idAreaOrganiz, e.IdEmpleado });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<EstructOrganiz>().WithMany().HasForeignKey(e => new { e.Year, e.idAreaOrganiz });
            });
            modelBuilder.Entity<EstadoCivil>(entity =>
            {
            });
            modelBuilder.Entity<EstadoEstudio>(entity =>
            {
            });
            modelBuilder.Entity<EstructOrganiz>(entity =>
            {
                entity.HasKey(e => new { e.Year, e.idAreaOrganiz });
                entity.HasOne<EstructOrganiz>().WithMany().HasPrincipalKey(e => new { e.Year, e.idAreaOrganiz }).HasForeignKey(e => new { e.YearDe, e.idDependeDe });
            });
            modelBuilder.Entity<EstudiosRealizado>(entity =>
            {
                entity.HasKey(e => new { e.IdTipoEstudios, e.IdEmpleado, e.Correlativo });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<EstadoEstudio>().WithMany().HasForeignKey(e => e.IdEstadoEstudio);
            });
            modelBuilder.Entity<ExcretasViv>(entity =>
            {
            });
            modelBuilder.Entity<ExpLaboral>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleado, e.IdExpLab });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<TipoInstitucion>().WithMany().HasForeignKey(e => e.IdTipoInstitucion);
            });
            modelBuilder.Entity<Familia>(entity =>
            {
                entity.HasKey(e => new { e.IdFamilia, e.IdClase, e.IdGrupo });
            });
            modelBuilder.Entity<Familiar>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleado, e.IdPersona });
                entity.HasOne<Discapacidad>().WithMany().HasForeignKey(e => e.IdDiscapacidad);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<EstadoCivil>().WithMany().HasForeignKey(e => e.IdEstadoCivil);
            });
            modelBuilder.Entity<Horario>(entity =>
            {
            });
            modelBuilder.Entity<HorarioTemporal>(entity =>
            {
                entity.HasKey(e => new { e.idEmpleado, e.N });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.HorarioAsignado);
            });
            modelBuilder.Entity<Inconveniente>(entity =>
            {
                entity.HasKey(e => new { e.IdObjetivo, e.Id_Actividad, e.IdInconveniente });
            });
            modelBuilder.Entity<Institucion>(entity =>
            {
            });
            modelBuilder.Entity<Limitacion>(entity =>
            {
            });
            modelBuilder.Entity<Local>(entity =>
            {
            });
            modelBuilder.Entity<Marcacion>(entity =>
            {
                entity.HasKey(e => new { e.Fecha, e.IdEmpleado });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<Horario>().WithMany().HasForeignKey(e => e.idHorario);
            });
            modelBuilder.Entity<MaterialViv>(entity =>
            {
            });
            modelBuilder.Entity<Motivo_Perm>(entity =>
            {
                entity.HasOne<TipoPermiso>().WithMany().HasForeignKey(e => e.idTipoPermiso);
            });
            modelBuilder.Entity<MotivoBaja>(entity =>
            {
            });
            modelBuilder.Entity<Nacionalidad>(entity =>
            {
            });
            modelBuilder.Entity<PeriodoLaboral>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleado, e.NroPeriodo });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(e => new { e.IdEmpleado, e.NPermiso });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<Motivo_Perm>().WithMany().HasForeignKey(e => e.idMotivo);
            });
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasOne<Ubigeo>().WithMany().HasForeignKey(e => e.IdUbigeo);
                entity.HasOne<TipoDocID>().WithMany().HasForeignKey(e => e.TipoDocID);
            });
            modelBuilder.Entity<RegAsisDiario>(entity =>
            {
                entity.HasKey(e => new { e.Fecha, e.IdEmpleado });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RegimenPension>(entity =>
            {
            });
            modelBuilder.Entity<Resolucion>(entity =>
            {
                entity.HasKey(e => new { e.idEmpleado, e.idResolucion });
                entity.HasOne<TipoResolucion>().WithMany().HasForeignKey(e => e.idTipoResolucion);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
            });
            modelBuilder.Entity<ResponsableXUO>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
                entity.HasOne<EstructOrganiz>().WithMany().HasForeignKey(e => new { e.Year, e.idAreaOrganiz });
            });
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasOne<SistemaOpcion>().WithMany().HasForeignKey(e => e.IdSistema);
            });
            modelBuilder.Entity<Rol_Acceso>(entity =>
            {
                entity.HasKey(e => new { e.IdRol, e.IdSistemaOpcion });
                entity.HasOne<Rol>().WithMany().HasForeignKey(e => e.IdRol);
            });
            modelBuilder.Entity<Rotacion>(entity =>
            {
                entity.HasKey(e => new { e.idEmpleado, e.idRotacion });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
                entity.HasOne<EstructOrganiz>().WithMany().HasForeignKey(e => new { e.Year, e.idAreaOrganiz });
            });
            modelBuilder.Entity<RRHH_AcudeEnferm>(entity =>
            {
            });
            modelBuilder.Entity<RRHH_Asegurado>(entity =>
            {
                entity.HasKey(e => new { e.idEmpleado, e.NroBeneficiario });
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.idEmpleado);
                entity.HasOne<EstadoCivil>().WithMany().HasForeignKey(e => e.idEstadoCivil);
                entity.HasOne<MotivoBaja>().WithMany().HasForeignKey(e => e.idMotivoBaja);
                entity.HasOne<Persona>().WithMany().HasForeignKey(e => e.idPersona);
            });
            modelBuilder.Entity<RRHH_AspSocio>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RRHH_DinamicaFamiliar>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RRHH_Feriado>(entity =>
            {
            });
            modelBuilder.Entity<RRHH_FuncFam>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RRHH_RelacHermano>(entity =>
            {
            });
            modelBuilder.Entity<RRHH_RelacPareja>(entity =>
            {
            });
            modelBuilder.Entity<RRHH_RelacPH>(entity =>
            {
            });
            modelBuilder.Entity<RRHH_Salud>(entity =>
            {
                entity.HasOne<RRHH_AcudeEnferm>().WithMany().HasForeignKey(e => e.idAcudeEnferm);
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<RRHH_TipoFamilia>(entity =>
            {
            });
            modelBuilder.Entity<SistemaOpcion>(entity =>
            {
            });
            modelBuilder.Entity<TenenciaViv>(entity =>
            {
            });
            modelBuilder.Entity<TipoComportamiento>(entity =>
            {
            });
            modelBuilder.Entity<TipoDoc>(entity =>
            {
                entity.HasKey(e => new { e.IdTipodoc, e.Anio });
            });
            modelBuilder.Entity<TipoDocID>(entity =>
            {
            });
            modelBuilder.Entity<TipoEstudio>(entity =>
            {
            });
            modelBuilder.Entity<TipoFamiliar>(entity =>
            {
            });
            modelBuilder.Entity<TipoInstitucion>(entity =>
            {
            });
            modelBuilder.Entity<TipoMoneda>(entity =>
            {
            });
            modelBuilder.Entity<TipoMovimiento>(entity =>
            {
                entity.HasKey(e => new { e.IdTipoMovimiento, e.IdTipodoc });
            });
            modelBuilder.Entity<TipoPermiso>(entity =>
            {
            });
            modelBuilder.Entity<TipoRecurso>(entity =>
            {
            });
            modelBuilder.Entity<TipoResolucion>(entity =>
            {
            });
            modelBuilder.Entity<TipoSangre>(entity =>
            {
            });
            modelBuilder.Entity<TipoTrabajador>(entity =>
            {
            });
            modelBuilder.Entity<TipoTransaccion>(entity =>
            {
                entity.HasKey(e => new { e.IdTipoTransaccion, e.Anio });
            });
            modelBuilder.Entity<TipoViv>(entity =>
            {
            });
            modelBuilder.Entity<TitulosEmpleado>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<UbicacionViv>(entity =>
            {
            });
            modelBuilder.Entity<Ubigeo>(entity =>
            {
            });
            modelBuilder.Entity<UnidadMedida>(entity =>
            {
            });
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasOne<Empleado>().WithMany().HasForeignKey(e => e.IdEmpleado);
            });
            modelBuilder.Entity<Usuario_Rol>(entity =>
            {
                entity.HasKey(e => new { e.IdUsuario, e.IdRol });
                entity.HasOne<Rol>().WithMany().HasForeignKey(e => e.IdRol);
                entity.HasOne<Usuario>().WithMany().HasForeignKey(e => e.IdUsuario);
            });
            modelBuilder.Entity<ViveCon>(entity =>
            {
            });
            modelBuilder.Entity<Vivienda>(entity =>
            {
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
    }
}
