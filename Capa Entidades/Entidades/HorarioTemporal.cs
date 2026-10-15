namespace Capa_Entidades
{
    public class HorarioTemporal
    {
    public int idEmpleado { get; set; }
    public int N { get; set; }
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public int? HorarioAsignado { get; set; }
    public string? NroDocumento { get; set; }
    public string? Sisgedo { get; set; }
    public DateTime? FechaRegistro { get; set; }
    }
}
