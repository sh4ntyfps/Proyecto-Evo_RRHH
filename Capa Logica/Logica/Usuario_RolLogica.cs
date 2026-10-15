using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class Usuario_RolLogica : LogicaBase<Usuario_Rol>
{
    private readonly IRepositorio<Usuario_Rol> _data;

    public Usuario_RolLogica(IRepositorio<Usuario_Rol> data) : base(data) => _data = data;

    public async Task<Usuario_Rol?> Obtener(int idUsuario, int idRol) => await _data.Obtener(idUsuario, idRol);
}
