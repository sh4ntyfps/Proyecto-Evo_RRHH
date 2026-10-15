using Capa_Entidades;

namespace Capa_Datos;

public class ExpLaboralData : RepositorioBase<ExpLaboral>
{
    public ExpLaboralData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<ExpLaboral?> Obtener(int IdEmpleadoParam, int IdExpLabParam) => ObtenerPorClave(IdEmpleadoParam, IdExpLabParam);
}
