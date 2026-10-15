using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ViviendaLogica : LogicaBase<Vivienda>
{
    private readonly IRepositorio<Vivienda> _data;

    public ViviendaLogica(IRepositorio<Vivienda> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Vivienda?> Obtener(int idVivienda) => await _data.Obtener(idVivienda);
}
