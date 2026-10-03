using Capa_Datos;
using Capa_Entidades;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Evo_RRLL.Servicios;

/// <summary>
/// Unica responsabilidad: garantizar que la base de datos tenga el usuario administrador
/// inicial con su rol (SRP). Antes esta regla estaba escrita dentro de Program.cs, que
/// asi mezclaba el arranque de la aplicacion con una regla de negocio.
/// El hash de la clave se delega en <see cref="ServicioClaves"/> para no repetir el
/// algoritmo que ya usa el inicio de sesion (DRY).
/// </summary>
public class SembradorAdministrador
{
    private const string LoginAdmin = "admin";
    private const string ClaveInicialPorDefecto = "Admin.2026";
    private const string DescripcionAdmin = "Administrador del Sistema";
    private const string RolAdministrador = "Administrador";
    private const string SistemaPorDefecto = "GRLL";

    private readonly EvoRRLDbContext _contexto;
    private readonly ServicioClaves _claves;
    private readonly string _claveInicial;

    public SembradorAdministrador(EvoRRLDbContext contexto, ServicioClaves claves, IConfiguration configuracion)
    {
        _contexto = contexto;
        _claves = claves;
        _claveInicial = configuracion["Seguridad:AdminClave"] ?? ClaveInicialPorDefecto;
    }

    /// <summary>Crea el administrador y su rol solo si todavia no existen.</summary>
    public async Task SembrarAsync()
    {
        if (await _contexto.Set<Usuario>().AnyAsync(u => u.Login != null && u.Login.Trim() == LoginAdmin))
            return;

        var admin = new Usuario
        {
            IdUsuario = await SiguienteIdUsuarioAsync(),
            Login = LoginAdmin,
            Descripcion = DescripcionAdmin,
            Fecha = DateTime.Now,
            Estado = true,
            PasswordHash = _claves.Generar(_claveInicial)
        };

        var rol = await ObtenerOCrearRolAdministradorAsync();

        _contexto.Set<Usuario>().Add(admin);
        await _contexto.SaveChangesAsync();
        _contexto.Set<Usuario_Rol>().Add(new Usuario_Rol { IdUsuario = admin.IdUsuario, IdRol = rol.IdRol });
        await _contexto.SaveChangesAsync();
    }

    private async Task<int> SiguienteIdUsuarioAsync() =>
        await _contexto.Set<Usuario>().AnyAsync()
            ? await _contexto.Set<Usuario>().MaxAsync(u => u.IdUsuario) + 1
            : 1;

    private async Task<Rol> ObtenerOCrearRolAdministradorAsync()
    {
        var rol = await _contexto.Set<Rol>()
            .FirstOrDefaultAsync(r => r.Descripcion != null && r.Descripcion.Trim().Equals(RolAdministrador));
        if (rol is not null)
            return rol;

        var siguienteId = await _contexto.Set<Rol>().AnyAsync()
            ? await _contexto.Set<Rol>().MaxAsync(r => r.IdRol) + 1
            : 1;
        rol = new Rol { IdRol = siguienteId, Descripcion = RolAdministrador, IdSistema = SistemaPorDefecto };
        _contexto.Set<Rol>().Add(rol);
        return rol;
    }
}
