using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ConservacionVivLogica : LogicaBase<ConservacionViv>
{
    private readonly ConservacionVivData _data;

    public ConservacionVivLogica(ConservacionVivData data) : base(data) => _data = data;

    public async Task<ConservacionViv?> Obtener(int IdConservacionParam) => await _data.Obtener(IdConservacionParam);
}
