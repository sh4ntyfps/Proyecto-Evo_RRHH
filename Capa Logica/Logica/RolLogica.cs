using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RolLogica : LogicaBase<Rol>
{
    private readonly IRepositorio<Rol> _data;

    public RolLogica(IRepositorio<Rol> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Rol?> Obtener(int idRol) => await _data.Obtener(idRol);
}
