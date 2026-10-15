using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_SaludData : RepositorioBase<RRHH_Salud>
{
    public RRHH_SaludData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<RRHH_Salud?> Obtener(int idSaludParam) => ObtenerPorClave(idSaludParam);
}
