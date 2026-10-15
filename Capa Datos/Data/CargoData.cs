using Capa_Entidades;

namespace Capa_Datos;

public class CargoData : RepositorioBase<Cargo>
{
    public CargoData(EvoRRLDbContext contexto) : base(contexto) { }
}
