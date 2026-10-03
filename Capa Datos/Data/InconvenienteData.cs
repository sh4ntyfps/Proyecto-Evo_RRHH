using Capa_Entidades;

namespace Capa_Datos;

public class InconvenienteData : RepositorioBase<Inconveniente>
{
    public InconvenienteData(EvoRRLDbContext contexto) : base(contexto) { }
}
