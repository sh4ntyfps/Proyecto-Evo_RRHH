using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class PeriodoLaboralLogica : LogicaBase<PeriodoLaboral>
{
    private readonly IRepositorio<PeriodoLaboral> _data;

    public PeriodoLaboralLogica(IRepositorio<PeriodoLaboral> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<PeriodoLaboral?> Obtener(int idEmpleado, int nroPeriodo) => await _data.Obtener(idEmpleado, nroPeriodo);
}
