using Capa_Entidades;

namespace Capa_Datos;

public class Motivo_PermData : RepositorioBase<Motivo_Perm>
{
    public Motivo_PermData(EvoRRLDbContext contexto) : base(contexto) { }
}
