using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoResolucionLogica : LogicaBase<TipoResolucion>
{
    private readonly TipoResolucionData _data;

    public TipoResolucionLogica(TipoResolucionData data) : base(data) => _data = data;

    public async Task<TipoResolucion?> Obtener(int idTipoResolucionParam) => await _data.Obtener(idTipoResolucionParam);
}
