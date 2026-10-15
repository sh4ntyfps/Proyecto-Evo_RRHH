using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoTransaccionLogica : LogicaBase<TipoTransaccion>
{
    private readonly TipoTransaccionData _data;

    public TipoTransaccionLogica(TipoTransaccionData data) : base(data) => _data = data;

    public async Task<TipoTransaccion?> Obtener(string IdTipoTransaccionParam, string AnioParam) => await _data.Obtener(IdTipoTransaccionParam, AnioParam);
}
