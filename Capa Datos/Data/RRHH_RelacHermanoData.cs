using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_RelacHermanoData : RepositorioBase<RRHH_RelacHermano>
{
    public RRHH_RelacHermanoData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<RRHH_RelacHermano?> Obtener(int idRelacHermanoParam) => ObtenerPorClave(idRelacHermanoParam);
}
