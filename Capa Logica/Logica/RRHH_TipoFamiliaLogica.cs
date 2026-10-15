using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_TipoFamiliaLogica : LogicaBase<RRHH_TipoFamilia>
{
    private readonly IRepositorio<RRHH_TipoFamilia> _data;

    public RRHH_TipoFamiliaLogica(IRepositorio<RRHH_TipoFamilia> data) : base(data) => _data = data;

    public async Task<RRHH_TipoFamilia?> Obtener(int idTipoFam) => await _data.Obtener(idTipoFam);
}
