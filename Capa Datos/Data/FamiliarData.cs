using Capa_Entidades;

namespace Capa_Datos;

public class FamiliarData : RepositorioBase<Familiar>
{
    public FamiliarData(EvoRRLDbContext contexto) : base(contexto) { }
}
