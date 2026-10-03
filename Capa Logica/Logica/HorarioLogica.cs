using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class HorarioLogica : LogicaBase<Horario, HorarioData>
{
    public HorarioLogica(HorarioData datos) : base(datos) { }
}
