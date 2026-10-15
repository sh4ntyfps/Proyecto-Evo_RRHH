using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Datos;

public class EmpleadoData : RepositorioBase<Empleado>, IEmpleadoData
{
    public EmpleadoData(EvoRRLDbContext contexto) : base(contexto) { }
}
