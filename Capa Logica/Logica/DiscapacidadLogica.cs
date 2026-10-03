using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class DiscapacidadLogica : LogicaBase<Discapacidad, DiscapacidadData>
{
    public DiscapacidadLogica(DiscapacidadData datos) : base(datos) { }
}
