using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class Empleado_AreaLogica : LogicaBase<Empleado_Area, Empleado_AreaData>
{
    public Empleado_AreaLogica(Empleado_AreaData datos) : base(datos) { }
}
