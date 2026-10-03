using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

/// <summary>
/// Repositorio generico de acceso a datos. Concentra el CRUD que antes estaba copiado
/// textualmente en las 76 clases XxxData (DRY): un unico lugar donde cambia la forma de
/// listar, buscar, crear, actualizar o eliminar un registro.
/// Su unica responsabilidad es la persistencia de una entidad (SRP); las consultas
/// propias de una entidad se agregan en su repositorio concreto.
/// </summary>
public abstract class RepositorioBase<TEntidad> where TEntidad : class
{
    protected readonly EvoRRLDbContext Contexto;

    protected RepositorioBase(EvoRRLDbContext contexto) => Contexto = contexto;

    /// <summary>Conjunto de la entidad administrada por este repositorio.</summary>
    protected DbSet<TEntidad> Conjunto => Contexto.Set<TEntidad>();

    public virtual async Task<List<TEntidad>> Listar() => await Conjunto.ToListAsync();

    /// <summary>
    /// Busca un registro por su clave primaria. Acepta claves simples (<c>Obtener(id)</c>)
    /// y compuestas (<c>Obtener(fecha, idEmpleado)</c>), en el orden declarado en el modelo.
    /// </summary>
    public virtual async Task<TEntidad?> Obtener(params object?[] clave) => await Conjunto.FindAsync(clave);

    public virtual async Task Crear(TEntidad registro)
    {
        Conjunto.Add(registro);
        await Contexto.SaveChangesAsync();
    }

    public virtual async Task Actualizar(TEntidad registro)
    {
        Conjunto.Update(registro);
        await Contexto.SaveChangesAsync();
    }

    public virtual async Task Eliminar(TEntidad registro)
    {
        Conjunto.Remove(registro);
        await Contexto.SaveChangesAsync();
    }
}
