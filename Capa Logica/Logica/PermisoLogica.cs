using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class PermisoLogica : LogicaBase<Permiso, PermisoData>
{
    public PermisoLogica(PermisoData datos) : base(datos) { }
}
