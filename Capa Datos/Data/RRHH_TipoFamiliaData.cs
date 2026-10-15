using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_TipoFamiliaData : RepositorioBase<RRHH_TipoFamilia>
{
    public RRHH_TipoFamiliaData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<RRHH_TipoFamilia?> Obtener(int idTipoFamParam) => ObtenerPorClave(idTipoFamParam);
}
