using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class UsuarioLogica : LogicaBase<Usuario>
{
    private readonly UsuarioData _data;

    public UsuarioLogica(UsuarioData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Usuario?> Obtener(int IdUsuarioParam) => await _data.Obtener(IdUsuarioParam);
    public async Task<Usuario?> ObtenerPorLogin(string login) => await _data.ObtenerPorLogin(login);
    public async Task<List<Rol>> ObtenerRoles(int idUsuario) => await _data.ObtenerRoles(idUsuario);

    public Task CrearConNuevoId(Usuario registro) =>
        CrearNumerado(registro, x => x.IdUsuario, id => registro.IdUsuario = id);

    public Task<bool> Registrar(Usuario registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
