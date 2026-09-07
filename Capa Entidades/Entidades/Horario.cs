using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Horario")]
    public class Horario
    {
    [Key]
    public int idHorario { get; set; }
    public string? Ingreso1 { get; set; }
    public string? Salida1 { get; set; }
    public string? Ingreso2 { get; set; }
    public string? Salida2 { get; set; }
    public string? Descrip_Horario { get; set; }
    public bool? Estado { get; set; }
    public bool? SgtDia { get; set; }
    }
}
