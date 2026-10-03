using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class ViviendaLogica : LogicaBase<Vivienda, ViviendaData>
{
    public ViviendaLogica(ViviendaData datos) : base(datos) { }
}
