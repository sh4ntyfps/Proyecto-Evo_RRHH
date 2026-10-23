namespace Capa_Entidades.Contratos;

// EsAutonumerico: clave que genera la base de datos (IDENTITY); no se pide al crear.
public record CatalogoColumna(string Nombre, string Tipo, bool EsClave, bool EsArchivo, bool EsAutonumerico = false);

// Mantenimiento generico de catalogos por nombre de entidad (ej. "Cargo").
public interface ICatalogoRepositorio
{
    IReadOnlyList<CatalogoColumna> Columnas(string catalogo);

    string[] ClavesPrimarias(string catalogo);

    Task<List<object>> Listar(string catalogo);

    Task<object?> Obtener(string catalogo, string[] ids);

    Task Crear(string catalogo, IReadOnlyDictionary<string, string> datos);

    Task Actualizar(string catalogo, string[] ids, IReadOnlyDictionary<string, string> datos);

    Task Eliminar(string catalogo, string[] ids);
}
