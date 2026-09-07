using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RRHH_AspSocio")]
    public class RRHH_AspSocio
    {
    [Key]
    public int idAspSocio { get; set; }
    public int? IdEmpleado { get; set; }
    public int? idPregunta1 { get; set; }
    public int? idPregunta2 { get; set; }
    public int? idPregunta3 { get; set; }
    public string? PrincipProblemas { get; set; }
    public string? PropMejoraInstitu { get; set; }
    public string? hobby { get; set; }
    public string? ActRecreaInstitu { get; set; }
    }
}
