namespace Capa_Logica.Exportacion;

// Elige el exportador segun el formato pedido entre todos los registrados en el contenedor.
public sealed class ExportadorTablas
{
    private readonly IReadOnlyDictionary<string, IExportadorTabla> _exportadores;

    public ExportadorTablas(IEnumerable<IExportadorTabla> exportadores)
        => _exportadores = exportadores.ToDictionary(e => e.Formato, StringComparer.OrdinalIgnoreCase);

    public bool Soporta(string formato) => _exportadores.ContainsKey(formato);

    public ArchivoExportado Exportar(string formato, IReadOnlyList<string[]> filas) => _exportadores[formato].Exportar(filas);
}
