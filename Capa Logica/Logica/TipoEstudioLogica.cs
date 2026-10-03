using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class TipoEstudioLogica : LogicaBase<TipoEstudio, TipoEstudioData>
{
    public TipoEstudioLogica(TipoEstudioData datos) : base(datos) { }
}
