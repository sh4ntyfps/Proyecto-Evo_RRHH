using Capa_Entidades;

namespace Capa_Datos;

public class ComportamientoData : RepositorioBase<Comportamiento>
{
    public ComportamientoData(EvoRRLDbContext contexto) : base(contexto) { }
}
