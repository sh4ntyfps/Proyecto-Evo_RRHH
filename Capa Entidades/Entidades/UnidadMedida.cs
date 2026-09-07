using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("UnidadMedida")]
    public class UnidadMedida
    {
    [Key]
    public string? IdUnidadMedida { get; set; }
    public string? Descripcion { get; set; }
    public string? Abreviado { get; set; }
    }
}
