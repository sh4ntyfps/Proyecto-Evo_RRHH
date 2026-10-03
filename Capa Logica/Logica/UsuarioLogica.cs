using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class UsuarioLogica : LogicaBase<Usuario, UsuarioData>
{
    public UsuarioLogica(UsuarioData datos) : base(datos) { }

    public async Task<Usuario?> ObtenerPorLogin(string login) => await Datos.ObtenerPorLogin(login);

    public async Task<List<Rol>> ObtenerRoles(int idUsuario) => await Datos.ObtenerRoles(idUsuario);
}
