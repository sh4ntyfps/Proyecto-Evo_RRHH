using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class TipoDocLogica : LogicaBase<TipoDoc, TipoDocData>
{
    public TipoDocLogica(TipoDocData datos) : base(datos) { }
}
