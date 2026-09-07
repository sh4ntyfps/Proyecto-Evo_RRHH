using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Familia")]
    public class Familia
    {
    [Key]
    public string? IdFamilia { get; set; }
    public string? Descripcion { get; set; }
    public string? Observaciones { get; set; }
    [Key]
    public string? IdClase { get; set; }
    [Key]
    public string? IdGrupo { get; set; }
    }
}
