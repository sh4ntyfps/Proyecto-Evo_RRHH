namespace Proyecto_Evo_RRLL.Models.ViewModels;

public static class CatalogoNavegacion
{
    public static readonly IReadOnlyDictionary<string, string> Items = new Dictionary<string, string>
    {
        ["Cargo"] = "Cargos",
        ["MotivoBaja"] = "Motivos de Baja",
        ["EstadoCivil"] = "Estado Civil",
        ["TipoDocID"] = "Tipos de Documento",
        ["TipoTrabajador"] = "Tipos de Trabajador",
        ["TipoSangre"] = "Grupos Sanguíneos",
        ["Nacionalidad"] = "Nacionalidades",
        ["RegimenPension"] = "Regímenes de Pensión",
        ["ViveCon"] = "¿Vive con?",
        ["TipoPermiso"] = "Tipos de Permiso",
        ["Motivo_Perm"] = "Motivos de Permiso",
        ["TipoEstudio"] = "Tipos de Estudio",
        ["TipoInstitucion"] = "Tipos de Institución",
        ["Discapacidad"] = "Discapacidades",
        ["Limitacion"] = "Limitaciones",
        ["TipoComportamiento"] = "Tipos de Comportamiento",
        ["TipoResolucion"] = "Tipos de Resolución"
    };
}