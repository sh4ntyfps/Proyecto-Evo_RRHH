using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class PersonaLogica : LogicaBase<Persona, PersonaData>
{
    public PersonaLogica(PersonaData datos) : base(datos) { }
}
