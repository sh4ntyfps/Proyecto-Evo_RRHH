using Capa_Entidades;

namespace Capa_Datos;

public class TipoTrabajadorData : RepositorioBase<TipoTrabajador>
{
    public TipoTrabajadorData(EvoRRLDbContext contexto) : base(contexto) { }
}
