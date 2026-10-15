using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_RelacParejaLogica : LogicaBase<RRHH_RelacPareja>
{
    private readonly RRHH_RelacParejaData _data;

    public RRHH_RelacParejaLogica(RRHH_RelacParejaData data) : base(data) => _data = data;

    public async Task<RRHH_RelacPareja?> Obtener(int idRelacParejaParam) => await _data.Obtener(idRelacParejaParam);
}
