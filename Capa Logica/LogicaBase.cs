using Capa_Entidades.Contratos;

namespace Capa_Logica;

// Operaciones comunes de la capa de logica; cada clase *Logica agrega solo sus reglas propias (DRY).
// Solo conoce contratos: la Capa Datos puede cambiar de base de datos sin tocar esta capa.
public abstract class LogicaBase<T> where T : class
{
    private readonly IRepositorio<T> _repositorio;
    private readonly IUnidadDeTrabajo? _unidad;

    // La unidad de trabajo solo la pasan las clases que registran con Registrar o EnTransaccion.
    protected LogicaBase(IRepositorio<T> repositorio, IUnidadDeTrabajo? unidad = null)
    {
        _repositorio = repositorio;
        _unidad = unidad;
    }

    public Task<List<T>> Listar() => _repositorio.Listar();

    // Si la entidad tiene clave generada, la Capa Datos la asigna y la deja en el registro.
    public Task Crear(T registro) => _repositorio.Crear(registro);
    public Task Actualizar(T registro) => _repositorio.Actualizar(registro);
    public Task Eliminar(T registro) => _repositorio.Eliminar(registro);

    // Alta con reintentos si otro usuario guarda al mismo tiempo: false si no se pudo guardar.
    public Task<bool> Registrar(T registro) => EnTransaccion(() => _repositorio.Crear(registro));

    // Varias operaciones en una sola transaccion: se guardan todas o ninguna.
    protected Task<bool> EnTransaccion(Func<Task> trabajo)
        => (_unidad ?? throw new InvalidOperationException($"{GetType().Name} no recibio IUnidadDeTrabajo."))
            .EjecutarAsync(trabajo);
}
