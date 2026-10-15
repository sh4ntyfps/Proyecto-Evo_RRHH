using Capa_Entidades;

namespace Capa_Datos;

public class EstudiosRealizadoData : RepositorioBase<EstudiosRealizado>
{
    public EstudiosRealizadoData(EvoRRLDbContext contexto) : base(contexto) { }
}
