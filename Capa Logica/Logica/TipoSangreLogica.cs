using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class TipoSangreLogica : LogicaBase<TipoSangre, TipoSangreData>
{
    public TipoSangreLogica(TipoSangreData datos) : base(datos) { }
}
