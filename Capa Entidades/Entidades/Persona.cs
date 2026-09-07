using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Persona")]
    public class Persona
    {
    [Key]
    public int IdPersona { get; set; }
    public string? Nombres { get; set; }
    public string? Apellido_Paterno { get; set; }
    public string? Apellido_Materno { get; set; }
    public int? TipoDocID { get; set; }
    public string? NumDocID { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? IdUbigeo { get; set; }
    public string? UbigeoDireccion { get; set; }
    public string? TipoPersona { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public string? NumCelular { get; set; }
    public string? FonoCentroLab { get; set; }
    }
}
