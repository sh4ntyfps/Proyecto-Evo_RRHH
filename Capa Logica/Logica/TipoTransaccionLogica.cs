using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class TipoTransaccionLogica : LogicaBase<TipoTransaccion, TipoTransaccionData>
{
    public TipoTransaccionLogica(TipoTransaccionData datos) : base(datos) { }
}
