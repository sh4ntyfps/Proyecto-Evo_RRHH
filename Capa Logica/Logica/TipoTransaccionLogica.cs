using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoTransaccionLogica
{
    private readonly TipoTransaccionData _data;

    public TipoTransaccionLogica(TipoTransaccionData data) => _data = data;

    public async Task<List<TipoTransaccion>> Listar() => await _data.Listar();

    public async Task<TipoTransaccion?> Obtener(string IdTipoTransaccionParam, string AnioParam) => await _data.Obtener(IdTipoTransaccionParam, AnioParam);

    public async Task Crear(TipoTransaccion registro) => await _data.Crear(registro);

    public async Task Actualizar(TipoTransaccion registro) => await _data.Actualizar(registro);

    public async Task Eliminar(TipoTransaccion registro) => await _data.Eliminar(registro);
}
