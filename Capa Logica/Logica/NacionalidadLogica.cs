using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class NacionalidadLogica : LogicaBase<Nacionalidad, NacionalidadData>
{
    public NacionalidadLogica(NacionalidadData datos) : base(datos) { }
}
