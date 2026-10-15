using Capa_Entidades;

namespace Capa_Datos;

public class TipoComportamientoData : RepositorioBase<TipoComportamiento>
{
    public TipoComportamientoData(EvoRRLDbContext contexto) : base(contexto) { }
}
