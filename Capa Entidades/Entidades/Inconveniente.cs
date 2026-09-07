using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Inconveniente")]
    public class Inconveniente
    {
    [Key]
    public int IdObjetivo { get; set; }
    [Key]
    public int Id_Actividad { get; set; }
    [Key]
    public int IdInconveniente { get; set; }
    [Column("Inconveniente")]
    public string? Descripcion { get; set; }
    public int Importancia { get; set; }
    public string? Desventaja { get; set; }
    public string? Observacion { get; set; }
    }
}
