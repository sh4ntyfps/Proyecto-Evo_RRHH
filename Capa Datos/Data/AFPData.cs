using Capa_Entidades;

namespace Capa_Datos;

public class AFPData : RepositorioBase<AFP>
{
    public AFPData(EvoRRLDbContext contexto) : base(contexto) { }
}
