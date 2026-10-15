using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_AseguradoData : RepositorioBase<RRHH_Asegurado>
{
    public RRHH_AseguradoData(EvoRRLDbContext contexto) : base(contexto) { }
}
