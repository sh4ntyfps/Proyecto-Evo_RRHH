using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoFamiliarLogica : LogicaBase<TipoFamiliar>
{
    private readonly TipoFamiliarData _data;

    public TipoFamiliarLogica(TipoFamiliarData data) : base(data) => _data = data;

    public async Task<TipoFamiliar?> Obtener(int IdTipoFamParam) => await _data.Obtener(IdTipoFamParam);
}
