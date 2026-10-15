using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RegAsisDiarioLogica : LogicaBase<RegAsisDiario>
{
    private readonly IRepositorio<RegAsisDiario> _data;

    public RegAsisDiarioLogica(IRepositorio<RegAsisDiario> data) : base(data) => _data = data;

    public async Task<RegAsisDiario?> Obtener(DateTime fecha, int idEmpleado) => await _data.Obtener(fecha, idEmpleado);
}
