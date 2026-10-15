using Capa_Entidades;

namespace Capa_Datos;

public class PermisoData : RepositorioBase<Permiso>
{
    public PermisoData(EvoRRLDbContext contexto) : base(contexto) { }
}
