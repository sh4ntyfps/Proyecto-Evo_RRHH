using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoMovimientoLogica : LogicaBase<TipoMovimiento>
{
    private readonly TipoMovimientoData _data;

    public TipoMovimientoLogica(TipoMovimientoData data) : base(data) => _data = data;

    public async Task<TipoMovimiento?> Obtener(string IdTipoMovimientoParam, string IdTipodocParam) => await _data.Obtener(IdTipoMovimientoParam, IdTipodocParam);
}
