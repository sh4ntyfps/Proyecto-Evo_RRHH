using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class PersonaLogica : LogicaBase<Persona>
{
    private readonly PersonaData _data;

    public PersonaLogica(PersonaData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Persona?> Obtener(int IdPersonaParam) => await _data.Obtener(IdPersonaParam);

    public Task CrearConNuevoId(Persona registro) =>
        CrearNumerado(registro, x => x.IdPersona, id => registro.IdPersona = id);

    public Task<bool> Registrar(Persona registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
