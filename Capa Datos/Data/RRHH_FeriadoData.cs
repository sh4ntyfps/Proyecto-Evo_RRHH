using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_FeriadoData : RepositorioBase<RRHH_Feriado>
{
    public RRHH_FeriadoData(EvoRRLDbContext contexto) : base(contexto) { }
}
