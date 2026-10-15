using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class UnidadMedidaLogica : LogicaBase<UnidadMedida>
{
    private readonly IRepositorio<UnidadMedida> _data;

    public UnidadMedidaLogica(IRepositorio<UnidadMedida> data) : base(data) => _data = data;

    public async Task<UnidadMedida?> Obtener(string idUnidadMedida) => await _data.Obtener(idUnidadMedida);
}
