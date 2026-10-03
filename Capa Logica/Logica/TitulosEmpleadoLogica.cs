using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class TitulosEmpleadoLogica : LogicaBase<TitulosEmpleado, TitulosEmpleadoData>
{
    public TitulosEmpleadoLogica(TitulosEmpleadoData datos) : base(datos) { }
}
