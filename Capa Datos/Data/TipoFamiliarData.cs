using Capa_Entidades;

namespace Capa_Datos;

public class TipoFamiliarData : RepositorioBase<TipoFamiliar>
{
    public TipoFamiliarData(EvoRRLDbContext contexto) : base(contexto) { }
}
