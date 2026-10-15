using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ExpLaboralLogica : LogicaBase<ExpLaboral>
{
    private readonly ExpLaboralData _data;

    public ExpLaboralLogica(ExpLaboralData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<ExpLaboral?> Obtener(int IdEmpleadoParam, int IdExpLabParam) => await _data.Obtener(IdEmpleadoParam, IdExpLabParam);

    // Numero de experiencia correlativo por empleado.
    public Task CrearConNuevoId(ExpLaboral registro) =>
        CrearNumerado(registro, x => x.IdExpLab, id => registro.IdExpLab = id, x => x.IdEmpleado == registro.IdEmpleado);

    public Task<bool> Registrar(ExpLaboral registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
