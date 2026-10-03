using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class RegimenPensionLogica : LogicaBase<RegimenPension, RegimenPensionData>
{
    public RegimenPensionLogica(RegimenPensionData datos) : base(datos) { }
}
