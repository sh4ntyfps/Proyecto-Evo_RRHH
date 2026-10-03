using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class RRHH_SaludLogica : LogicaBase<RRHH_Salud, RRHH_SaludData>
{
    public RRHH_SaludLogica(RRHH_SaludData datos) : base(datos) { }
}
