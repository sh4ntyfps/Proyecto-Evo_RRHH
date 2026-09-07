using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("TipoTransaccion")]
    public class TipoTransaccion
    {
    [Key]
    public string? Anio { get; set; }
    [Key]
    public string? IdTipoTransaccion { get; set; }
    public string? Descripcion { get; set; }
    }
}
