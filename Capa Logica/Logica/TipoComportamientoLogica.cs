using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class TipoComportamientoLogica : LogicaBase<TipoComportamiento, TipoComportamientoData>
{
    public TipoComportamientoLogica(TipoComportamientoData datos) : base(datos) { }
}
