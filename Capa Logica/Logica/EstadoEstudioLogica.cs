using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EstadoEstudioLogica : LogicaBase<EstadoEstudio>
{
    private readonly EstadoEstudioData _data;

    public EstadoEstudioLogica(EstadoEstudioData data) : base(data) => _data = data;

    public async Task<EstadoEstudio?> Obtener(int IdEstadoEstudioParam) => await _data.Obtener(IdEstadoEstudioParam);
}
