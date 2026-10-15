using Capa_Entidades;

namespace Capa_Datos;

public class TipoInstitucionData : RepositorioBase<TipoInstitucion>
{
    public TipoInstitucionData(EvoRRLDbContext contexto) : base(contexto) { }
}
