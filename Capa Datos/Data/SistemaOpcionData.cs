using Capa_Entidades;

namespace Capa_Datos;

public class SistemaOpcionData : RepositorioBase<SistemaOpcion>
{
    public SistemaOpcionData(EvoRRLDbContext contexto) : base(contexto) { }
}
