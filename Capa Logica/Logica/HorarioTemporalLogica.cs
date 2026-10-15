using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class HorarioTemporalLogica : LogicaBase<HorarioTemporal>
{
    private readonly IRepositorio<HorarioTemporal> _data;

    public HorarioTemporalLogica(IRepositorio<HorarioTemporal> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<HorarioTemporal?> Obtener(int idEmpleado, int n) => await _data.Obtener(idEmpleado, n);
}
