using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class InconvenienteLogica : LogicaBase<Inconveniente>
{
    private readonly IRepositorio<Inconveniente> _data;

    public InconvenienteLogica(IRepositorio<Inconveniente> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Inconveniente?> Obtener(int idObjetivo, int id_Actividad, int idInconveniente) => await _data.Obtener(idObjetivo, id_Actividad, idInconveniente);
}
