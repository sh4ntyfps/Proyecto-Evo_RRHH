namespace Capa_Entidades
{
    public class Usuario
    {
    public int IdUsuario { get; set; }
    public string? Login { get; set; }
    public string? Descripcion { get; set; }
    public DateTime? Fecha { get; set; }
    public bool? Estado { get; set; }
    public int? IdEmpleado { get; set; }
    public string? PasswordHash { get; set; }
    }
}
