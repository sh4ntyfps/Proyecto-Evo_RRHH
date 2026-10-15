using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoSangreLogica : LogicaBase<TipoSangre>
{
    private readonly TipoSangreData _data;

    public TipoSangreLogica(TipoSangreData data) : base(data) => _data = data;

    public async Task<TipoSangre?> Obtener(int IdTipoSangreParam) => await _data.Obtener(IdTipoSangreParam);
}
