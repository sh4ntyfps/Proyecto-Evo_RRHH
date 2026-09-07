using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class UnidadMedidaData
{
    private readonly EvoRRLDbContext _contexto;

    public UnidadMedidaData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<UnidadMedida>> Listar() => await _contexto.Set<UnidadMedida>().ToListAsync();

    public async Task<UnidadMedida?> Obtener(string IdUnidadMedidaParam)
    {
        return await _contexto.Set<UnidadMedida>().FindAsync(IdUnidadMedidaParam);
    }

    public async Task Crear(UnidadMedida registro)
    {
        _contexto.Set<UnidadMedida>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(UnidadMedida registro)
    {
        _contexto.Set<UnidadMedida>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(UnidadMedida registro)
    {
        _contexto.Set<UnidadMedida>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
