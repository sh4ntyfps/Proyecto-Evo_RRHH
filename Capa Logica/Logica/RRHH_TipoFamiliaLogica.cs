using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_TipoFamiliaLogica : LogicaBase<RRHH_TipoFamilia>
{
    private readonly RRHH_TipoFamiliaData _data;

    public RRHH_TipoFamiliaLogica(RRHH_TipoFamiliaData data) : base(data) => _data = data;

    public async Task<RRHH_TipoFamilia?> Obtener(int idTipoFamParam) => await _data.Obtener(idTipoFamParam);
}
