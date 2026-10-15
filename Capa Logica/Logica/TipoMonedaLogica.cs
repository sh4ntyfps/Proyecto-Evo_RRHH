using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoMonedaLogica : LogicaBase<TipoMoneda>
{
    private readonly TipoMonedaData _data;

    public TipoMonedaLogica(TipoMonedaData data) : base(data) => _data = data;

    public async Task<TipoMoneda?> Obtener(int IdTipoMonedaParam) => await _data.Obtener(IdTipoMonedaParam);
}
