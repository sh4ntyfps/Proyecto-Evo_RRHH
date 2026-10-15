using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class UsuarioLogica : LogicaBase<Usuario>
{
    private readonly IUsuarioRepositorio _data;

    public UsuarioLogica(IUsuarioRepositorio data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Usuario?> Obtener(int idUsuario) => await _data.Obtener(idUsuario);
    public async Task<Usuario?> ObtenerPorLogin(string login) => await _data.ObtenerPorLogin(login);
    public async Task<List<Rol>> ObtenerRoles(int idUsuario) => await _data.ObtenerRoles(idUsuario);
}
