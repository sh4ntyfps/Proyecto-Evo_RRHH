using Capa_Entidades;

namespace Capa_Datos;

public class AguaVivData : RepositorioBase<AguaViv>
{
    public AguaVivData(EvoRRLDbContext contexto) : base(contexto) { }
}
