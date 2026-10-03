using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class SistemaOpcionLogica : LogicaBase<SistemaOpcion, SistemaOpcionData>
{
    public SistemaOpcionLogica(SistemaOpcionData datos) : base(datos) { }
}
