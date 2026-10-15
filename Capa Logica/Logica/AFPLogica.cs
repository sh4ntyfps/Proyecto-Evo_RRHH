using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class AFPLogica : LogicaBase<AFP>
{
    private readonly IRepositorio<AFP> _data;

    public AFPLogica(IRepositorio<AFP> data) : base(data) => _data = data;

    public async Task<AFP?> Obtener(int idAFP) => await _data.Obtener(idAFP);
}
