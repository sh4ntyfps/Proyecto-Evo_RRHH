using Capa_Entidades;

namespace Capa_Datos;

public class EstructOrganizData : RepositorioBase<EstructOrganiz>
{
    public EstructOrganizData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<EstructOrganiz?> Obtener(int YearParam, int idAreaOrganizParam) => ObtenerPorClave(YearParam, idAreaOrganizParam);
}
