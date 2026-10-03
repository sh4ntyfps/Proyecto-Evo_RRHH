using Capa_Entidades;

namespace Capa_Datos;

public class EstadoEstudioData : RepositorioBase<EstadoEstudio>
{
    public EstadoEstudioData(EvoRRLDbContext contexto) : base(contexto) { }
}
