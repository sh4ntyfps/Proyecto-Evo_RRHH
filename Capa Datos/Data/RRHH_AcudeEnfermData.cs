using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_AcudeEnfermData : RepositorioBase<RRHH_AcudeEnferm>
{
    public RRHH_AcudeEnfermData(EvoRRLDbContext contexto) : base(contexto) { }
}
