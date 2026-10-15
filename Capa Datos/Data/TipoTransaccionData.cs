using Capa_Entidades;

namespace Capa_Datos;

public class TipoTransaccionData : RepositorioBase<TipoTransaccion>
{
    public TipoTransaccionData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<TipoTransaccion?> Obtener(string IdTipoTransaccionParam, string AnioParam) => ObtenerPorClave(IdTipoTransaccionParam, AnioParam);
}
