using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RegAsisDiarioLogica : LogicaBase<RegAsisDiario>
{
    private readonly RegAsisDiarioData _data;

    public RegAsisDiarioLogica(RegAsisDiarioData data) : base(data) => _data = data;

    public async Task<RegAsisDiario?> Obtener(DateTime FechaParam, int IdEmpleadoParam) => await _data.Obtener(FechaParam, IdEmpleadoParam);
}
