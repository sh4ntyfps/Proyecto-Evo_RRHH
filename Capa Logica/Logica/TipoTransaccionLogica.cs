using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoTransaccionLogica : LogicaBase<TipoTransaccion>
{
    private readonly IRepositorio<TipoTransaccion> _data;

    public TipoTransaccionLogica(IRepositorio<TipoTransaccion> data) : base(data) => _data = data;

    public async Task<TipoTransaccion?> Obtener(string idTipoTransaccion, string anio) => await _data.Obtener(idTipoTransaccion, anio);
}
