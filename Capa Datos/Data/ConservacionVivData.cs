using Capa_Entidades;

namespace Capa_Datos;

public class ConservacionVivData : RepositorioBase<ConservacionViv>
{
    public ConservacionVivData(EvoRRLDbContext contexto) : base(contexto) { }
}
