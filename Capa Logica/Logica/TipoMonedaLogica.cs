using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoMonedaLogica
{
    private readonly TipoMonedaData _data;

    public TipoMonedaLogica(TipoMonedaData data) => _data = data;

    public async Task<List<TipoMoneda>> Listar() => await _data.Listar();

    public async Task<TipoMoneda?> Obtener(int IdTipoMonedaParam) => await _data.Obtener(IdTipoMonedaParam);

    public async Task Crear(TipoMoneda registro) => await _data.Crear(registro);

    public async Task Actualizar(TipoMoneda registro) => await _data.Actualizar(registro);

    public async Task Eliminar(TipoMoneda registro) => await _data.Eliminar(registro);
}
