using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class InstitucionData
{
    private readonly EvoRRLDbContext _contexto;

    public InstitucionData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Institucion>> Listar() => await _contexto.Set<Institucion>().ToListAsync();

    public async Task<Institucion?> Obtener(int IdInstitucionParam)
    {
        return await _contexto.Set<Institucion>().FindAsync(IdInstitucionParam);
    }

    public async Task Crear(Institucion registro)
    {
        _contexto.Set<Institucion>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Institucion registro)
    {
        _contexto.Set<Institucion>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Institucion registro)
    {
        _contexto.Set<Institucion>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
