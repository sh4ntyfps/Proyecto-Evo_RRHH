using Capa_Entidades;

namespace Capa_Datos;

public class TipoVivData : RepositorioBase<TipoViv>
{
    public TipoVivData(EvoRRLDbContext contexto) : base(contexto) { }
}
