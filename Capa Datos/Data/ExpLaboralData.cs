using Capa_Entidades;

namespace Capa_Datos;

public class ExpLaboralData : RepositorioBase<ExpLaboral>
{
    public ExpLaboralData(EvoRRLDbContext contexto) : base(contexto) { }
}
