using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TitulosEmpleadoLogica : LogicaBase<TitulosEmpleado>
{
    private readonly TitulosEmpleadoData _data;

    public TitulosEmpleadoLogica(TitulosEmpleadoData data) : base(data) => _data = data;

    public async Task<TitulosEmpleado?> Obtener(string NumColegiaturaParam) => await _data.Obtener(NumColegiaturaParam);
}
