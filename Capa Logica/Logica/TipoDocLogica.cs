using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoDocLogica : LogicaBase<TipoDoc>
{
    private readonly IRepositorio<TipoDoc> _data;

    public TipoDocLogica(IRepositorio<TipoDoc> data) : base(data) => _data = data;

    public async Task<TipoDoc?> Obtener(string idTipodoc, string anio) => await _data.Obtener(idTipodoc, anio);
}
