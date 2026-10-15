using Microsoft.EntityFrameworkCore;
using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Datos;

public class UsuarioData : RepositorioBase<Usuario>, IUsuarioRepositorio
{
    public UsuarioData(EvoRRLDbContext contexto) : base(contexto) { }

    public async Task<Usuario?> ObtenerPorLogin(string login) =>
        await Tabla.FirstOrDefaultAsync(u => u.Login != null && u.Login.Trim() == login);

    public async Task<List<Rol>> ObtenerRoles(int idUsuario) =>
        await _contexto.Set<Usuario_Rol>()
            .Where(ur => ur.IdUsuario == idUsuario)
            .Join(_contexto.Set<Rol>(), ur => ur.IdRol, r => r.IdRol, (ur, r) => r)
            .ToListAsync();
}
