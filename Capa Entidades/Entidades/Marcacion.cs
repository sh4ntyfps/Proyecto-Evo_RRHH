using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Marcacion")]
    public class Marcacion
    {
    [Key]
    public DateTime Fecha { get; set; }
    public string? Estado { get; set; }
    [Key]
    public int IdEmpleado { get; set; }
    public string? Lugar { get; set; }
    public int? idHorario { get; set; }
    }
}
