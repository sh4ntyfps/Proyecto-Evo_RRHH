using Capa_Entidades;

namespace Capa_Datos;

public class NacionalidadData : RepositorioBase<Nacionalidad>
{
    public NacionalidadData(EvoRRLDbContext contexto) : base(contexto) { }
}
