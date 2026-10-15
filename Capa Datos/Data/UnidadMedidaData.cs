using Capa_Entidades;

namespace Capa_Datos;

public class UnidadMedidaData : RepositorioBase<UnidadMedida>
{
    public UnidadMedidaData(EvoRRLDbContext contexto) : base(contexto) { }
}
