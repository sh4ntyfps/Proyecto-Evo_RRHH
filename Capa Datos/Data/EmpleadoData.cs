using Capa_Entidades;

namespace Capa_Datos;

public class EmpleadoData : RepositorioBase<Empleado>, IEmpleadoData
{
    public EmpleadoData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Empleado?> Obtener(int IdEmpleadoParam) => ObtenerPorClave(IdEmpleadoParam);
}
