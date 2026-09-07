using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class InconvenienteData
{
    private readonly EvoRRLDbContext _contexto;

    public InconvenienteData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<Inconveniente>> Listar() => await _contexto.Set<Inconveniente>().ToListAsync();

    public async Task<Inconveniente?> Obtener(int IdObjetivoParam, int Id_ActividadParam, int IdInconvenienteParam)
    {
        return await _contexto.Set<Inconveniente>().FindAsync(IdObjetivoParam, Id_ActividadParam, IdInconvenienteParam);
    }

    public async Task Crear(Inconveniente registro)
    {
        _contexto.Set<Inconveniente>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(Inconveniente registro)
    {
        _contexto.Set<Inconveniente>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(Inconveniente registro)
    {
        _contexto.Set<Inconveniente>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
