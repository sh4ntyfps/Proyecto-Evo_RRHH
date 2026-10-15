using Capa_Entidades;

namespace Capa_Datos;

public class EstadoCivilData : RepositorioBase<EstadoCivil>
{
    public EstadoCivilData(EvoRRLDbContext contexto) : base(contexto) { }
}
