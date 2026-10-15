using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoDocIDLogica : LogicaBase<TipoDocID>
{
    private readonly IRepositorio<TipoDocID> _data;

    public TipoDocIDLogica(IRepositorio<TipoDocID> data) : base(data) => _data = data;

    public async Task<TipoDocID?> Obtener(int tipoDocID) => await _data.Obtener(tipoDocID);
}
