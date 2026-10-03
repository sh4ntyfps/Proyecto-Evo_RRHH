using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class ViveConLogica : LogicaBase<ViveCon, ViveConData>
{
    public ViveConLogica(ViveConData datos) : base(datos) { }
}
