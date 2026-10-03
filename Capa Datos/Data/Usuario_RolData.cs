using Capa_Entidades;

namespace Capa_Datos;

public class Usuario_RolData : RepositorioBase<Usuario_Rol>
{
    public Usuario_RolData(EvoRRLDbContext contexto) : base(contexto) { }
}
