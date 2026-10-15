using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_RelacHermanoLogica : LogicaBase<RRHH_RelacHermano>
{
    private readonly RRHH_RelacHermanoData _data;

    public RRHH_RelacHermanoLogica(RRHH_RelacHermanoData data) : base(data) => _data = data;

    public async Task<RRHH_RelacHermano?> Obtener(int idRelacHermanoParam) => await _data.Obtener(idRelacHermanoParam);
}
