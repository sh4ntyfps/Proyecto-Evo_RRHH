using Capa_Datos;

namespace Capa_Logica;

/// <summary>
/// Logica de negocio generica. Antes cada una de las 76 clases XxxLogica repetia los
/// mismos cinco metodos de reenvio hacia su XxxData (DRY); ahora ese reenvio vive aqui
/// una sola vez y cada clase concreta solo declara su entidad y su repositorio.
/// Las reglas de negocio propias de una entidad se sobrescriben en su clase concreta,
/// que sigue siendo el unico punto de cambio para esa entidad (SRP).
/// </summary>
public abstract class LogicaBase<TEntidad, TRepositorio>
    where TEntidad : class
    where TRepositorio : RepositorioBase<TEntidad>
{
    protected readonly TRepositorio Datos;

    protected LogicaBase(TRepositorio datos) => Datos = datos;

    public virtual async Task<List<TEntidad>> Listar() => await Datos.Listar();

    public virtual async Task<TEntidad?> Obtener(params object?[] clave) => await Datos.Obtener(clave);

    public virtual async Task Crear(TEntidad registro) => await Datos.Crear(registro);

    public virtual async Task Actualizar(TEntidad registro) => await Datos.Actualizar(registro);

    public virtual async Task Eliminar(TEntidad registro) => await Datos.Eliminar(registro);
}
