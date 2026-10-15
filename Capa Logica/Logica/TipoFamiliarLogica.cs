using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoFamiliarLogica : LogicaBase<TipoFamiliar>
{
    private readonly IRepositorio<TipoFamiliar> _data;

    public TipoFamiliarLogica(IRepositorio<TipoFamiliar> data) : base(data) => _data = data;

    public async Task<TipoFamiliar?> Obtener(int idTipoFam) => await _data.Obtener(idTipoFam);
}
