using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoSangreLogica : LogicaBase<TipoSangre>
{
    private readonly IRepositorio<TipoSangre> _data;

    public TipoSangreLogica(IRepositorio<TipoSangre> data) : base(data) => _data = data;

    public async Task<TipoSangre?> Obtener(int idTipoSangre) => await _data.Obtener(idTipoSangre);
}
