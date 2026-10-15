using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TipoEstudioLogica : LogicaBase<TipoEstudio>
{
    private readonly IRepositorio<TipoEstudio> _data;

    public TipoEstudioLogica(IRepositorio<TipoEstudio> data) : base(data) => _data = data;

    public async Task<TipoEstudio?> Obtener(int idTipoEstudios) => await _data.Obtener(idTipoEstudios);
}
