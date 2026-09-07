namespace Proyecto_Evo_RRLL.Models.ViewModels;

public class EmpleadoDetalleViewModel
{
    public int IdEmpleado { get; set; }

    public string? NombresCompletos { get; set; }

    public string? TipoDoc { get; set; }

    public string? NumDoc { get; set; }

    public string? Sexo { get; set; }

    public string? EstadoCivil { get; set; }

    public string? Cargo { get; set; }

    public string? Area { get; set; }

    public string? Local { get; set; }

    public string? Horario { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public string? TipoTrabajador { get; set; }

    public string? RegimenPension { get; set; }

    public bool TieneFoto { get; set; }
}