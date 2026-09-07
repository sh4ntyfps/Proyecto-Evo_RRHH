using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoMovimientoLogica
{
    private readonly TipoMovimientoData _data;

    public TipoMovimientoLogica(TipoMovimientoData data) => _data = data;

    public async Task<List<TipoMovimiento>> Listar() => await _data.Listar();

    public async Task<TipoMovimiento?> Obtener(string IdTipoMovimientoParam, string IdTipodocParam) => await _data.Obtener(IdTipoMovimientoParam, IdTipodocParam);

    public async Task Crear(TipoMovimiento registro) => await _data.Crear(registro);

    public async Task Actualizar(TipoMovimiento registro) => await _data.Actualizar(registro);

    public async Task Eliminar(TipoMovimiento registro) => await _data.Eliminar(registro);
}
