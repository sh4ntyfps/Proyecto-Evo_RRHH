using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Familiar")]
    public class Familiar
    {
    [Key]
    public int IdEmpleado { get; set; }
    [Key]
    public int IdPersona { get; set; }
    public string? FamSexo { get; set; }
    public string? FamLugTrab { get; set; }
    public int? IdEstadoCivil { get; set; }
    public string? NumAutogenSalud { get; set; }
    public string? FamOcupacion { get; set; }
    public string? FamGradoInstruc { get; set; }
    public int? IdTipoFam { get; set; }
    public bool? ViveCasa { get; set; }
    public bool? LaboraInstitucion { get; set; }
    public int? IdDiscapacidad { get; set; }
    public string? Procedencia { get; set; }
    public string? Enfermedad { get; set; }
    public string? TiempoEnfermedad { get; set; }
    public int? IdTipoSangre { get; set; }
    }
}
