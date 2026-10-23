using Capa_Entidades.Contratos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Capa_Datos;

// CRUD comun a todas las tablas: cada clase *Data hereda de aqui y solo declara lo propio (DRY).
// Donde la tabla de RRHH_Normalizado no coincide con la entidad, la clase *Data traduce
// sobrescribiendo Completar (al leer) y AntesDeGuardar / DespuesDeGuardar (al escribir).
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

    public virtual async Task<List<T>> Listar()
    {
        var registros = await Tabla.ToListAsync();
        await Completar(registros);
        return registros;
    }

    public virtual async Task<T?> Obtener(params object[] claves)
    {
        var registro = await Tabla.FindAsync(claves);
        if (registro is not null)
            await Completar(new List<T> { registro });
        return registro;
    }

    public virtual async Task Crear(T registro)
    {
        var regla = Numeracion.Para<T>();
        // El numero y el INSERT van en la misma transaccion para que dos usuarios no obtengan el mismo.
        // La traduccion a otras tablas tambien, para que se guarde todo o nada.
        var guardo = await _unidad.EjecutarAsync(async () =>
        {
            if (regla is not null)
                await regla.AsignarAsync(Tabla, registro);
            var entrada = Tabla.Add(registro);
            AntesDeGuardar(entrada, esNuevo: true);
            await _contexto.SaveChangesAsync();
            await DespuesDeGuardar(registro, esNuevo: true);
        });
        if (!guardo)
            throw new InvalidOperationException("No se pudo generar el número del registro: intente nuevamente.");
    }

    public virtual async Task Actualizar(T registro)
    {
        var guardo = await _unidad.EjecutarAsync(async () =>
        {
            var entrada = Tabla.Update(registro);
            // Las columnas sin propiedad en la entidad (shadow) no se pisan con NULL;
            // solo se escriben si AntesDeGuardar las asigna.
            foreach (var propiedad in entrada.Properties.Where(p => p.Metadata.IsShadowProperty() && !p.Metadata.IsPrimaryKey()))
                propiedad.IsModified = false;
            AntesDeGuardar(entrada, esNuevo: false);
            await _contexto.SaveChangesAsync();
            await DespuesDeGuardar(registro, esNuevo: false);
        });
        if (!guardo)
            throw new InvalidOperationException("No se pudo guardar el registro: intente nuevamente.");
    }

    public virtual async Task Eliminar(T registro)
    {
        await AntesDeEliminar(registro);
        Tabla.Remove(registro);
        await _contexto.SaveChangesAsync();
    }

    // Llena las propiedades que en RRHH_Normalizado viven en otra tabla o vista.
    protected virtual Task Completar(List<T> registros) => Task.CompletedTask;

    // Copia a columnas sin propiedad (shadow) lo que la entidad trae en otra forma.
    protected virtual void AntesDeGuardar(EntityEntry<T> entrada, bool esNuevo) { }

    // Guarda en otras tablas lo que la entidad trae y su tabla ya no tiene.
    protected virtual Task DespuesDeGuardar(T registro, bool esNuevo) => Task.CompletedTask;

    // Borra las filas dependientes que la BD no elimina en cascada.
    protected virtual Task AntesDeEliminar(T registro) => Task.CompletedTask;

    protected static void Asignar(EntityEntry entrada, string columna, object? valor)
    {
        var propiedad = entrada.Property(columna);
        propiedad.CurrentValue = valor;
        if (entrada.State != EntityState.Added)
            propiedad.IsModified = true;
    }
}
