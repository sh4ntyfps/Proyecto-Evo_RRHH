using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_SaludLogica : LogicaBase<RRHH_Salud>
{
    private readonly RRHH_SaludData _data;

    public RRHH_SaludLogica(RRHH_SaludData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<RRHH_Salud?> Obtener(int idSaludParam) => await _data.Obtener(idSaludParam);

    public Task CrearConNuevoId(RRHH_Salud registro) =>
        CrearNumerado(registro, x => x.idSalud, id => registro.idSalud = id);

    public Task<bool> Registrar(RRHH_Salud registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
