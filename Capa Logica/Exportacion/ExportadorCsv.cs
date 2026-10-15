using System.Text;

namespace Capa_Logica.Exportacion;

// CSV separado por ";" y lineas CRLF.
// Nota: GetBytes no escribe el BOM aunque se use UTF8Encoding(true); se conserva asi porque
// es lo que el sistema genera hoy (Excel puede mostrar mal las tildes).
public sealed class ExportadorCsv : IExportadorTabla
{
    public string Formato => "csv";

    public ArchivoExportado Exportar(IReadOnlyList<string[]> filas)
    {
        var contenido = string.Join("\r\n", filas.Select(f => string.Join(";", f.Select(EscaparCampo))));
        return new ArchivoExportado(new UTF8Encoding(true).GetBytes(contenido), "text/csv; charset=utf-8", "csv");
    }

    internal static string EscaparCampo(string valor)
    {
        if (valor.Contains(';') || valor.Contains('"') || valor.Contains('\n') || valor.Contains('\r'))
            return $"\"{valor.Replace("\"", "\"\"")}\"";
        return valor;
    }
}
