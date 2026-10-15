using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_RelacParejaData : RepositorioBase<RRHH_RelacPareja>
{
    public RRHH_RelacParejaData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<RRHH_RelacPareja?> Obtener(int idRelacParejaParam) => ObtenerPorClave(idRelacParejaParam);
}
