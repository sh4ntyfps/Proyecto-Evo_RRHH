using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class PermisoLogica : LogicaBase<Permiso>
{
    private readonly IRepositorio<Permiso> _data;

    public PermisoLogica(IRepositorio<Permiso> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Permiso?> Obtener(int idEmpleado, int nPermiso) => await _data.Obtener(idEmpleado, nPermiso);
}
