using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_RelacParejaLogica : LogicaBase<RRHH_RelacPareja>
{
    private readonly IRepositorio<RRHH_RelacPareja> _data;

    public RRHH_RelacParejaLogica(IRepositorio<RRHH_RelacPareja> data) : base(data) => _data = data;

    public async Task<RRHH_RelacPareja?> Obtener(int idRelacPareja) => await _data.Obtener(idRelacPareja);
}
