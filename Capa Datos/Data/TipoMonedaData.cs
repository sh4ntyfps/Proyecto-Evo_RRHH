using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoMonedaData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoMonedaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoMoneda>> Listar() => await _contexto.Set<TipoMoneda>().ToListAsync();

    public async Task<TipoMoneda?> Obtener(int IdTipoMonedaParam)
    {
        return await _contexto.Set<TipoMoneda>().FindAsync(IdTipoMonedaParam);
    }

    public async Task Crear(TipoMoneda registro)
    {
        _contexto.Set<TipoMoneda>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoMoneda registro)
    {
        _contexto.Set<TipoMoneda>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoMoneda registro)
    {
        _contexto.Set<TipoMoneda>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
