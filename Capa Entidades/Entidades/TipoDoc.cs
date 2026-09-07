using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoDoc")]
    public class TipoDoc
    {
    [Key]
    public string? IdTipodoc { get; set; }
    public string? Descripcion { get; set; }
    public int? Numero { get; set; }
    public byte? Serie { get; set; }
    [Key]
    public string? Anio { get; set; }
    }
}
