using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoPermisoLogica : LogicaBase<TipoPermiso>
{
    private readonly IRepositorio<TipoPermiso> _data;

    public TipoPermisoLogica(IRepositorio<TipoPermiso> data) : base(data) => _data = data;

    public async Task<TipoPermiso?> Obtener(int idTipoPermiso) => await _data.Obtener(idTipoPermiso);
}
