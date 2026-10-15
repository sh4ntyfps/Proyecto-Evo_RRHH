using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class HorarioTemporalLogica : LogicaBase<HorarioTemporal>
{
    private readonly HorarioTemporalData _data;

    public HorarioTemporalLogica(HorarioTemporalData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<HorarioTemporal?> Obtener(int idEmpleadoParam, int NParam) => await _data.Obtener(idEmpleadoParam, NParam);

    // Numero de horario temporal correlativo por empleado.
    public Task CrearConNuevoId(HorarioTemporal registro) =>
        CrearNumerado(registro, x => x.N, id => registro.N = id, x => x.idEmpleado == registro.idEmpleado);

    public Task<bool> Registrar(HorarioTemporal registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
