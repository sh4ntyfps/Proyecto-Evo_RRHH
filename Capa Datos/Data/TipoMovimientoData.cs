using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoMovimientoData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoMovimientoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoMovimiento>> Listar() => await _contexto.Set<TipoMovimiento>().ToListAsync();

    public async Task<TipoMovimiento?> Obtener(string IdTipoMovimientoParam, string IdTipodocParam)
    {
        return await _contexto.Set<TipoMovimiento>().FindAsync(IdTipoMovimientoParam, IdTipodocParam);
    }

    public async Task Crear(TipoMovimiento registro)
    {
        _contexto.Set<TipoMovimiento>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoMovimiento registro)
    {
        _contexto.Set<TipoMovimiento>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoMovimiento registro)
    {
        _contexto.Set<TipoMovimiento>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
