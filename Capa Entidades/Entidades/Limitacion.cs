using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Limitacion")]
    public class Limitacion
    {
    [Key]
    public int IdLimitacion { get; set; }
    public string? Descripcion { get; set; }
    }
}
