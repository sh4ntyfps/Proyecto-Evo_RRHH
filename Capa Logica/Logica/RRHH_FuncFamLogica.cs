using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_FuncFamLogica : LogicaBase<RRHH_FuncFam>
{
    private readonly IRepositorio<RRHH_FuncFam> _data;

    public RRHH_FuncFamLogica(IRepositorio<RRHH_FuncFam> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<RRHH_FuncFam?> Obtener(int idFuncFamiliar) => await _data.Obtener(idFuncFamiliar);
}
