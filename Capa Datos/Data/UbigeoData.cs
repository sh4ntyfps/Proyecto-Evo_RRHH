using Capa_Entidades;

namespace Capa_Datos;

public class UbigeoData : RepositorioBase<Ubigeo>
{
    public UbigeoData(EvoRRLDbContext contexto) : base(contexto) { }
}
