using Capa_Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Capa_Datos;

// En RRHH_Normalizado el numero de trabajadores del area se calcula del historial
// Empleado_Area (como vw_EstructOrganizDotacion).
public class EstructOrganizData : RepositorioBase<EstructOrganiz>
{
    public EstructOrganizData(EvoRRLDbContext contexto) : base(contexto) { }

    protected override async Task Completar(List<EstructOrganiz> areas)
    {
        if (areas.Count == 0) return;
        var dotacion = (await _contexto.Set<Empleado_Area>().AsNoTracking()
                .GroupBy(a => new { a.Year, a.idAreaOrganiz })
                .Select(g => new { g.Key.Year, g.Key.idAreaOrganiz, Total = g.Count() })
                .ToListAsync())
            .ToDictionary(x => (x.Year, x.idAreaOrganiz), x => x.Total);

        foreach (var area in areas)
            area.NroTrabajaUO = dotacion.GetValueOrDefault((area.Year, area.idAreaOrganiz));
    }

    // Abrev y Sigla son obligatorias en RRHH_Normalizado; vacias se guardan como texto vacio.
    protected override void AntesDeGuardar(EntityEntry<EstructOrganiz> entrada, bool esNuevo)
    {
        entrada.Entity.Abrev ??= "";
        entrada.Entity.Sigla ??= "";
    }
}
