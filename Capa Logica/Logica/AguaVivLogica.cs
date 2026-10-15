using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class AguaVivLogica : LogicaBase<AguaViv>
{
    private readonly AguaVivData _data;

    public AguaVivLogica(AguaVivData data) : base(data) => _data = data;

    public async Task<AguaViv?> Obtener(int IdAguaParam) => await _data.Obtener(IdAguaParam);
}
