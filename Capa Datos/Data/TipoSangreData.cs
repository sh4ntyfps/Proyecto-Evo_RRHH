using Capa_Entidades;

namespace Capa_Datos;

public class TipoSangreData : RepositorioBase<TipoSangre>
{
    public TipoSangreData(EvoRRLDbContext contexto) : base(contexto) { }
}
