using Capa_Entidades;

namespace Capa_Datos;

public class TipoTransaccionData : RepositorioBase<TipoTransaccion>
{
    public TipoTransaccionData(EvoRRLDbContext contexto) : base(contexto) { }
}
