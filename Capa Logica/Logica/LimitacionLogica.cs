using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class LimitacionLogica : LogicaBase<Limitacion>
{
    private readonly IRepositorio<Limitacion> _data;

    public LimitacionLogica(IRepositorio<Limitacion> data) : base(data) => _data = data;

    public async Task<Limitacion?> Obtener(int idLimitacion) => await _data.Obtener(idLimitacion);
}
