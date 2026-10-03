using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class InstitucionLogica : LogicaBase<Institucion, InstitucionData>
{
    public InstitucionLogica(InstitucionData datos) : base(datos) { }
}
