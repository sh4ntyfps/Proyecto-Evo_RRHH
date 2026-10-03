using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class LimitacionLogica : LogicaBase<Limitacion, LimitacionData>
{
    public LimitacionLogica(LimitacionData datos) : base(datos) { }
}
