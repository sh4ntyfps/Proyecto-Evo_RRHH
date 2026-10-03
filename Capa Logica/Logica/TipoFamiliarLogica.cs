using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class TipoFamiliarLogica : LogicaBase<TipoFamiliar, TipoFamiliarData>
{
    public TipoFamiliarLogica(TipoFamiliarData datos) : base(datos) { }
}
