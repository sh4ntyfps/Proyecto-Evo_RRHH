using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoRecursoLogica : LogicaBase<TipoRecurso>
{
    private readonly IRepositorio<TipoRecurso> _data;

    public TipoRecursoLogica(IRepositorio<TipoRecurso> data) : base(data) => _data = data;

    public async Task<TipoRecurso?> Obtener(string idTipoRecurso) => await _data.Obtener(idTipoRecurso);
}
