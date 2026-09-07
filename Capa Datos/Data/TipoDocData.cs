using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoDocData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoDocData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoDoc>> Listar() => await _contexto.Set<TipoDoc>().ToListAsync();

    public async Task<TipoDoc?> Obtener(string IdTipodocParam, string AnioParam)
    {
        return await _contexto.Set<TipoDoc>().FindAsync(IdTipodocParam, AnioParam);
    }

    public async Task Crear(TipoDoc registro)
    {
        _contexto.Set<TipoDoc>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoDoc registro)
    {
        _contexto.Set<TipoDoc>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoDoc registro)
    {
        _contexto.Set<TipoDoc>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
