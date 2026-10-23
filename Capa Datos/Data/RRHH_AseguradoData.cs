using Capa_Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Capa_Datos;

// En RRHH_Normalizado el sexo y el estado civil del asegurado son los de su Persona, y el
// vinculo familiar es el catalogo VinculoAsegurado (antes texto libre).
public class RRHH_AseguradoData : RepositorioBase<RRHH_Asegurado>
{
    public RRHH_AseguradoData(EvoRRLDbContext contexto) : base(contexto) { }

    protected override async Task Completar(List<RRHH_Asegurado> asegurados)
    {
        if (asegurados.Count == 0) return;
        var idsPersona = asegurados.Where(a => a.idPersona is not null).Select(a => a.idPersona!.Value).ToList();
        var personas = await PersonaTraduccion.LeerAsync(_contexto, idsPersona, new[] { "Sexo", "IdEstadoCivil" });
        var vinculos = await _contexto.Set<VinculoAseguradoFila>().AsNoTracking()
            .ToDictionaryAsync(v => v.IdVinculoAsegurado, v => v.Descripcion);

        foreach (var a in asegurados)
        {
            if (a.idPersona is { } idPersona && personas.TryGetValue(idPersona, out var datos))
            {
                a.Sexo = datos["Sexo"] as string;
                a.idEstadoCivil = datos["IdEstadoCivil"] as int?;
            }
            var idVinculo = _contexto.Entry(a).Property<int?>("IdVinculoAsegurado").CurrentValue;
            a.VinculoFamiliar = idVinculo is { } id ? vinculos.GetValueOrDefault(id) : null;
        }
    }

    protected override void AntesDeGuardar(EntityEntry<RRHH_Asegurado> entrada, bool esNuevo)
    {
        var vinculo = entrada.Entity.VinculoFamiliar?.Trim();
        Asignar(entrada, "IdVinculoAsegurado", string.IsNullOrEmpty(vinculo) ? null : IdVinculo(vinculo));
    }

    protected override async Task DespuesDeGuardar(RRHH_Asegurado asegurado, bool esNuevo)
    {
        if (asegurado.idPersona is not { } idPersona) return;
        await PersonaTraduccion.EscribirAsync(_contexto, idPersona, new Dictionary<string, object?>
        {
            ["Sexo"] = asegurado.Sexo,
            ["IdEstadoCivil"] = asegurado.idEstadoCivil,
        });
        await _contexto.SaveChangesAsync();
    }

    // Un vinculo escrito que aun no esta en el catalogo se agrega.
    private int IdVinculo(string descripcion)
    {
        var vinculos = _contexto.Set<VinculoAseguradoFila>();
        var existente = vinculos.Local.FirstOrDefault(v => string.Equals(v.Descripcion, descripcion, StringComparison.OrdinalIgnoreCase))
            ?? vinculos.FirstOrDefault(v => v.Descripcion == descripcion);
        if (existente is not null)
            return existente.IdVinculoAsegurado;

        var nuevo = new VinculoAseguradoFila { Descripcion = descripcion };
        vinculos.Add(nuevo);
        _contexto.SaveChanges();
        return nuevo.IdVinculoAsegurado;
    }
}
