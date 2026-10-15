using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoVivLogica : LogicaBase<TipoViv>
{
    private readonly TipoVivData _data;

    public TipoVivLogica(TipoVivData data) : base(data) => _data = data;

    public async Task<TipoViv?> Obtener(int IdTipoParam) => await _data.Obtener(IdTipoParam);
}
