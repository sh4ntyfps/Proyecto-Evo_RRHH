using Capa_Entidades;

namespace Capa_Datos;

public class TipoRecursoData : RepositorioBase<TipoRecurso>
{
    public TipoRecursoData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<TipoRecurso?> Obtener(string IdTipoRecursoParam) => ObtenerPorClave(IdTipoRecursoParam);
}
