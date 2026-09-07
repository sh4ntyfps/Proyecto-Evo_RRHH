using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capa_Entidades
{
    [Table("Empleado")]
    public class Empleado
    {
    [Key]
    public int IdEmpleado { get; set; }
    public int Year { get; set; }
    public int idAreaOrganiz { get; set; }
    public int? IdPersona { get; set; }
    public string? ExpSocial { get; set; }
    public DateTime? FechaIngreso { get; set; }
    public int? IdAFP { get; set; }
    public string? Brevete { get; set; }
    public int? IdEstadoCivil { get; set; }
    public string? Sexo { get; set; }
    public string? CtaCorriente { get; set; }
    public string? NroAFP { get; set; }
    public DateTime? FechaAFP { get; set; }
    public int? IdSeguro { get; set; }
    public string? NroSeguro { get; set; }
    public int? IdTipoSangre { get; set; }
    public string? DecLey { get; set; }
    public byte[]? Foto { get; set; }
    public int? IdTipoTrabajador { get; set; }
    public string? Estado { get; set; }
    public int? IdCargo { get; set; }
    public int? IdDiscapacidad { get; set; }
    public string? Profesion { get; set; }
    public bool? JefeHogar { get; set; }
    public string? CargaFam { get; set; }
    public string? NumHijos { get; set; }
    public int? IdViveCon { get; set; }
    public string? ObsFam { get; set; }
    public string? AspecSalud { get; set; }
    public string? AspecEducacion { get; set; }
    public string? AspecRecreacion { get; set; }
    public string? AspecPsico { get; set; }
    public string? AspecSocial { get; set; }
    public string? DiagnosSocial { get; set; }
    public int? IdNacionalidad { get; set; }
    public string? NumLibretaMilitar { get; set; }
    public string? GradoInstruccion { get; set; }
    public int? IdRegimenPen { get; set; }
    public string? NumRUC { get; set; }
    public string? NumAutogenSalud { get; set; }
    public string? NumResolCese { get; set; }
    public DateTime? FechaResolCese { get; set; }
    public string? NumResol20530 { get; set; }
    public DateTime? FechaResol20530 { get; set; }
    public string? NumResolPenMen { get; set; }
    public DateTime? FechaResolPenMen { get; set; }
    public string? NumResoIngreEsta { get; set; }
    public DateTime? FechaIngreEstado { get; set; }
    public DateTime? FechaResolIngreEsta { get; set; }
    public string? NumResoIngreInstitu { get; set; }
    public DateTime? FechaResoIngreInstitu { get; set; }
    public int? idHorario { get; set; }
    [NotMapped]
    public Guid? Userid { get; set; }
    public int? idLocal { get; set; }
    }
}
