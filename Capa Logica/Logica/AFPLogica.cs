using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class AFPLogica : LogicaBase<AFP, AFPData>
{
    public AFPLogica(AFPData datos) : base(datos) { }
}
