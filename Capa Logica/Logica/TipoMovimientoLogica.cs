using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class TipoMovimientoLogica : LogicaBase<TipoMovimiento, TipoMovimientoData>
{
    public TipoMovimientoLogica(TipoMovimientoData datos) : base(datos) { }
}
