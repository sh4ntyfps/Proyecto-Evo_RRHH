using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_FeriadoLogica : LogicaBase<RRHH_Feriado>
{
    private readonly RRHH_FeriadoData _data;

    public RRHH_FeriadoLogica(RRHH_FeriadoData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<RRHH_Feriado?> Obtener(int idFeriadoParam) => await _data.Obtener(idFeriadoParam);

    public Task CrearConNuevoId(RRHH_Feriado registro) =>
        CrearNumerado(registro, x => x.idFeriado, id => registro.idFeriado = id);

    public Task<bool> Registrar(RRHH_Feriado registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
