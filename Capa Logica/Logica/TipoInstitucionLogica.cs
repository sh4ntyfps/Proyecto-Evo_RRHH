using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoInstitucionLogica : LogicaBase<TipoInstitucion>
{
    private readonly IRepositorio<TipoInstitucion> _data;

    public TipoInstitucionLogica(IRepositorio<TipoInstitucion> data) : base(data) => _data = data;

    public async Task<TipoInstitucion?> Obtener(int idTipoInstitucion) => await _data.Obtener(idTipoInstitucion);
}
