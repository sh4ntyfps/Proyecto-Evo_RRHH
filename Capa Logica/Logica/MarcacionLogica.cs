using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class MarcacionLogica : LogicaBase<Marcacion>
{
    private readonly IRepositorio<Marcacion> _data;

    public MarcacionLogica(IRepositorio<Marcacion> data) : base(data) => _data = data;

    public async Task<Marcacion?> Obtener(DateTime fecha, int idEmpleado) => await _data.Obtener(fecha, idEmpleado);
}
