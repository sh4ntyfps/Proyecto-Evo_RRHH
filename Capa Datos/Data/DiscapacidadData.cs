using Capa_Entidades;

namespace Capa_Datos;

public class DiscapacidadData : RepositorioBase<Discapacidad>
{
    public DiscapacidadData(EvoRRLDbContext contexto) : base(contexto) { }
}
