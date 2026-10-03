using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class CargoLogica : LogicaBase<Cargo, CargoData>
{
    public CargoLogica(CargoData datos) : base(datos) { }
}
