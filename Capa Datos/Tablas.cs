namespace Capa_Datos;

// Tablas y vistas de RRHH_Normalizado que no tienen entidad propia: solo las usan los
// repositorios que traducen (EmpleadoData, FamiliarData, ViviendaData...). No salen de esta capa.

internal class VinculoAseguradoFila
{
    public int IdVinculoAsegurado { get; set; }
    public string Descripcion { get; set; } = "";
}

internal class ViviendaServicioFila
{
    public int IdVivienda { get; set; }
    public int IdServicio { get; set; }
}

internal class FuncFamPreguntaFila
{
    public byte IdPregunta { get; set; }
    public string Codigo { get; set; } = "";
}

internal class FuncFamRespuestaFila
{
    public int IdEmpleado { get; set; }
    public byte IdPregunta { get; set; }
    public string? Respuesta { get; set; }
}
