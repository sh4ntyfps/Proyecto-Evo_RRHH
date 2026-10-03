using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class InconvenienteLogica : LogicaBase<Inconveniente, InconvenienteData>
{
    public InconvenienteLogica(InconvenienteData datos) : base(datos) { }
}
