using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class PeriodoLaboralLogica : LogicaBase<PeriodoLaboral>
{
    private readonly PeriodoLaboralData _data;

    public PeriodoLaboralLogica(PeriodoLaboralData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<PeriodoLaboral?> Obtener(int IdEmpleadoParam, int NroPeriodoParam) => await _data.Obtener(IdEmpleadoParam, NroPeriodoParam);

    // Numero de periodo correlativo por empleado.
    public Task CrearConNuevoId(PeriodoLaboral registro) =>
        CrearNumerado(registro, x => x.NroPeriodo, id => registro.NroPeriodo = id, x => x.IdEmpleado == registro.IdEmpleado);

    public Task<bool> Registrar(PeriodoLaboral registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
