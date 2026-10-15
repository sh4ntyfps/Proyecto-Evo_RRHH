using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class MotivoBajaLogica : LogicaBase<MotivoBaja>
{
    private readonly IRepositorio<MotivoBaja> _data;

    public MotivoBajaLogica(IRepositorio<MotivoBaja> data) : base(data) => _data = data;

    public async Task<MotivoBaja?> Obtener(int idMotivoBaja) => await _data.Obtener(idMotivoBaja);
}
