namespace Capa_Entidades.Contratos;

// Consultas propias de algunas entidades, ademas del CRUD de IRepositorio<T>.

// Contratos minimos que necesita EmpleadoServicio: solo declaran lo que se usa (ISP, IoC).
// Permiten sustituir la base de datos por un doble en las pruebas.
public interface IEmpleadoData
{
    Task<List<Empleado>> Listar();
}

public interface IPersonaData
{
    Task<List<Persona>> Listar();
}

public interface IUsuarioRepositorio : IRepositorio<Usuario>
{
    Task<Usuario?> ObtenerPorLogin(string login);

    Task<List<Rol>> ObtenerRoles(int idUsuario);
}

public interface IAsistenciaRepositorio : IRepositorio<Asistencia>
{
    Task<int> ContarEntreFechas(DateTime desde, DateTime hasta);
}

public interface IBitacoraRepositorio
{
    Task Registrar(Bitacora registro);

    Task<List<Bitacora>> Ultimas(int cantidad);
}
