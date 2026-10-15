using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class Usuario_RolLogica : LogicaBase<Usuario_Rol>
{
    private readonly Usuario_RolData _data;

    public Usuario_RolLogica(Usuario_RolData data) : base(data) => _data = data;

    public async Task<Usuario_Rol?> Obtener(int IdUsuarioParam, int IdRolParam) => await _data.Obtener(IdUsuarioParam, IdRolParam);
}
