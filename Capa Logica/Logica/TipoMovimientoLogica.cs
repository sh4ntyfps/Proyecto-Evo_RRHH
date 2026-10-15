using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoMovimientoLogica : LogicaBase<TipoMovimiento>
{
    private readonly IRepositorio<TipoMovimiento> _data;

    public TipoMovimientoLogica(IRepositorio<TipoMovimiento> data) : base(data) => _data = data;

    public async Task<TipoMovimiento?> Obtener(string idTipoMovimiento, string idTipodoc) => await _data.Obtener(idTipoMovimiento, idTipodoc);
}
