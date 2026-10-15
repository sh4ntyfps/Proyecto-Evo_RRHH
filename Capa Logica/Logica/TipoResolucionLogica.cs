using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoResolucionLogica : LogicaBase<TipoResolucion>
{
    private readonly IRepositorio<TipoResolucion> _data;

    public TipoResolucionLogica(IRepositorio<TipoResolucion> data) : base(data) => _data = data;

    public async Task<TipoResolucion?> Obtener(int idTipoResolucion) => await _data.Obtener(idTipoResolucion);
}
