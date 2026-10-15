using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoDocIDLogica : LogicaBase<TipoDocID>
{
    private readonly TipoDocIDData _data;

    public TipoDocIDLogica(TipoDocIDData data) : base(data) => _data = data;

    public async Task<TipoDocID?> Obtener(int TipoDocIDParam) => await _data.Obtener(TipoDocIDParam);
}
