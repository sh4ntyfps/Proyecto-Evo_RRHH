using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_FuncFamLogica : LogicaBase<RRHH_FuncFam>
{
    private readonly RRHH_FuncFamData _data;

    public RRHH_FuncFamLogica(RRHH_FuncFamData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<RRHH_FuncFam?> Obtener(int idFuncFamiliarParam) => await _data.Obtener(idFuncFamiliarParam);

    public Task CrearConNuevoId(RRHH_FuncFam registro) =>
        CrearNumerado(registro, x => x.idFuncFamiliar, id => registro.idFuncFamiliar = id);

    public Task<bool> Registrar(RRHH_FuncFam registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
