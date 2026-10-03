using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_DinamicaFamiliarData : RepositorioBase<RRHH_DinamicaFamiliar>
{
    public RRHH_DinamicaFamiliarData(EvoRRLDbContext contexto) : base(contexto) { }
}
