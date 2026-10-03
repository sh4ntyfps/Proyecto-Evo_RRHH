using Capa_Entidades;

namespace Capa_Datos;

public class FamiliaData : RepositorioBase<Familia>
{
    public FamiliaData(EvoRRLDbContext contexto) : base(contexto) { }
}
