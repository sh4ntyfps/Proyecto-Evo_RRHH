using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class Rol_AccesoLogica : LogicaBase<Rol_Acceso, Rol_AccesoData>
{
    public Rol_AccesoLogica(Rol_AccesoData datos) : base(datos) { }
}
