using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class TipoInstitucionLogica : LogicaBase<TipoInstitucion, TipoInstitucionData>
{
    public TipoInstitucionLogica(TipoInstitucionData datos) : base(datos) { }
}
