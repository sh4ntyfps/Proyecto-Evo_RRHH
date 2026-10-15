using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RotacionLogica : LogicaBase<Rotacion>
{
    private readonly IRepositorio<Rotacion> _data;

    public RotacionLogica(IRepositorio<Rotacion> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Rotacion?> Obtener(int idEmpleado, int idRotacion) => await _data.Obtener(idEmpleado, idRotacion);
}
