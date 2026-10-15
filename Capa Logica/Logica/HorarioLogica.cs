using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class HorarioLogica : LogicaBase<Horario>
{
    private readonly HorarioData _data;

    public HorarioLogica(HorarioData data) : base(data) => _data = data;

    public async Task<Horario?> Obtener(int idHorarioParam) => await _data.Obtener(idHorarioParam);
}
