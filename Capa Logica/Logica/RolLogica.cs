using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RolLogica : LogicaBase<Rol>
{
    private readonly RolData _data;

    public RolLogica(RolData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Rol?> Obtener(int IdRolParam) => await _data.Obtener(IdRolParam);

    public Task CrearConNuevoId(Rol registro) =>
        CrearNumerado(registro, x => x.IdRol, id => registro.IdRol = id);

    public Task<bool> Registrar(Rol registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
