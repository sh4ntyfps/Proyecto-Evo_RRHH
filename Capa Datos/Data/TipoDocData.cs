using Capa_Entidades;

namespace Capa_Datos;

public class TipoDocData : RepositorioBase<TipoDoc>
{
    public TipoDocData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<TipoDoc?> Obtener(string IdTipodocParam, string AnioParam) => ObtenerPorClave(IdTipodocParam, AnioParam);
}
