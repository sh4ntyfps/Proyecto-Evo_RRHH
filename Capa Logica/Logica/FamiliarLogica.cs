using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class FamiliarLogica : LogicaBase<Familiar, FamiliarData>
{
    public FamiliarLogica(FamiliarData datos) : base(datos) { }
}
