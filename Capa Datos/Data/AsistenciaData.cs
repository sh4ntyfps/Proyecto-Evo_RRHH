using Microsoft.EntityFrameworkCore;
using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Datos;

public class AsistenciaData : RepositorioBase<Asistencia>, IAsistenciaRepositorio
{
    public AsistenciaData(EvoRRLDbContext contexto) : base(contexto) { }

    public async Task<int> ContarEntreFechas(DateTime desde, DateTime hasta)
        => await Tabla.CountAsync(a => a.Fecha.Date >= desde.Date && a.Fecha.Date <= hasta.Date);
}
