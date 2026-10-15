using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ConservacionVivLogica : LogicaBase<ConservacionViv>
{
    private readonly IRepositorio<ConservacionViv> _data;

    public ConservacionVivLogica(IRepositorio<ConservacionViv> data) : base(data) => _data = data;

    public async Task<ConservacionViv?> Obtener(int idConservacion) => await _data.Obtener(idConservacion);
}
