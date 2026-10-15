using System.Data;
using Capa_Entidades.Contratos;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

// Transaccion serializable con reintentos: evita que dos usuarios obtengan el mismo correlativo.
public class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private const int Intentos = 5;

    private readonly EvoRRLDbContext _contexto;

    public UnidadDeTrabajo(EvoRRLDbContext contexto) => _contexto = contexto;

    public async Task<bool> EjecutarAsync(Func<Task> trabajo)
    {
        // Dentro de una transaccion abierta el trabajo se une a ella; si falla, reintenta la externa.
        if (_contexto.Database.CurrentTransaction is not null)
        {
            await trabajo();
            return true;
        }

        for (var intento = 1; intento <= Intentos; intento++)
        {
            try
            {
                await using var transaccion = await _contexto.Database.BeginTransactionAsync(IsolationLevel.Serializable);
                await trabajo();
                await transaccion.CommitAsync();
                return true;
            }
            catch (Exception ex) when (EsTransitorio(ex) && intento < Intentos)
            {
                _contexto.ChangeTracker.Clear();
            }
        }
        return false;
    }

    // Bloqueos, deadlocks y clave duplicada (otro usuario tomo el mismo numero): se puede reintentar.
    private static bool EsTransitorio(Exception ex)
    {
        for (var actual = ex; actual is not null; actual = actual.InnerException)
        {
            if (actual is SqlException { } sql && (sql.Number is 1204 or 1205 or 1222 or 2601 or 2627))
                return true;
        }
        return false;
    }
}
