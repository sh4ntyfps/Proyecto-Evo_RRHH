using Capa_Entidades;

namespace Capa_Datos;

public class LocalData : RepositorioBase<Local>
{
    public LocalData(EvoRRLDbContext contexto) : base(contexto) { }
}
