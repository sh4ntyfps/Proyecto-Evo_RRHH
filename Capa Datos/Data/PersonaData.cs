using Capa_Entidades;

namespace Capa_Datos;

public class PersonaData : RepositorioBase<Persona>, IPersonaData
{
    public PersonaData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Persona?> Obtener(int IdPersonaParam) => ObtenerPorClave(IdPersonaParam);
}
