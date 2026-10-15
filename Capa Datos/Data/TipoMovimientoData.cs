using Capa_Entidades;

namespace Capa_Datos;

public class TipoMovimientoData : RepositorioBase<TipoMovimiento>
{
    public TipoMovimientoData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<TipoMovimiento?> Obtener(string IdTipoMovimientoParam, string IdTipodocParam) => ObtenerPorClave(IdTipoMovimientoParam, IdTipodocParam);
}
