namespace Capa_Entidades
{
    public class ResponsableXUO
    {
    public int IdResponsable { get; set; }
    public int? Year { get; set; }
    public int? idAreaOrganiz { get; set; }
    public string? AreaOrganizacional { get; set; }
    public int? idDependeDe { get; set; }
    public int? IdEmpleado { get; set; }
    public DateTime? F_Inicio { get; set; }
    public DateTime? F_Termino { get; set; }
    }
}
