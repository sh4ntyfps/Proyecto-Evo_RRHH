using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TitulosEmpleadoLogica : LogicaBase<TitulosEmpleado>
{
    private readonly IRepositorio<TitulosEmpleado> _data;

    public TitulosEmpleadoLogica(IRepositorio<TitulosEmpleado> data) : base(data) => _data = data;

    public async Task<TitulosEmpleado?> Obtener(string numColegiatura) => await _data.Obtener(numColegiatura);
}
