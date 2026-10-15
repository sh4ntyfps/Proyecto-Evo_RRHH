namespace Capa_Entidades
{
    public class Resolucion
    {
    public int idEmpleado { get; set; }
    public int idResolucion { get; set; }
    public int? idTipoResolucion { get; set; }
    public DateOnly? FechaResolucion { get; set; }
    public string? NumeroResolucion { get; set; }
    }
}
