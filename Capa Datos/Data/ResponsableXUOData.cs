using Capa_Entidades;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

// En RRHH_Normalizado el nombre del area y de quien depende salen de EstructOrganiz.
public class ResponsableXUOData : RepositorioBase<ResponsableXUO>
{
    public ResponsableXUOData(EvoRRLDbContext contexto) : base(contexto) { }

    protected override async Task Completar(List<ResponsableXUO> responsables)
    {
        if (responsables.Count == 0) return;
        var areas = await _contexto.Set<EstructOrganiz>().AsNoTracking()
            .Select(a => new { a.Year, a.idAreaOrganiz, a.AreaOrganizacional, a.idDependeDe })
            .ToListAsync();
        var porClave = areas.ToDictionary(a => (a.Year, a.idAreaOrganiz));

        foreach (var r in responsables)
        {
            if (r.Year is { } year && r.idAreaOrganiz is { } id && porClave.TryGetValue((year, id), out var area))
            {
                r.AreaOrganizacional = area.AreaOrganizacional;
                r.idDependeDe = area.idDependeDe;
            }
        }
    }
}
