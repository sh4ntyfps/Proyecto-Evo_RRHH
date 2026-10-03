using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class FamiliaLogica : LogicaBase<Familia, FamiliaData>
{
    public FamiliaLogica(FamiliaData datos) : base(datos) { }
}
