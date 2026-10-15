using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_RelacHermanoLogica : LogicaBase<RRHH_RelacHermano>
{
    private readonly IRepositorio<RRHH_RelacHermano> _data;

    public RRHH_RelacHermanoLogica(IRepositorio<RRHH_RelacHermano> data) : base(data) => _data = data;

    public async Task<RRHH_RelacHermano?> Obtener(int idRelacHermano) => await _data.Obtener(idRelacHermano);
}
