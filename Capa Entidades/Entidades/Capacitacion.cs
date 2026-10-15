namespace Capa_Entidades
{
    public class Capacitacion
    {
    public int IdCapacitacion { get; set; }
    public string? NombreCurso { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? NumHoras { get; set; }
    public string? InstitucionOrganizadora { get; set; }
    public int? IdEmpleado { get; set; }
    }
}
