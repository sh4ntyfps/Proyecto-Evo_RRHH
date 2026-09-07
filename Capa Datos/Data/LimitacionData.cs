using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class LimitacionData
{
    private readonly EvoRRLDbContext _contexto;

    public LimitacionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Limitacion>> Listar() => await _contexto.Set<Limitacion>().ToListAsync();

    public async Task<Limitacion?> Obtener(int IdLimitacionParam)
    {
        return await _contexto.Set<Limitacion>().FindAsync(IdLimitacionParam);
    }

    public async Task Crear(Limitacion registro)
    {
        _contexto.Set<Limitacion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Limitacion registro)
    {
        _contexto.Set<Limitacion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Limitacion registro)
    {
        _contexto.Set<Limitacion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
