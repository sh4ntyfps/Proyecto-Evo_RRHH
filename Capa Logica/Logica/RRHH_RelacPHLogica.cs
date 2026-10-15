using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_RelacPHLogica : LogicaBase<RRHH_RelacPH>
{
    private readonly RRHH_RelacPHData _data;

    public RRHH_RelacPHLogica(RRHH_RelacPHData data) : base(data) => _data = data;

    public async Task<RRHH_RelacPH?> Obtener(int idRelacPHParam) => await _data.Obtener(idRelacPHParam);
}
