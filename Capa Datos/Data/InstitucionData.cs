using Capa_Entidades;

namespace Capa_Datos;

public class InstitucionData : RepositorioBase<Institucion>
{
    public InstitucionData(EvoRRLDbContext contexto) : base(contexto) { }
}
