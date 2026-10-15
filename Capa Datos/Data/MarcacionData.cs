using Capa_Entidades;

namespace Capa_Datos;

public class MarcacionData : RepositorioBase<Marcacion>
{
    public MarcacionData(EvoRRLDbContext contexto) : base(contexto) { }
}
