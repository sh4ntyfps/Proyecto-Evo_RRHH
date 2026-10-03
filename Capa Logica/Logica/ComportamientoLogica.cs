using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class ComportamientoLogica : LogicaBase<Comportamiento, ComportamientoData>
{
    public ComportamientoLogica(ComportamientoData datos) : base(datos) { }
}
