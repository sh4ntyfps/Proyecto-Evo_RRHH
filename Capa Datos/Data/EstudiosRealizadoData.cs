using Capa_Entidades;

namespace Capa_Datos;

public class EstudiosRealizadoData : RepositorioBase<EstudiosRealizado>
{
    public EstudiosRealizadoData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<EstudiosRealizado?> Obtener(int IdTipoEstudiosParam, int IdEmpleadoParam, int CorrelativoParam) => ObtenerPorClave(IdTipoEstudiosParam, IdEmpleadoParam, CorrelativoParam);
}
