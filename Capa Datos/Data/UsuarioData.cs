using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class UsuarioData : RepositorioBase<Usuario>
{
    public UsuarioData(EvoRRLDbContext contexto) : base(contexto) { }

    public async Task<Usuario?> ObtenerPorLogin(string login) =>
        await Conjunto.FirstOrDefaultAsync(u => u.Login != null && u.Login.Trim() == login);

    public async Task<List<Rol>> ObtenerRoles(int idUsuario) =>
        await Contexto.Set<Usuario_Rol>()
            .Where(ur => ur.IdUsuario == idUsuario)
            .Join(Contexto.Set<Rol>(), ur => ur.IdRol, r => r.IdRol, (ur, r) => r)
            .ToListAsync();
}
