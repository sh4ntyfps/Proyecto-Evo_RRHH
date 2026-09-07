using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class FamiliaData
{
    private readonly EvoRRLDbContext _contexto;

    public FamiliaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Familia>> Listar() => await _contexto.Set<Familia>().ToListAsync();

    public async Task<Familia?> Obtener(string IdFamiliaParam, string IdClaseParam, string IdGrupoParam)
    {
        return await _contexto.Set<Familia>().FindAsync(IdFamiliaParam, IdClaseParam, IdGrupoParam);
    }

    public async Task Crear(Familia registro)
    {
        _contexto.Set<Familia>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Familia registro)
    {
        _contexto.Set<Familia>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Familia registro)
    {
        _contexto.Set<Familia>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
