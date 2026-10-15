using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class UsuarioData : RepositorioBase<Usuario>
{
    public UsuarioData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Usuario?> Obtener(int IdUsuarioParam) => ObtenerPorClave(IdUsuarioParam);

    public async Task<Usuario?> ObtenerPorLogin(string login) =>
        await Tabla.FirstOrDefaultAsync(u => u.Login != null && u.Login.Trim() == login);

    public async Task<List<Rol>> ObtenerRoles(int idUsuario) =>
        await _contexto.Set<Usuario_Rol>()
            .Where(ur => ur.IdUsuario == idUsuario)
            .Join(_contexto.Set<Rol>(), ur => ur.IdRol, r => r.IdRol, (ur, r) => r)
            .ToListAsync();
}
