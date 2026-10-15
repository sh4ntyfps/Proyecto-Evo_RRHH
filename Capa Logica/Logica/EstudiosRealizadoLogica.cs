using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EstudiosRealizadoLogica : LogicaBase<EstudiosRealizado>
{
    private readonly IRepositorio<EstudiosRealizado> _data;

    public EstudiosRealizadoLogica(IRepositorio<EstudiosRealizado> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<EstudiosRealizado?> Obtener(int idTipoEstudios, int idEmpleado, int correlativo) => await _data.Obtener(idTipoEstudios, idEmpleado, correlativo);
}
