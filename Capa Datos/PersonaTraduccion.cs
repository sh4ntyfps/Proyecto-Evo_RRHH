using Capa_Entidades;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

// Lee y escribe las columnas de Persona que no tienen propiedad en la entidad (shadow):
// los datos personales que en RRHH_Nuevo estaban repetidos en Empleado, Familiar y RRHH_Asegurado.
internal static class PersonaTraduccion
{
    public static async Task<Dictionary<int, Dictionary<string, object?>>> LeerAsync(
        EvoRRLDbContext contexto, IReadOnlyCollection<int> idsPersona, IEnumerable<string> columnas)
    {
        var resultado = new Dictionary<int, Dictionary<string, object?>>();
        if (idsPersona.Count == 0) return resultado;

        var columnasLista = columnas.ToList();
        var yaSeguidas = contexto.ChangeTracker.Entries<Persona>().Select(e => e.Entity.IdPersona).ToHashSet();
        var ids = idsPersona.Distinct().ToList();
        // Para listas grandes (todos los empleados) es mas rapido leer la tabla completa que un IN enorme.
        var personas = ids.Count switch
        {
            1 => await contexto.Set<Persona>().Where(p => p.IdPersona == ids[0]).ToListAsync(),
            <= 200 => await contexto.Set<Persona>().Where(p => ids.Contains(p.IdPersona)).ToListAsync(),
            _ => await contexto.Set<Persona>().ToListAsync()
        };

        foreach (var persona in personas)
        {
            var entrada = contexto.Entry(persona);
            resultado[persona.IdPersona] = columnasLista.ToDictionary(c => c, c => entrada.Property(c).CurrentValue);
            // Solo se leyo para traducir: no se deja en seguimiento lo que no estaba.
            if (!yaSeguidas.Contains(persona.IdPersona))
                entrada.State = EntityState.Detached;
        }
        return resultado;
    }

    // Deja los cambios en el contexto; los guarda el SaveChanges de quien llama.
    public static async Task EscribirAsync(EvoRRLDbContext contexto, int idPersona, IReadOnlyDictionary<string, object?> valores)
    {
        var persona = await contexto.Set<Persona>().FindAsync(idPersona);
        if (persona is null) return;
        var entrada = contexto.Entry(persona);
        foreach (var (columna, valor) in valores)
        {
            var propiedad = entrada.Property(columna);
            var texto = valor as string;
            propiedad.CurrentValue = texto is not null && string.IsNullOrWhiteSpace(texto) ? null : valor;
        }
    }
}
