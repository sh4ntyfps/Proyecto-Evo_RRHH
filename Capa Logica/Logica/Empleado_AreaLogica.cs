using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class Empleado_AreaLogica : LogicaBase<Empleado_Area>
{
    private readonly Empleado_AreaData _data;

    public Empleado_AreaLogica(Empleado_AreaData data) : base(data) => _data = data;

    public async Task<Empleado_Area?> Obtener(int YearParam, int idAreaOrganizParam, int IdEmpleadoParam) => await _data.Obtener(YearParam, idAreaOrganizParam, IdEmpleadoParam);
}
