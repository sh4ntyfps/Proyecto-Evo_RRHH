using Capa_Entidades;

namespace Capa_Datos;

public class RotacionData : RepositorioBase<Rotacion>
{
    public RotacionData(EvoRRLDbContext contexto) : base(contexto) { }
}
