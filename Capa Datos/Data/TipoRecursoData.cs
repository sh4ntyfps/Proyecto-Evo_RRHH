using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class TipoRecursoData
{
    private readonly EvoRRLDbContext _contexto;

    public TipoRecursoData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<TipoRecurso>> Listar() => await _contexto.Set<TipoRecurso>().ToListAsync();

    public async Task<TipoRecurso?> Obtener(string IdTipoRecursoParam)
    {
        return await _contexto.Set<TipoRecurso>().FindAsync(IdTipoRecursoParam);
    }

    public async Task Crear(TipoRecurso registro)
    {
        _contexto.Set<TipoRecurso>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(TipoRecurso registro)
    {
        _contexto.Set<TipoRecurso>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(TipoRecurso registro)
    {
        _contexto.Set<TipoRecurso>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
