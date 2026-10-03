using Capa_Entidades;

namespace Capa_Datos;

public class ResolucionData : RepositorioBase<Resolucion>
{
    public ResolucionData(EvoRRLDbContext contexto) : base(contexto) { }
}
