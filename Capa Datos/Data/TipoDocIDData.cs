using Capa_Entidades;

namespace Capa_Datos;

public class TipoDocIDData : RepositorioBase<TipoDocID>
{
    public TipoDocIDData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<TipoDocID?> Obtener(int TipoDocIDParam) => ObtenerPorClave(TipoDocIDParam);
}
