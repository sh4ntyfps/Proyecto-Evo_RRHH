using Capa_Entidades;

namespace Capa_Datos;

public class TipoPermisoData : RepositorioBase<TipoPermiso>
{
    public TipoPermisoData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<TipoPermiso?> Obtener(int idTipoPermisoParam) => ObtenerPorClave(idTipoPermisoParam);
}
