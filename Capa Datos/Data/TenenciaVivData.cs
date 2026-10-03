using Capa_Entidades;

namespace Capa_Datos;

public class TenenciaVivData : RepositorioBase<TenenciaViv>
{
    public TenenciaVivData(EvoRRLDbContext contexto) : base(contexto) { }
}
