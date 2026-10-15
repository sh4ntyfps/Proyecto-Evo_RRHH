using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class Empleado_AreaLogica : LogicaBase<Empleado_Area>
{
    private readonly IRepositorio<Empleado_Area> _data;

    public Empleado_AreaLogica(IRepositorio<Empleado_Area> data) : base(data) => _data = data;

    public async Task<Empleado_Area?> Obtener(int year, int idAreaOrganiz, int idEmpleado) => await _data.Obtener(year, idAreaOrganiz, idEmpleado);
}
