using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

// Lista de empleados con su nombre para combos y encabezados de todos los modulos.
// Depende de contratos, no de clases concretas: el contenedor entrega la implementacion (IoC).
public class EmpleadoServicio
{
    private readonly IEmpleadoData _empleadoData;
    private readonly IPersonaData _personaData;

    public EmpleadoServicio(IEmpleadoData empleadoData, IPersonaData personaData)
    {
        _empleadoData = empleadoData;
        _personaData = personaData;
    }

    public async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
    {
        var empleados = await _empleadoData.Listar();
        var personas = await _personaData.Listar();
        var personasPorId = personas.ToDictionary(p => p.IdPersona);
        return empleados.Select(e =>
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null ? $"(Empleado {e.IdEmpleado})" : persona.NombreCompleto();
            return (e.IdEmpleado, nombre);
        }).OrderBy(x => x.nombre, StringComparer.CurrentCultureIgnoreCase).ToList();
    }

    public async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => (await EmpleadosAsync()).FirstOrDefault(x => x.Id == idEmpleado).Nombre;
}
