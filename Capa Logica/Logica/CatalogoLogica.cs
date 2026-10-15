using Capa_Entidades.Contratos;

namespace Capa_Logica;

// Mantenimiento generico de catalogos. La regla de negocio (que catalogos se pueden editar)
// vive aqui; leer y guardar los registros lo hace la Capa Datos.
public class CatalogoLogica
{
    private static readonly string[] Editable =
    {
        "Cargo", "MotivoBaja", "EstadoCivil", "TipoDocID", "TipoTrabajador",
        "TipoSangre", "Nacionalidad", "RegimenPension", "ViveCon", "TipoPermiso",
        "Motivo_Perm", "TipoEstudio", "TipoInstitucion", "Discapacidad",
        "Limitacion", "TipoComportamiento", "TipoResolucion", "Familia",
        "TipoFamiliar", "RRHH_TipoFamilia", "RRHH_RelacPareja", "RRHH_RelacPH",
        "RRHH_RelacHermano", "AFP", "AguaViv", "AlumbradoViv", "ConservacionViv",
        "ExcretasViv", "MaterialViv", "TenenciaViv", "TipoViv", "UbicacionViv",
        "RRHH_AcudeEnferm", "TipoMoneda", "TipoRecurso", "UnidadMedida", "Horario"
    };

    private readonly ICatalogoRepositorio _repositorio;

    public CatalogoLogica(ICatalogoRepositorio repositorio) => _repositorio = repositorio;

    public static bool EsEditable(string catalogo) => Editable.Contains(catalogo);

    public IReadOnlyList<CatalogoColumna> Columnas(string catalogo) => _repositorio.Columnas(catalogo);
    public string[] ClavesPrimarias(string catalogo) => _repositorio.ClavesPrimarias(catalogo);
    public Task<List<object>> Listar(string catalogo) => _repositorio.Listar(catalogo);
    public Task<object?> Obtener(string catalogo, string[] ids) => _repositorio.Obtener(catalogo, ids);

    public Task Crear(string catalogo, IReadOnlyDictionary<string, string> datos)
    {
        ValidarEditable(catalogo);
        return _repositorio.Crear(catalogo, datos);
    }

    public Task Actualizar(string catalogo, string[] ids, IReadOnlyDictionary<string, string> datos)
    {
        ValidarEditable(catalogo);
        return _repositorio.Actualizar(catalogo, ids, datos);
    }

    public Task Eliminar(string catalogo, string[] ids)
    {
        ValidarEditable(catalogo);
        return _repositorio.Eliminar(catalogo, ids);
    }

    private static void ValidarEditable(string catalogo)
    {
        if (!EsEditable(catalogo))
            throw new InvalidOperationException($"El catálogo {catalogo} no es editable.");
    }
}
