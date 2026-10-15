using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EmpleadoLogica : LogicaBase<Empleado>
{
    private readonly EmpleadoData _data;
    private readonly PersonaLogica _personaLogica;

    public EmpleadoLogica(EmpleadoData data, PersonaLogica personaLogica, SecuenciaService secuencia) : base(data, secuencia)
    {
        _data = data;
        _personaLogica = personaLogica;
    }

    public async Task<Empleado?> Obtener(int IdEmpleadoParam) => await _data.Obtener(IdEmpleadoParam);

    public Task CrearConNuevoId(Empleado registro) =>
        CrearNumerado(registro, x => x.IdEmpleado, id => registro.IdEmpleado = id);

    public Task<bool> Registrar(Empleado registro) => EnTransaccion(() => CrearConNuevoId(registro));

    // Alta de un empleado nuevo: su persona y el empleado se guardan en la misma transaccion.
    public Task<bool> RegistrarConPersona(Persona persona, Empleado empleado) => EnTransaccion(async () =>
    {
        persona.FechaRegistro = DateTime.Now;
        await _personaLogica.CrearConNuevoId(persona);

        empleado.IdPersona = persona.IdPersona;
        if (string.IsNullOrWhiteSpace(empleado.Estado))
            empleado.Estado = "A";
        await CrearConNuevoId(empleado);
    });
}
