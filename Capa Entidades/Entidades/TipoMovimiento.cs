using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoMovimiento")]
    public class TipoMovimiento
    {
    [Key]
    public string? IdTipoMovimiento { get; set; }
    public string? Descripcion { get; set; }
    [Key]
    public string? IdTipodoc { get; set; }
    public string? TipoOperacion { get; set; }
    }
}
