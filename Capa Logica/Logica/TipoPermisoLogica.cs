using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoPermisoLogica : LogicaBase<TipoPermiso>
{
    private readonly TipoPermisoData _data;

    public TipoPermisoLogica(TipoPermisoData data) : base(data) => _data = data;

    public async Task<TipoPermiso?> Obtener(int idTipoPermisoParam) => await _data.Obtener(idTipoPermisoParam);
}
