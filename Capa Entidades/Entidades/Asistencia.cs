namespace Capa_Entidades
{
    public class Asistencia
    {
    public DateTime Fecha { get; set; }
    public int IdEmpleado { get; set; }
    public string? Hor_Ent { get; set; }
    public string? Hor_Sal { get; set; }
    public string? Alm_Sal { get; set; }
    public string? Alm_Ent { get; set; }
    public bool? Flag_Ent { get; set; }
    public bool? Flag_Sal { get; set; }
    public bool? Estado { get; set; }
    public int? idHorario { get; set; }
    public string? Usuario { get; set; }
    public string? PC { get; set; }
    }
}
