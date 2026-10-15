using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_RelacPHData : RepositorioBase<RRHH_RelacPH>
{
    public RRHH_RelacPHData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<RRHH_RelacPH?> Obtener(int idRelacPHParam) => ObtenerPorClave(idRelacPHParam);
}
