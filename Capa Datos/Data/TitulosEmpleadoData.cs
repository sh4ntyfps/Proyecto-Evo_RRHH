using Capa_Entidades;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

// En RRHH_Normalizado la clave de TitulosEmpleado es IdTitulo (IDENTITY) y no tiene propiedad
// en la entidad; la aplicacion sigue identificando el titulo por su NumColegiatura.
public class TitulosEmpleadoData : RepositorioBase<TitulosEmpleado>
{
    public TitulosEmpleadoData(EvoRRLDbContext contexto) : base(contexto) { }

    public override async Task<TitulosEmpleado?> Obtener(params object[] claves)
    {
        var numColegiatura = claves.FirstOrDefault() as string;
        return await Tabla.Where(t => t.NumColegiatura == numColegiatura)
            .OrderBy(t => EF.Property<int>(t, "IdTitulo")).FirstOrDefaultAsync();
    }
}
