namespace Capa_Entidades.Contratos;

// Agrupa varias operaciones en una sola transaccion: se guardan todas o ninguna.
public interface IUnidadDeTrabajo
{
    // Reintenta si hay un conflicto de concurrencia; devuelve false si no se pudo guardar.
    // Si ya hay una transaccion en curso, el trabajo se une a ella.
    Task<bool> EjecutarAsync(Func<Task> trabajo);
}
