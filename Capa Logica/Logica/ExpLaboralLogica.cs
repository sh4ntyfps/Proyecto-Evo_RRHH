using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ExpLaboralLogica : LogicaBase<ExpLaboral>
{
    private readonly IRepositorio<ExpLaboral> _data;

    public ExpLaboralLogica(IRepositorio<ExpLaboral> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<ExpLaboral?> Obtener(int idEmpleado, int idExpLab) => await _data.Obtener(idEmpleado, idExpLab);
}
