using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoTransaccionData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoTransaccionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoTransaccion>> Listar() => await _contexto.Set<TipoTransaccion>().ToListAsync();

    public async Task<TipoTransaccion?> Obtener(string IdTipoTransaccionParam, string AnioParam)
    {
        return await _contexto.Set<TipoTransaccion>().FindAsync(IdTipoTransaccionParam, AnioParam);
    }

    public async Task Crear(TipoTransaccion registro)
    {
        _contexto.Set<TipoTransaccion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoTransaccion registro)
    {
        _contexto.Set<TipoTransaccion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoTransaccion registro)
    {
        _contexto.Set<TipoTransaccion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
