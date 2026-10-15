using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoTrabajadorLogica : LogicaBase<TipoTrabajador>
{
    private readonly IRepositorio<TipoTrabajador> _data;

    public TipoTrabajadorLogica(IRepositorio<TipoTrabajador> data) : base(data) => _data = data;

    public async Task<TipoTrabajador?> Obtener(int idTipoTrabajador) => await _data.Obtener(idTipoTrabajador);
}
