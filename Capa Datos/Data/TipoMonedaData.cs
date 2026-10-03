using Capa_Entidades;

namespace Capa_Datos;

public class TipoMonedaData : RepositorioBase<TipoMoneda>
{
    public TipoMonedaData(EvoRRLDbContext contexto) : base(contexto) { }
}
