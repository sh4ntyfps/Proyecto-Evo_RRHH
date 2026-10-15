using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoMonedaLogica : LogicaBase<TipoMoneda>
{
    private readonly IRepositorio<TipoMoneda> _data;

    public TipoMonedaLogica(IRepositorio<TipoMoneda> data) : base(data) => _data = data;

    public async Task<TipoMoneda?> Obtener(int idTipoMoneda) => await _data.Obtener(idTipoMoneda);
}
