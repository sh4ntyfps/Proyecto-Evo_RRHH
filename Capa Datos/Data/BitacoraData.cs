using Microsoft.EntityFrameworkCore;
using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Datos;

public class BitacoraData : IBitacoraRepositorio
{
    private readonly EvoRRLDbContext _contexto;

    public BitacoraData(EvoRRLDbContext contexto) => _contexto = contexto;

    // Registro tolerante: si la tabla aun no existe en la BD, no se rompe la operacion principal.
    public async Task Registrar(Bitacora registro)
    {
        try
        {
            _contexto.Set<Bitacora>().Add(registro);
            await _contexto.SaveChangesAsync();
        }
        catch
        {
            // La bitacora nunca debe impedir el flujo principal del negocio.
        }
    }

    public async Task<List<Bitacora>> Ultimas(int cantidad)
    {
        try
        {
            return await _contexto.Set<Bitacora>()
                .OrderByDescending(b => b.Fecha)
                .Take(cantidad)
                .ToListAsync();
        }
        catch
        {
            return new List<Bitacora>();
        }
    }
}
