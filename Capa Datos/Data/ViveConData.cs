using Capa_Entidades;

namespace Capa_Datos;

public class ViveConData : RepositorioBase<ViveCon>
{
    public ViveConData(EvoRRLDbContext contexto) : base(contexto) { }
}
