using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoDocLogica : LogicaBase<TipoDoc>
{
    private readonly TipoDocData _data;

    public TipoDocLogica(TipoDocData data) : base(data) => _data = data;

    public async Task<TipoDoc?> Obtener(string IdTipodocParam, string AnioParam) => await _data.Obtener(IdTipodocParam, AnioParam);
}
