using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoRecursoLogica : LogicaBase<TipoRecurso>
{
    private readonly TipoRecursoData _data;

    public TipoRecursoLogica(TipoRecursoData data) : base(data) => _data = data;

    public async Task<TipoRecurso?> Obtener(string IdTipoRecursoParam) => await _data.Obtener(IdTipoRecursoParam);
}
