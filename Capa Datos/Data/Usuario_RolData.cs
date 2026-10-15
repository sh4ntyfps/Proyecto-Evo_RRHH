using Capa_Entidades;

namespace Capa_Datos;

public class Usuario_RolData : RepositorioBase<Usuario_Rol>
{
    public Usuario_RolData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Usuario_Rol?> Obtener(int IdUsuarioParam, int IdRolParam) => ObtenerPorClave(IdUsuarioParam, IdRolParam);
}
