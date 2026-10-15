using Capa_Entidades;

namespace Capa_Datos;

public class InconvenienteData : RepositorioBase<Inconveniente>
{
    public InconvenienteData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Inconveniente?> Obtener(int IdObjetivoParam, int Id_ActividadParam, int IdInconvenienteParam) => ObtenerPorClave(IdObjetivoParam, Id_ActividadParam, IdInconvenienteParam);
}
