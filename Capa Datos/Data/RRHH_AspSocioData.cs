using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_AspSocioData : RepositorioBase<RRHH_AspSocio>
{
    public RRHH_AspSocioData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<RRHH_AspSocio?> Obtener(int idAspSocioParam) => ObtenerPorClave(idAspSocioParam);
}
