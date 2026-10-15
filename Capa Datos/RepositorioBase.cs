using Capa_Entidades.Contratos;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

// CRUD comun a todas las tablas: cada clase *Data hereda de aqui y solo declara lo propio (DRY).
public abstract class RepositorioBase<T> : IRepositorio<T> where T : class
{
    protected readonly EvoRRLDbContext _contexto;
    private readonly UnidadDeTrabajo _unidad;

    protected RepositorioBase(EvoRRLDbContext contexto)
    {
        _contexto = contexto;
        _unidad = new UnidadDeTrabajo(contexto);
    }

    protected DbSet<T> Tabla => _contexto.Set<T>();

    public async Task<List<T>> Listar() => await Tabla.ToListAsync();

    public async Task<T?> Obtener(params object[] claves) => await Tabla.FindAsync(claves);

    public async Task Crear(T registro)
    {
        var regla = Numeracion.Para<T>();
        if (regla is null)
        {
            Tabla.Add(registro);
            await _contexto.SaveChangesAsync();
            return;
        }

        // El numero y el INSERT van en la misma transaccion para que dos usuarios no obtengan el mismo.
        var guardo = await _unidad.EjecutarAsync(async () =>
        {
            await regla.AsignarAsync(Tabla, registro);
            Tabla.Add(registro);
            await _contexto.SaveChangesAsync();
        });
        if (!guardo)
            throw new InvalidOperationException("No se pudo generar el número del registro: intente nuevamente.");
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
}
