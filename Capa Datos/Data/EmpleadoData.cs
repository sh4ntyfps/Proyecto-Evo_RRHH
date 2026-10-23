using Capa_Entidades;
using Capa_Entidades.Contratos;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

// En RRHH_Normalizado el empleado se reparte en varias tablas; este repositorio entrega
// y recibe el mismo Empleado de siempre:
//   Persona        -> datos personales (Sexo, estado civil, profesion, foto...)
//   Empleado_Area  -> Year / idAreaOrganiz (el area vigente es la del anio mas reciente)
//   RRHH_AspSocio  -> aspectos sociales (ObsFam, AspecSalud... DiagnosSocial)
//   Resolucion     -> resoluciones de ingreso al Estado y a la institucion
//   Familiar       -> NumHijos (se calcula)
public class EmpleadoData : RepositorioBase<Empleado>, IEmpleadoData
{
    // Propiedad de Empleado -> columna de Persona.
    internal static readonly IReadOnlyDictionary<string, string> DatosDePersona = new Dictionary<string, string>
    {
        [nameof(Empleado.Sexo)] = "Sexo",
        [nameof(Empleado.IdEstadoCivil)] = "IdEstadoCivil",
        [nameof(Empleado.IdNacionalidad)] = "IdNacionalidad",
        [nameof(Empleado.IdTipoSangre)] = "IdTipoSangre",
        [nameof(Empleado.IdDiscapacidad)] = "IdDiscapacidad",
        [nameof(Empleado.NumAutogenSalud)] = "NumAutogenSalud",
        [nameof(Empleado.NumRUC)] = "NumRUC",
        [nameof(Empleado.Brevete)] = "Brevete",
        [nameof(Empleado.NumLibretaMilitar)] = "NumLibretaMilitar",
        [nameof(Empleado.GradoInstruccion)] = "GradoInstruccion",
        [nameof(Empleado.Profesion)] = "Profesion",
        [nameof(Empleado.Foto)] = "Foto",
    };

    // Mismo nombre en Empleado y en RRHH_AspSocio.
    internal static readonly string[] DatosDeAspSocio =
    {
        nameof(Empleado.ObsFam), nameof(Empleado.AspecSalud), nameof(Empleado.AspecEducacion),
        nameof(Empleado.AspecRecreacion), nameof(Empleado.AspecPsico), nameof(Empleado.AspecSocial),
        nameof(Empleado.DiagnosSocial)
    };

    private const int TipoFamiliarHijo = 4;
    private const string IngresoAlEstado = "Ingreso al Estado";
    private const string IngresoALaInstitucion = "Ingreso a la Institucion";

    public EmpleadoData(EvoRRLDbContext contexto) : base(contexto) { }

    protected override async Task Completar(List<Empleado> empleados)
    {
        if (empleados.Count == 0) return;
        var ids = empleados.Select(e => e.IdEmpleado).ToList();
        var uno = ids.Count == 1 ? ids[0] : (int?)null;

        var idsPersona = empleados.Where(e => e.IdPersona is not null).Select(e => e.IdPersona!.Value).ToList();
        var personas = await PersonaTraduccion.LeerAsync(_contexto, idsPersona, DatosDePersona.Values);

        var areas = (await _contexto.Set<Empleado_Area>().AsNoTracking()
                .Where(a => uno == null || a.IdEmpleado == uno).ToListAsync())
            .GroupBy(a => a.IdEmpleado)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(a => a.Year).ThenByDescending(a => a.idAreaOrganiz).First());

        var aspectos = (await _contexto.Set<RRHH_AspSocio>().AsNoTracking()
                .Where(a => a.IdEmpleado != null && (uno == null || a.IdEmpleado == uno))
                .Select(a => new
                {
                    IdEmpleado = a.IdEmpleado!.Value,
                    a.idAspSocio,
                    ObsFam = EF.Property<string?>(a, "ObsFam"),
                    AspecSalud = EF.Property<string?>(a, "AspecSalud"),
                    AspecEducacion = EF.Property<string?>(a, "AspecEducacion"),
                    AspecRecreacion = EF.Property<string?>(a, "AspecRecreacion"),
                    AspecPsico = EF.Property<string?>(a, "AspecPsico"),
                    AspecSocial = EF.Property<string?>(a, "AspecSocial"),
                    DiagnosSocial = EF.Property<string?>(a, "DiagnosSocial"),
                }).ToListAsync())
            .GroupBy(a => a.IdEmpleado)
            .ToDictionary(g => g.Key, g => g.OrderBy(a => a.idAspSocio).First());

