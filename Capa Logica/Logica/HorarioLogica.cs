using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class HorarioLogica : LogicaBase<Horario>
{
    private readonly IRepositorio<Horario> _data;

    public HorarioLogica(IRepositorio<Horario> data) : base(data) => _data = data;

    public async Task<Horario?> Obtener(int idHorario) => await _data.Obtener(idHorario);
}
