using Capa_Entidades;

namespace Capa_Datos;

public class RegimenPensionData : RepositorioBase<RegimenPension>
{
    public RegimenPensionData(EvoRRLDbContext contexto) : base(contexto) { }
}