        var hijos = (await _contexto.Set<Familiar>().AsNoTracking()
                .Where(f => f.IdTipoFam == TipoFamiliarHijo && (uno == null || f.IdEmpleado == uno))
                .GroupBy(f => f.IdEmpleado).Select(g => new { g.Key, Total = g.Count() }).ToListAsync())
            .ToDictionary(x => x.Key, x => x.Total);

        var (tipoEstado, tipoInstitucion) = await TiposDeIngresoAsync();
        var ingresos = await _contexto.Set<Resolucion>().AsNoTracking()
            .Where(r => (r.idTipoResolucion == tipoEstado || r.idTipoResolucion == tipoInstitucion) && (uno == null || r.idEmpleado == uno))
            .Select(r => new { r.idEmpleado, r.idResolucion, r.idTipoResolucion, r.NumeroResolucion, r.FechaResolucion, FechaVigencia = EF.Property<DateOnly?>(r, "FechaVigencia") })
            .ToListAsync();

        foreach (var e in empleados)
        {
            if (e.IdPersona is { } idPersona && personas.TryGetValue(idPersona, out var datos))
            {
                foreach (var (propiedad, columna) in DatosDePersona)
                    typeof(Empleado).GetProperty(propiedad)!.SetValue(e, datos[columna]);
            }

            if (areas.TryGetValue(e.IdEmpleado, out var area))
            {
                e.Year = area.Year;
                e.idAreaOrganiz = area.idAreaOrganiz;
            }

            if (aspectos.TryGetValue(e.IdEmpleado, out var asp))
            {
                e.ObsFam = asp.ObsFam;
                e.AspecSalud = asp.AspecSalud;
                e.AspecEducacion = asp.AspecEducacion;
                e.AspecRecreacion = asp.AspecRecreacion;
                e.AspecPsico = asp.AspecPsico;
                e.AspecSocial = asp.AspecSocial;
                e.DiagnosSocial = asp.DiagnosSocial;
            }

            e.NumHijos = hijos.GetValueOrDefault(e.IdEmpleado).ToString();

            var estado = ingresos.Where(r => r.idEmpleado == e.IdEmpleado && r.idTipoResolucion == tipoEstado).MaxBy(r => r.idResolucion);
            e.NumResoIngreEsta = estado?.NumeroResolucion;
            e.FechaResolIngreEsta = AFecha(estado?.FechaResolucion);
            e.FechaIngreEstado = AFecha(estado?.FechaVigencia);

            var institucion = ingresos.Where(r => r.idEmpleado == e.IdEmpleado && r.idTipoResolucion == tipoInstitucion).MaxBy(r => r.idResolucion);
            e.NumResoIngreInstitu = institucion?.NumeroResolucion;
            e.FechaResoIngreInstitu = AFecha(institucion?.FechaResolucion);
        }
    }

    protected override async Task DespuesDeGuardar(Empleado empleado, bool esNuevo)
    {
        if (empleado.IdPersona is { } idPersona)
        {
            var valores = DatosDePersona.ToDictionary(d => d.Value, d => typeof(Empleado).GetProperty(d.Key)!.GetValue(empleado));
            await PersonaTraduccion.EscribirAsync(_contexto, idPersona, valores);
        }

        await GuardarAreaAsync(empleado);
        await GuardarAspectosAsync(empleado);
        await GuardarIngresosAsync(empleado);
        await _contexto.SaveChangesAsync();
    }

    // El area es historial: si cambia, se agrega una fila para el anio indicado.
    private async Task GuardarAreaAsync(Empleado empleado)
    {
        if (empleado.Year <= 0 || empleado.idAreaOrganiz <= 0) return;

        var actual = (await _contexto.Set<Empleado_Area>().AsNoTracking()
                .Where(a => a.IdEmpleado == empleado.IdEmpleado).ToListAsync())
            .OrderByDescending(a => a.Year).ThenByDescending(a => a.idAreaOrganiz).FirstOrDefault();
        if (actual is not null && actual.Year == empleado.Year && actual.idAreaOrganiz == empleado.idAreaOrganiz)
            return;

        var existe = await _contexto.Set<Empleado_Area>().AnyAsync(a =>
            a.IdEmpleado == empleado.IdEmpleado && a.Year == empleado.Year && a.idAreaOrganiz == empleado.idAreaOrganiz);
        if (!existe)
            _contexto.Set<Empleado_Area>().Add(new Empleado_Area { IdEmpleado = empleado.IdEmpleado, Year = empleado.Year, idAreaOrganiz = empleado.idAreaOrganiz });
    }

    private async Task GuardarAspectosAsync(Empleado empleado)
    {
        var valores = new Dictionary<string, object?>
        {
            ["ObsFam"] = empleado.ObsFam, ["AspecSalud"] = empleado.AspecSalud, ["AspecEducacion"] = empleado.AspecEducacion,
            ["AspecRecreacion"] = empleado.AspecRecreacion, ["AspecPsico"] = empleado.AspecPsico,
            ["AspecSocial"] = empleado.AspecSocial, ["DiagnosSocial"] = empleado.DiagnosSocial,
        };

        var aspSocio = await _contexto.Set<RRHH_AspSocio>()
            .Where(a => a.IdEmpleado == empleado.IdEmpleado).OrderBy(a => a.idAspSocio).FirstOrDefaultAsync();
        if (aspSocio is null)
        {
            if (valores.Values.All(v => string.IsNullOrWhiteSpace(v as string))) return;
            aspSocio = new RRHH_AspSocio { IdEmpleado = empleado.IdEmpleado };
            _contexto.Set<RRHH_AspSocio>().Add(aspSocio);
        }

        var entrada = _contexto.Entry(aspSocio);
        foreach (var (columna, valor) in valores)
            Asignar(entrada, columna, valor);
    }

    private async Task GuardarIngresosAsync(Empleado empleado)
    {
        var (tipoEstado, tipoInstitucion) = await TiposDeIngresoAsync();
        if (tipoEstado is not null)
            await GuardarIngresoAsync(empleado.IdEmpleado, tipoEstado.Value, empleado.NumResoIngreEsta,
                AFechaSolo(empleado.FechaResolIngreEsta), AFechaSolo(empleado.FechaIngreEstado));
        if (tipoInstitucion is not null)
            await GuardarIngresoAsync(empleado.IdEmpleado, tipoInstitucion.Value, empleado.NumResoIngreInstitu,
                AFechaSolo(empleado.FechaResoIngreInstitu), AFechaSolo(empleado.FechaIngreso));
    }

    private async Task GuardarIngresoAsync(int idEmpleado, int idTipo, string? numero, DateOnly? fecha, DateOnly? vigencia)
    {
        var resoluciones = _contexto.Set<Resolucion>();
        var actual = await resoluciones.Where(r => r.idEmpleado == idEmpleado && r.idTipoResolucion == idTipo)
            .OrderByDescending(r => r.idResolucion).FirstOrDefaultAsync();
        if (actual is null)
        {
            if (string.IsNullOrWhiteSpace(numero) && fecha is null) return;
            var ultimo = await resoluciones.Where(r => r.idEmpleado == idEmpleado).MaxAsync(r => (int?)r.idResolucion) ?? 0;
            actual = new Resolucion { idEmpleado = idEmpleado, idResolucion = ultimo + 1, idTipoResolucion = idTipo };
            resoluciones.Add(actual);
        }
        actual.NumeroResolucion = string.IsNullOrWhiteSpace(numero) ? null : numero.Trim();
        actual.FechaResolucion = fecha;
        Asignar(_contexto.Entry(actual), "FechaVigencia", vigencia);
    }

    // Los tipos se buscan por descripcion (sin tildes) para no depender del Id.
    private async Task<(int? Estado, int? Institucion)> TiposDeIngresoAsync()
    {
        var tipos = await _contexto.Set<TipoResolucion>().AsNoTracking().ToListAsync();
        int? Buscar(string descripcion) => tipos.FirstOrDefault(t =>
            string.Compare(t.DescripTipoResolucion?.Trim(), descripcion, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.CompareOptions.IgnoreCase | System.Globalization.CompareOptions.IgnoreNonSpace) == 0)?.idTipoResolucion;
        return (Buscar(IngresoAlEstado), Buscar(IngresoALaInstitucion));
    }

    private static DateTime? AFecha(DateOnly? fecha) => fecha?.ToDateTime(TimeOnly.MinValue);

    private static DateOnly? AFechaSolo(DateTime? fecha) => fecha is null ? null : DateOnly.FromDateTime(fecha.Value);
}
