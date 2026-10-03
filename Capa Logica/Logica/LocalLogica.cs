using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class LocalLogica : LogicaBase<Local, LocalData>
{
    public LocalLogica(LocalData datos) : base(datos) { }
}
