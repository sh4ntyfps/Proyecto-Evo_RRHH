using System.Linq.Expressions;
using Capa_Datos;

namespace Capa_Logica;

// Operaciones comunes de la capa de logica; cada clase *Logica agrega solo sus reglas propias (DRY).
public abstract class LogicaBase<T> where T : class
{
    private readonly RepositorioBase<T> _repositorio;
    private readonly SecuenciaService? _secuencia;

    // La secuencia solo la pasan las clases que numeran sus registros.
    protected LogicaBase(RepositorioBase<T> repositorio, SecuenciaService? secuencia = null)
    {
        _repositorio = repositorio;
        _secuencia = secuencia;
    }

    public Task<List<T>> Listar() => _repositorio.Listar();
    public Task Crear(T registro) => _repositorio.Crear(registro);
    public Task Actualizar(T registro) => _repositorio.Actualizar(registro);
    public Task Eliminar(T registro) => _repositorio.Eliminar(registro);

    // Numeracion "maximo + 1" del sistema legado: asigna el siguiente numero y guarda.
    // "grupo" limita el maximo a un conjunto (ej. permisos del mismo empleado).
    protected async Task CrearNumerado(T registro, Expression<Func<T, int>> campo, Action<int> asignar,
        Expression<Func<T, bool>>? grupo = null)
    {
        asignar(await _repositorio.SiguienteId(campo, grupo));
        await _repositorio.Crear(registro);
    }

    // Transaccion serializable con reintentos: evita que dos usuarios obtengan el mismo numero.
    protected Task<bool> EnTransaccion(Func<Task> trabajo)
        => (_secuencia ?? throw new InvalidOperationException($"{GetType().Name} no recibio SecuenciaService."))
            .EjecutarTransaccionalAsync(trabajo);
}
