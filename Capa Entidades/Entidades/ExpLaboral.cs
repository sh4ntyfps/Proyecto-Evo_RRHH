namespace Capa_Entidades
{
    public class ExpLaboral
    {
    public int IdEmpleado { get; set; }
    public int IdExpLab { get; set; }
    public string? NomInstitucion { get; set; }
    public string? Cargo { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public DateTime? FechaEgreso { get; set; }
    public string? UnidadOrganica { get; set; }
    public int? IdTipoInstitucion { get; set; }
    }
}
