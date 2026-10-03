using Capa_Entidades;

namespace Capa_Datos;

public class PeriodoLaboralData : RepositorioBase<PeriodoLaboral>
{
    public PeriodoLaboralData(EvoRRLDbContext contexto) : base(contexto) { }
}
