using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class PersonaLogica : LogicaBase<Persona>
{
    private readonly IRepositorio<Persona> _data;

    public PersonaLogica(IRepositorio<Persona> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Persona?> Obtener(int idPersona) => await _data.Obtener(idPersona);
}
