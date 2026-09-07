using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class LimitacionLogica
{
    private readonly LimitacionData _data;

    public LimitacionLogica(LimitacionData data) => _data = data;

    public async Task<List<Limitacion>> Listar() => await _data.Listar();

    public async Task<Limitacion?> Obtener(int IdLimitacionParam) => await _data.Obtener(IdLimitacionParam);

    public async Task Crear(Limitacion registro) => await _data.Crear(registro);

    public async Task Actualizar(Limitacion registro) => await _data.Actualizar(registro);

    public async Task Eliminar(Limitacion registro) => await _data.Eliminar(registro);
}
