namespace Capa_Entidades
{
    public class EstudiosRealizado
    {
    public int IdTipoEstudios { get; set; }
    public int IdEmpleado { get; set; }
    public int Correlativo { get; set; }
    public string? CentroEstudios { get; set; }
    public DateTime? AñoInicio { get; set; }
    public string? Especialidad { get; set; }
    public DateTime? AñoTermino { get; set; }
    public int? IdEstadoEstudio { get; set; }
    }
}
