using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoComportamientoLogica : LogicaBase<TipoComportamiento>
{
    private readonly TipoComportamientoData _data;

    public TipoComportamientoLogica(TipoComportamientoData data) : base(data) => _data = data;

    public async Task<TipoComportamiento?> Obtener(int idTipoComportamientoParam) => await _data.Obtener(idTipoComportamientoParam);
}
