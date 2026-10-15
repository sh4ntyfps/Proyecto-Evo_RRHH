using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class CapacitacionLogica : LogicaBase<Capacitacion>
{
    private readonly IRepositorio<Capacitacion> _data;

    public CapacitacionLogica(IRepositorio<Capacitacion> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Capacitacion?> Obtener(int idCapacitacion) => await _data.Obtener(idCapacitacion);
}
