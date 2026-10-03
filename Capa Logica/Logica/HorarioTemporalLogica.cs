using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class HorarioTemporalLogica : LogicaBase<HorarioTemporal, HorarioTemporalData>
{
    public HorarioTemporalLogica(HorarioTemporalData datos) : base(datos) { }
}
