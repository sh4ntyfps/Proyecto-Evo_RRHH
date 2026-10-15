using Capa_Entidades;

namespace Capa_Datos;

public class TipoResolucionData : RepositorioBase<TipoResolucion>
{
    public TipoResolucionData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<TipoResolucion?> Obtener(int idTipoResolucionParam) => ObtenerPorClave(idTipoResolucionParam);
}
