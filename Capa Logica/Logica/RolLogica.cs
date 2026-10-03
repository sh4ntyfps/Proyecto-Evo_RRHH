using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class RolLogica : LogicaBase<Rol, RolData>
{
    public RolLogica(RolData datos) : base(datos) { }
}
