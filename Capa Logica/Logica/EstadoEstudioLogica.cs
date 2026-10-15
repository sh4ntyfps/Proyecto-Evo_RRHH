using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EstadoEstudioLogica : LogicaBase<EstadoEstudio>
{
    private readonly IRepositorio<EstadoEstudio> _data;

    public EstadoEstudioLogica(IRepositorio<EstadoEstudio> data) : base(data) => _data = data;

    public async Task<EstadoEstudio?> Obtener(int idEstadoEstudio) => await _data.Obtener(idEstadoEstudio);
}
