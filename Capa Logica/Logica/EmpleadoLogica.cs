using Capa_Entidades;
using Capa_Entidades.Constantes;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EmpleadoLogica : LogicaBase<Empleado>
{
    private readonly IRepositorio<Empleado> _data;
    private readonly PersonaLogica _personaLogica;

    public EmpleadoLogica(IRepositorio<Empleado> data, PersonaLogica personaLogica, IUnidadDeTrabajo unidad) : base(data, unidad)
    {
        _data = data;
        _personaLogica = personaLogica;
    }

    public async Task<Empleado?> Obtener(int idEmpleado) => await _data.Obtener(idEmpleado);

    // Alta de un empleado nuevo: su persona y el empleado se guardan en la misma transaccion.
    public Task<bool> RegistrarConPersona(Persona persona, Empleado empleado) => EnTransaccion(async () =>
    {
        persona.FechaRegistro = DateTime.Now;
        await _personaLogica.Crear(persona);

        empleado.IdPersona = persona.IdPersona;
        if (string.IsNullOrWhiteSpace(empleado.Estado))
            empleado.Estado = EstadosEmpleado.Activo;
        await Crear(empleado);
    });
}
