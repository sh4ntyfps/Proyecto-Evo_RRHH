using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class EstructOrganizLogica : LogicaBase<EstructOrganiz, EstructOrganizData>
{
    public EstructOrganizLogica(EstructOrganizData datos) : base(datos) { }
}
