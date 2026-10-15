using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ComportamientoLogica : LogicaBase<Comportamiento>
{
    private readonly IRepositorio<Comportamiento> _data;

    public ComportamientoLogica(IRepositorio<Comportamiento> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Comportamiento?> Obtener(int idComportamiento) => await _data.Obtener(idComportamiento);
}
