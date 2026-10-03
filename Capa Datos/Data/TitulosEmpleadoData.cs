using Capa_Entidades;

namespace Capa_Datos;

public class TitulosEmpleadoData : RepositorioBase<TitulosEmpleado>
{
    public TitulosEmpleadoData(EvoRRLDbContext contexto) : base(contexto) { }
}
