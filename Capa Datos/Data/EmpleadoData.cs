using Capa_Entidades;

namespace Capa_Datos;

public class EmpleadoData : RepositorioBase<Empleado>
{
    public EmpleadoData(EvoRRLDbContext contexto) : base(contexto) { }
}
