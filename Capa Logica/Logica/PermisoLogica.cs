using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class PermisoLogica : LogicaBase<Permiso>
{
    private readonly PermisoData _data;

    public PermisoLogica(PermisoData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Permiso?> Obtener(int IdEmpleadoParam, int NPermisoParam) => await _data.Obtener(IdEmpleadoParam, NPermisoParam);

    // Numero de permiso correlativo por empleado.
    public Task CrearConNuevoId(Permiso registro) =>
        CrearNumerado(registro, x => x.NPermiso, id => registro.NPermiso = id, x => x.IdEmpleado == registro.IdEmpleado);

    public Task<bool> Registrar(Permiso registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
