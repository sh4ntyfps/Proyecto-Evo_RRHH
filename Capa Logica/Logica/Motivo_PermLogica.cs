using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class Motivo_PermLogica : LogicaBase<Motivo_Perm, Motivo_PermData>
{
    public Motivo_PermLogica(Motivo_PermData datos) : base(datos) { }
}
