using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoInstitucionLogica : LogicaBase<TipoInstitucion>
{
    private readonly TipoInstitucionData _data;

    public TipoInstitucionLogica(TipoInstitucionData data) : base(data) => _data = data;

    public async Task<TipoInstitucion?> Obtener(int IdTipoInstitucionParam) => await _data.Obtener(IdTipoInstitucionParam);
}
