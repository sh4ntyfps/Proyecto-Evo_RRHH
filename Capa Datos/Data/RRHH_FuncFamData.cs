using Microsoft.EntityFrameworkCore;
using Capa_Entidades;

namespace Capa_Datos;

public class RRHH_FuncFamData
{
    private readonly EvoRRLDbContext _contexto;

    public RRHH_FuncFamData(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<List<RRHH_FuncFam>> Listar() => await _contexto.Set<RRHH_FuncFam>().ToListAsync();

    public async Task<RRHH_FuncFam?> Obtener(int idFuncFamiliarParam)
    {
        return await _contexto.Set<RRHH_FuncFam>().FindAsync(idFuncFamiliarParam);
    }

    public async Task Crear(RRHH_FuncFam registro)
    {
        _contexto.Set<RRHH_FuncFam>().Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(RRHH_FuncFam registro)
    {
        _contexto.Set<RRHH_FuncFam>().Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(RRHH_FuncFam registro)
    {
        _contexto.Set<RRHH_FuncFam>().Remove(registro);
        await _contexto.SaveChangesAsync();
    }
}
