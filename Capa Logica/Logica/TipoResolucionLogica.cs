using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class TipoResolucionLogica : LogicaBase<TipoResolucion, TipoResolucionData>
{
    public TipoResolucionLogica(TipoResolucionData datos) : base(datos) { }
}
