using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoComportamientoLogica : LogicaBase<TipoComportamiento>
{
    private readonly IRepositorio<TipoComportamiento> _data;

    public TipoComportamientoLogica(IRepositorio<TipoComportamiento> data) : base(data) => _data = data;

    public async Task<TipoComportamiento?> Obtener(int idTipoComportamiento) => await _data.Obtener(idTipoComportamiento);
}
