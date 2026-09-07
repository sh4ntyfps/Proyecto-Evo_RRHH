using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("RegAsisDiario")]
    public class RegAsisDiario
    {
    [Key]
    public DateTime Fecha { get; set; }
    [Key]
    public int IdEmpleado { get; set; }
    public string? MinNormales { get; set; }
    public string? MinTarde { get; set; }
    public string? Estado { get; set; }
    }
}
