using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoVivLogica : LogicaBase<TipoViv>
{
    private readonly IRepositorio<TipoViv> _data;

    public TipoVivLogica(IRepositorio<TipoViv> data) : base(data) => _data = data;

    public async Task<TipoViv?> Obtener(int idTipo) => await _data.Obtener(idTipo);
}
