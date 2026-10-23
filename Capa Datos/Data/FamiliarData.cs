using Capa_Entidades;
using Capa_Entidades.Constantes;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

// En RRHH_Normalizado los datos personales del familiar viven en su Persona y
// LaboraInstitucion se calcula: tiene un vinculo laboral activo (como vw_FamiliarDetalle).
public class FamiliarData : RepositorioBase<Familiar>
{
    // Propiedad de Familiar -> columna de Persona.
    internal static readonly IReadOnlyDictionary<string, string> DatosDePersona = new Dictionary<string, string>
    {
        [nameof(Familiar.FamSexo)] = "Sexo",
        [nameof(Familiar.FamLugTrab)] = "LugarTrabajo",
        [nameof(Familiar.IdEstadoCivil)] = "IdEstadoCivil",
        [nameof(Familiar.NumAutogenSalud)] = "NumAutogenSalud",
        [nameof(Familiar.FamOcupacion)] = "Ocupacion",
        [nameof(Familiar.FamGradoInstruc)] = "GradoInstruccion",
        [nameof(Familiar.IdDiscapacidad)] = "IdDiscapacidad",
        [nameof(Familiar.Procedencia)] = "Procedencia",
        [nameof(Familiar.Enfermedad)] = "Enfermedad",
        [nameof(Familiar.TiempoEnfermedad)] = "TiempoEnfermedad",
        [nameof(Familiar.IdTipoSangre)] = "IdTipoSangre",
    };

    public FamiliarData(EvoRRLDbContext contexto) : base(contexto) { }

    protected override async Task Completar(List<Familiar> familiares)
    {
        if (familiares.Count == 0) return;
        var idsPersona = familiares.Select(f => f.IdPersona).Distinct().ToList();
        var personas = await PersonaTraduccion.LeerAsync(_contexto, idsPersona, DatosDePersona.Values);

        // El estado se guarda como bit; se compara con el valor que entrega la entidad.
        var conVinculo = (await _contexto.Set<Empleado>().AsNoTracking()
                .Where(e => e.IdPersona != null && e.Estado == EstadosEmpleado.Activo)
                .Select(e => e.IdPersona!.Value).ToListAsync())
            .ToHashSet();

        foreach (var f in familiares)
        {
            if (personas.TryGetValue(f.IdPersona, out var datos))
            {
                foreach (var (propiedad, columna) in DatosDePersona)
                    typeof(Familiar).GetProperty(propiedad)!.SetValue(f, datos[columna]);
            }
            f.LaboraInstitucion = conVinculo.Contains(f.IdPersona);
        }
    }

    protected override async Task DespuesDeGuardar(Familiar familiar, bool esNuevo)
    {
        var valores = DatosDePersona.ToDictionary(d => d.Value, d => typeof(Familiar).GetProperty(d.Key)!.GetValue(familiar));
        await PersonaTraduccion.EscribirAsync(_contexto, familiar.IdPersona, valores);
        await _contexto.SaveChangesAsync();
    }
}
