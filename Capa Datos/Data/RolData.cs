using Capa_Entidades;

namespace Capa_Datos;

public class RolData : RepositorioBase<Rol>
{
    public RolData(EvoRRLDbContext contexto) : base(contexto) { }
}
