using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoRecursoLogica
{
    private readonly TipoRecursoData _data;

    public TipoRecursoLogica(TipoRecursoData data) => _data = data;

    public async Task<List<TipoRecurso>> Listar() => await _data.Listar();

    public async Task<TipoRecurso?> Obtener(string IdTipoRecursoParam) => await _data.Obtener(IdTipoRecursoParam);

    public async Task Crear(TipoRecurso registro) => await _data.Crear(registro);

    public async Task Actualizar(TipoRecurso registro) => await _data.Actualizar(registro);

    public async Task Eliminar(TipoRecurso registro) => await _data.Eliminar(registro);
}
