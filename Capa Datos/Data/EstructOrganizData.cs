using Capa_Entidades;

namespace Capa_Datos;

public class EstructOrganizData : RepositorioBase<EstructOrganiz>
{
    public EstructOrganizData(EvoRRLDbContext contexto) : base(contexto) { }
}
