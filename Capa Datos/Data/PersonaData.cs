using Capa_Entidades;

namespace Capa_Datos;

public class PersonaData : RepositorioBase<Persona>
{
    public PersonaData(EvoRRLDbContext contexto) : base(contexto) { }
}
