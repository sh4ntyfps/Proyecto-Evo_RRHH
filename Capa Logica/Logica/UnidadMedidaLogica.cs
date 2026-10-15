using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class UnidadMedidaLogica : LogicaBase<UnidadMedida>
{
    private readonly UnidadMedidaData _data;

    public UnidadMedidaLogica(UnidadMedidaData data) : base(data) => _data = data;

    public async Task<UnidadMedida?> Obtener(string IdUnidadMedidaParam) => await _data.Obtener(IdUnidadMedidaParam);
}
