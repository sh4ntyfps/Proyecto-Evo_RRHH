using Capa_Entidades;

namespace Capa_Datos;

public class PermisoData : RepositorioBase<Permiso>
{
    public PermisoData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Permiso?> Obtener(int IdEmpleadoParam, int NPermisoParam) => ObtenerPorClave(IdEmpleadoParam, NPermisoParam);
}
