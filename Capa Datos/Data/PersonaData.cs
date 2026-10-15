using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Datos;

public class PersonaData : RepositorioBase<Persona>, IPersonaData
{
    public PersonaData(EvoRRLDbContext contexto) : base(contexto) { }
}
