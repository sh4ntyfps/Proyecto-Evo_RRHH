using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RegimenPensionLogica : LogicaBase<RegimenPension>
{
    private readonly RegimenPensionData _data;

    public RegimenPensionLogica(RegimenPensionData data) : base(data) => _data = data;

    public async Task<RegimenPension?> Obtener(int IdRegimenPenParam) => await _data.Obtener(IdRegimenPenParam);
}
