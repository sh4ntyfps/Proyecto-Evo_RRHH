using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class EstadoCivilLogica : LogicaBase<EstadoCivil, EstadoCivilData>
{
    public EstadoCivilLogica(EstadoCivilData datos) : base(datos) { }
}
