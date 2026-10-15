using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

// CRUD comun a todas las tablas: cada clase *Data hereda de aqui y solo declara lo propio (DRY).
public abstract class RepositorioBase<T> where T : class
{
    protected readonly EvoRRLDbContext _contexto;

    protected RepositorioBase(EvoRRLDbContext contexto) => _contexto = contexto;

    protected DbSet<T> Tabla => _contexto.Set<T>();

    public async Task<List<T>> Listar() => await Tabla.ToListAsync();

    // El filtro se ejecuta en SQL Server, no en memoria.
    public async Task<List<T>> Buscar(Expression<Func<T, bool>> filtro) => await Tabla.Where(filtro).ToListAsync();

    public async Task<T?> Primero(Expression<Func<T, bool>> filtro) => await Tabla.FirstOrDefaultAsync(filtro);

    // Correlativo "maximo + 1" (1 si no hay registros), opcionalmente dentro de un grupo (ej. por empleado).
    public async Task<int> SiguienteId(Expression<Func<T, int>> campo, Expression<Func<T, bool>>? filtro = null)
    {
        var consulta = filtro is null ? Tabla : Tabla.Where(filtro);
        // Se proyecta como int? para que MAX sobre una tabla vacia devuelva NULL en lugar de fallar.
        var campoNullable = Expression.Lambda<Func<T, int?>>(
            Expression.Convert(campo.Body, typeof(int?)), campo.Parameters);
        var maximo = await consulta.MaxAsync(campoNullable);
        return (maximo ?? 0) + 1;
    }

    public async Task Crear(T registro)
    {
        Tabla.Add(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Actualizar(T registro)
    {
        Tabla.Update(registro);
        await _contexto.SaveChangesAsync();
    }

    public async Task Eliminar(T registro)
    {
        Tabla.Remove(registro);
        await _contexto.SaveChangesAsync();
    }

    protected async Task<T?> ObtenerPorClave(params object[] claves) => await Tabla.FindAsync(claves);
}
