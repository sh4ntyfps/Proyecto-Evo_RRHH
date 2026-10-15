using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ResolucionLogica : LogicaBase<Resolucion>
{
    private readonly IRepositorio<Resolucion> _data;

    public ResolucionLogica(IRepositorio<Resolucion> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Resolucion?> Obtener(int idEmpleado, int idResolucion) => await _data.Obtener(idEmpleado, idResolucion);
}
