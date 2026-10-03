using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class TipoTrabajadorLogica : LogicaBase<TipoTrabajador, TipoTrabajadorData>
{
    public TipoTrabajadorLogica(TipoTrabajadorData datos) : base(datos) { }
}
