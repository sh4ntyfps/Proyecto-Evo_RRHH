namespace Capa_Entidades.Contratos;

// Acceso a datos de una entidad. La Capa Logica solo conoce este contrato; en que tablas
// se guarda la entidad y como se generan sus claves lo decide la Capa Datos.
public interface IRepositorio<T> where T : class
{
    Task<List<T>> Listar();

    // Las claves van en el orden de la clave primaria de la entidad.
    Task<T?> Obtener(params object[] claves);

    // Si la entidad tiene clave generada (correlativo o IDENTITY), la Capa Datos la asigna
    // y la deja cargada en el registro.
    Task Crear(T registro);

    Task Actualizar(T registro);

    Task Eliminar(T registro);
}
