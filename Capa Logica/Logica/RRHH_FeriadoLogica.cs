using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_FeriadoLogica : LogicaBase<RRHH_Feriado>
{
    private readonly IRepositorio<RRHH_Feriado> _data;

    public RRHH_FeriadoLogica(IRepositorio<RRHH_Feriado> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<RRHH_Feriado?> Obtener(int idFeriado) => await _data.Obtener(idFeriado);
}
