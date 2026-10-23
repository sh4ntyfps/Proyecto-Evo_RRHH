using Capa_Entidades;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

// En RRHH_Normalizado los servicios de la vivienda (Telefono, Cable, Internet, Otros) son
// filas de ViviendaServicio contra el catalogo ServicioViv.
public class ViviendaData : RepositorioBase<Vivienda>
{
    // Propiedad de Vivienda -> IdServicio de ServicioViv.
    internal static readonly IReadOnlyDictionary<string, int> Servicios = new Dictionary<string, int>
    {
        [nameof(Vivienda.Telefono)] = 1,
        [nameof(Vivienda.Cable)] = 2,
        [nameof(Vivienda.Internet)] = 3,
        [nameof(Vivienda.Otros)] = 4,
    };

    public ViviendaData(EvoRRLDbContext contexto) : base(contexto) { }

    protected override async Task Completar(List<Vivienda> viviendas)
    {
        if (viviendas.Count == 0) return;
        var ids = viviendas.Select(v => v.IdVivienda).ToList();
        var filas = ids.Count == 1
            ? await _contexto.Set<ViviendaServicioFila>().AsNoTracking().Where(s => s.IdVivienda == ids[0]).ToListAsync()
            : await _contexto.Set<ViviendaServicioFila>().AsNoTracking().ToListAsync();
        var porVivienda = filas.ToLookup(s => s.IdVivienda, s => s.IdServicio);

        foreach (var v in viviendas)
        {
            var tiene = porVivienda[v.IdVivienda].ToHashSet();
            foreach (var (propiedad, idServicio) in Servicios)
                typeof(Vivienda).GetProperty(propiedad)!.SetValue(v, tiene.Contains(idServicio));
        }
    }

    protected override async Task DespuesDeGuardar(Vivienda vivienda, bool esNuevo)
    {
        var tabla = _contexto.Set<ViviendaServicioFila>();
        var actuales = await tabla.Where(s => s.IdVivienda == vivienda.IdVivienda).ToListAsync();
        foreach (var (propiedad, idServicio) in Servicios)
        {
            var marcado = typeof(Vivienda).GetProperty(propiedad)!.GetValue(vivienda) as bool? == true;
            var fila = actuales.FirstOrDefault(s => s.IdServicio == idServicio);
            if (marcado && fila is null)
                tabla.Add(new ViviendaServicioFila { IdVivienda = vivienda.IdVivienda, IdServicio = idServicio });
            else if (!marcado && fila is not null)
                tabla.Remove(fila);
        }
        await _contexto.SaveChangesAsync();
    }

    protected override async Task AntesDeEliminar(Vivienda vivienda)
        => _contexto.Set<ViviendaServicioFila>().RemoveRange(
            await _contexto.Set<ViviendaServicioFila>().Where(s => s.IdVivienda == vivienda.IdVivienda).ToListAsync());
}
