using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Institucion")]
    public class Institucion
    {
    [Key]
    public int IdInstitucion { get; set; }
    public string? Descripcion { get; set; }
    public string? Direccion { get; set; }
    }
}
