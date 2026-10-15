using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class AsistenciaData : RepositorioBase<Asistencia>
{
    public AsistenciaData(EvoRRLDbContext contexto) : base(contexto) { }

    public async Task<int> ContarEntreFechas(DateTime desde, DateTime hasta)
        => await Tabla.CountAsync(a => a.Fecha.Date >= desde.Date && a.Fecha.Date <= hasta.Date);

    public Task<Asistencia?> Obtener(DateTime FechaParam, int IdEmpleadoParam) => ObtenerPorClave(FechaParam, IdEmpleadoParam);
}
