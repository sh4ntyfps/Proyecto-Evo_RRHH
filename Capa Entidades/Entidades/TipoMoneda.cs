using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoMoneda")]
    public class TipoMoneda
    {
    [Key]
    public int IdTipoMoneda { get; set; }
    public string? Descripcion { get; set; }
    public string? Abreviatura { get; set; }
    }
}
