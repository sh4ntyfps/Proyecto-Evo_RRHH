using Capa_Entidades;

namespace Capa_Datos;

public class RegAsisDiarioData : RepositorioBase<RegAsisDiario>
{
    public RegAsisDiarioData(EvoRRLDbContext contexto) : base(contexto) { }
}
