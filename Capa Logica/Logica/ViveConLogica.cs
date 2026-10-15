using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ViveConLogica : LogicaBase<ViveCon>
{
    private readonly IRepositorio<ViveCon> _data;

    public ViveConLogica(IRepositorio<ViveCon> data) : base(data) => _data = data;

    public async Task<ViveCon?> Obtener(int idViveCon) => await _data.Obtener(idViveCon);
}
