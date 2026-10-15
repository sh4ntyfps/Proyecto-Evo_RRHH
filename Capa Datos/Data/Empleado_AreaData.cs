using Capa_Entidades;

namespace Capa_Datos;

public class Empleado_AreaData : RepositorioBase<Empleado_Area>
{
    public Empleado_AreaData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Empleado_Area?> Obtener(int YearParam, int idAreaOrganizParam, int IdEmpleadoParam) => ObtenerPorClave(YearParam, idAreaOrganizParam, IdEmpleadoParam);
}
