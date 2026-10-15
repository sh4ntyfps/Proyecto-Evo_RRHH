using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_RelacPHLogica : LogicaBase<RRHH_RelacPH>
{
    private readonly IRepositorio<RRHH_RelacPH> _data;

    public RRHH_RelacPHLogica(IRepositorio<RRHH_RelacPH> data) : base(data) => _data = data;

    public async Task<RRHH_RelacPH?> Obtener(int idRelacPH) => await _data.Obtener(idRelacPH);
}
