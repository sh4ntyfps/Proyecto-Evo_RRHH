using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Vivienda")]
    public class Vivienda
    {
    public int? IdEmpleado { get; set; }
    [Key]
    public int IdVivienda { get; set; }
    public int? IdTenencia { get; set; }
    public int? IdTipo { get; set; }
    public int? IdMaterial { get; set; }
    public int? IdConservacion { get; set; }
    public string? NumDormitorio { get; set; }
    public string? NumServHig { get; set; }
    public int? IdUbicacion { get; set; }
    public int? IdAlumbrado { get; set; }
    public string? OtroAlumbrado { get; set; }
    public int? IdAgua { get; set; }
    public string? OtroAgua { get; set; }
    public int? IdExcretas { get; set; }
    public string? OtroExcreta { get; set; }
    public string? NumCompart { get; set; }
    public string? Observacion { get; set; }
    public string? NumPerDorm { get; set; }
    public bool? Telefono { get; set; }
    public bool? Cable { get; set; }
    public bool? Internet { get; set; }
    public bool? Otros { get; set; }
    }
}
