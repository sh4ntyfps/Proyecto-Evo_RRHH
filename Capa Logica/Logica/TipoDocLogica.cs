using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoDocLogica
{
    private readonly TipoDocData _data;

    public TipoDocLogica(TipoDocData data) => _data = data;

    public async Task<List<TipoDoc>> Listar() => await _data.Listar();

    public async Task<TipoDoc?> Obtener(string IdTipodocParam, string AnioParam) => await _data.Obtener(IdTipodocParam, AnioParam);

    public async Task Crear(TipoDoc registro) => await _data.Crear(registro);

    public async Task Actualizar(TipoDoc registro) => await _data.Actualizar(registro);

    public async Task Eliminar(TipoDoc registro) => await _data.Eliminar(registro);
}
