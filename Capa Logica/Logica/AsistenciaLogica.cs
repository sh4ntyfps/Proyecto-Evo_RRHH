using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class AsistenciaLogica : LogicaBase<Asistencia>
{
    private readonly IAsistenciaRepositorio _data;

    public AsistenciaLogica(IAsistenciaRepositorio data) : base(data) => _data = data;

    public async Task<int> ContarEntreFechas(DateTime desde, DateTime hasta) => await _data.ContarEntreFechas(desde, hasta);
    public async Task<Asistencia?> Obtener(DateTime fecha, int idEmpleado) => await _data.Obtener(fecha, idEmpleado);
}
