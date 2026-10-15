using Capa_Entidades;

namespace Capa_Datos;

public class EstadoEstudioData : RepositorioBase<EstadoEstudio>
{
    public EstadoEstudioData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<EstadoEstudio?> Obtener(int IdEstadoEstudioParam) => ObtenerPorClave(IdEstadoEstudioParam);
}
