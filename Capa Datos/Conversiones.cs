using System.Globalization;
using Capa_Entidades.Constantes;

namespace Capa_Datos;

// Traducen los tipos de las entidades (texto, como en RRHH_Nuevo) a los de RRHH_Normalizado.
// Un texto que no se puede convertir se guarda como NULL, igual que hizo la migracion (TRY_CONVERT).
internal static class Conversiones
{
    // 'hh:mm:ss' <-> time
    public static TimeSpan? AHora(string? texto)
        => TimeSpan.TryParse(texto?.Trim(), CultureInfo.InvariantCulture, out var hora) ? hora : null;

    public static string? DeHora(TimeSpan? hora) => hora?.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);

    // texto numerico <-> smallint / tinyint
    public static short? AShort(string? texto)
        => short.TryParse(texto?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) ? n : null;

    public static byte? AByte(string? texto)
        => byte.TryParse(texto?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) ? n : null;

    public static string? DeNumero(short? numero) => numero?.ToString(CultureInfo.InvariantCulture);

    public static string? DeNumero(byte? numero) => numero?.ToString(CultureInfo.InvariantCulture);

    // Empleado.Estado 'A'/'I' <-> bit
    public static bool? AEstado(string? estado) => estado?.Trim() switch
    {
        EstadosEmpleado.Activo => true,
        EstadosEmpleado.Inactivo => false,
        _ => null
    };

    public static string? DeEstado(bool? activo) => activo switch
    {
        true => EstadosEmpleado.Activo,
        false => EstadosEmpleado.Inactivo,
        null => null
    };

    // RegAsisDiario.MinTarde 'hh:mm:ss' (o minutos) <-> segundos enteros
    public static int? ASegundos(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return null;
        if (texto.Contains(':'))
            return AHora(texto) is { } hora ? (int)hora.TotalSeconds : null;
        return int.TryParse(texto.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var minutos) ? minutos * 60 : null;
    }

    public static string? DeSegundos(int? segundos)
        => segundos is null ? null : DeHora(TimeSpan.FromSeconds(segundos.Value));
}
