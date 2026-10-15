using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_AseguradoLogica : LogicaBase<RRHH_Asegurado>
{
    private readonly RRHH_AseguradoData _data;

    public RRHH_AseguradoLogica(RRHH_AseguradoData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<RRHH_Asegurado?> Obtener(int idEmpleadoParam, int NroBeneficiarioParam) => await _data.Obtener(idEmpleadoParam, NroBeneficiarioParam);

    // Numero de beneficiario correlativo por empleado.
    public Task CrearConNuevoId(RRHH_Asegurado registro) =>
        CrearNumerado(registro, x => x.NroBeneficiario, id => registro.NroBeneficiario = id, x => x.idEmpleado == registro.idEmpleado);

    public Task<bool> Registrar(RRHH_Asegurado registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
