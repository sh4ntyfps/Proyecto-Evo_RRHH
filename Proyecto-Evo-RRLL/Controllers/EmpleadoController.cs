using Capa_Logica;
using Proyecto_Evo_RRLL.Utilidades;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class EmpleadoController : Controller
{
    private readonly EmpleadoLogica _empleadoData;
    private readonly PersonaLogica _personaData;
    private readonly CargoLogica _cargoData;
    private readonly EstructOrganizLogica _estructOrganizData;
    private readonly LocalLogica _localData;
    private readonly HorarioLogica _horarioData;
    private readonly TipoTrabajadorLogica _tipoTrabajadorData;
    private readonly RegimenPensionLogica _regimenPensionData;
    private readonly TipoDocIDLogica _tipoDocData;
    private readonly AFPLogica _afpData;
    private readonly EstadoCivilLogica _estadoCivilData;
    private readonly TipoSangreLogica _tipoSangreData;
    private readonly DiscapacidadLogica _discapacidadData;
    private readonly ViveConLogica _viveConData;
    private readonly NacionalidadLogica _nacionalidadData;
    private readonly SecuenciaService _secuencia;

    public EmpleadoController(
        EmpleadoLogica empleadoData,
        PersonaLogica personaData,
        CargoLogica cargoData,
        EstructOrganizLogica estructuraOrganizData,
        LocalLogica localData,
        HorarioLogica horarioData,
        TipoTrabajadorLogica tipoTrabajadorData,
        RegimenPensionLogica regimenPensionData,
        TipoDocIDLogica tipoDocData,
        AFPLogica afpData,
        EstadoCivilLogica estadoCivilData,
        TipoSangreLogica tipoSangreData,
        DiscapacidadLogica discapacidadData,
        ViveConLogica viveConData,
        NacionalidadLogica nacionalidadData,
        SecuenciaService secuencia)
    {
        _empleadoData = empleadoData;
        _personaData = personaData;
        _cargoData = cargoData;
        _estructOrganizData = estructuraOrganizData;
        _localData = localData;
        _horarioData = horarioData;
        _tipoTrabajadorData = tipoTrabajadorData;
        _regimenPensionData = regimenPensionData;
        _tipoDocData = tipoDocData;
        _afpData = afpData;
        _estadoCivilData = estadoCivilData;
        _tipoSangreData = tipoSangreData;
        _discapacidadData = discapacidadData;
        _viveConData = viveConData;
        _nacionalidadData = nacionalidadData;
        _secuencia = secuencia;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? q, int pagina = 1)
    {
        const int tamanoPagina = 25;
        var (filtrados, personasPorId, cargosPorId, areasPorKey) = await FiltrarEmpleadosAsync(q);

        var total = filtrados.Count;
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)tamanoPagina));
        if (pagina < 1) pagina = 1;
        if (pagina > totalPaginas) pagina = totalPaginas;

        var vm = new EmpleadoViewModel { Busqueda = q, Pagina = pagina, TotalPaginas = totalPaginas, Total = total };

        foreach (var e in filtrados.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina))
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null
                ? null
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
            var cargo = e.IdCargo is null ? null : cargosPorId.GetValueOrDefault(e.IdCargo.Value)?.Descripcion;
            var area = areasPorKey.TryGetValue($"{e.Year}|{e.idAreaOrganiz}", out var areaRegistro) ? areaRegistro.AreaOrganizacional : null;

            vm.Empleados.Add(new EmpleadoFila
            {
                IdEmpleado = e.IdEmpleado,
                Nombres = nombre,
                NumDoc = persona?.NumDocID,
                Cargo = cargo,
                Area = area,
                Estado = e.Estado,
                FechaIngreso = e.FechaIngreso,
                TieneFoto = e.Foto is not null
            });
        }

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Exportar(string? q)
    {
        var (filtrados, personasPorId, cargosPorId, areasPorKey) = await FiltrarEmpleadosAsync(q);

        var filas = new List<string[]>
        {
            new[] { "IdEmpleado", "Nombres", "NumDoc", "Cargo", "Area", "Estado", "FechaIngreso" }
        };

        foreach (var e in filtrados)
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null ? null : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
            var cargo = e.IdCargo is null ? null : cargosPorId.GetValueOrDefault(e.IdCargo.Value)?.Descripcion;
            var area = areasPorKey.TryGetValue($"{e.Year}|{e.idAreaOrganiz}", out var areaRegistro) ? areaRegistro.AreaOrganizacional : null;

            filas.Add(new[]
            {
                e.IdEmpleado.ToString(),
                nombre ?? string.Empty,
                persona?.NumDocID ?? string.Empty,
                cargo ?? string.Empty,
                area ?? string.Empty,
                e.Estado ?? string.Empty,
                e.FechaIngreso?.ToString("dd/MM/yyyy") ?? string.Empty
            });
        }

        var contenido = string.Join("\r\n", filas.Select(f => string.Join(";", f.Select(CsvUtilidad.EscaparCsv))));
        var nombreArchivo = $"empleados_{(string.IsNullOrWhiteSpace(q) ? "todos" : "filtro")}_{DateTime.Now:yyyyMMdd_HHmm}.csv";
        return File(new System.Text.UTF8Encoding(true).GetBytes(contenido), "text/csv; charset=utf-8", nombreArchivo);
    }

    private async Task<(List<Empleado> Filtrados, Dictionary<int, Persona> Personas, Dictionary<int, Cargo> Cargos, Dictionary<string, EstructOrganiz> Areas)> FiltrarEmpleadosAsync(string? q)
    {
        var empleados = await _empleadoData.Listar();
        var personas = await _personaData.Listar();
        var cargos = await _cargoData.Listar();
        var areas = await _estructOrganizData.Listar();

        var personasPorId = personas.ToDictionary(p => p.IdPersona);
        var cargosPorId = cargos.ToDictionary(c => c.IdCargo);
        var areasPorKey = areas.ToDictionary(a => $"{a.Year}|{a.idAreaOrganiz}");

        if (string.IsNullOrWhiteSpace(q))
            return (empleados.OrderBy(e => e.IdEmpleado).ToList(), personasPorId, cargosPorId, areasPorKey);

        var filtrados = new List<Empleado>();
        foreach (var e in empleados)
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null ? null : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
            if (nombre?.Contains(q, StringComparison.OrdinalIgnoreCase) == true
                || persona?.NumDocID?.Contains(q, StringComparison.OrdinalIgnoreCase) == true)
                filtrados.Add(e);
        }

        return (filtrados.OrderBy(e => e.IdEmpleado).ToList(), personasPorId, cargosPorId, areasPorKey);
    }


    [HttpGet]
    public async Task<IActionResult> Detalles(int id)
    {
        var empleado = await _empleadoData.Obtener(id);
        if (empleado is null)
            return NotFound();

        var personas = await _personaData.Listar();
        var persona = empleado.IdPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == empleado.IdPersona);

        var vm = new EmpleadoDetalleViewModel
        {
            IdEmpleado = empleado.IdEmpleado,
            Empleado = empleado,
            Persona = persona,
            NombresCompletos = persona is null
                ? null
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim(),
            TipoDoc = persona?.TipoDocID is null ? null : (await _tipoDocData.Listar()).FirstOrDefault(t => t.Nombre == persona.TipoDocID)?.Descripcion,
            Cargo = empleado.IdCargo is null ? null : (await _cargoData.Listar()).FirstOrDefault(c => c.IdCargo == empleado.IdCargo)?.Descripcion,
            Area = (await _estructOrganizData.Listar())
                .FirstOrDefault(a => a.Year == empleado.Year && a.idAreaOrganiz == empleado.idAreaOrganiz)?.AreaOrganizacional,
            Local = empleado.idLocal is null ? null : (await _localData.Listar()).FirstOrDefault(l => l.idLocal == empleado.idLocal)?.NombreLocal,
            Horario = empleado.idHorario is null ? null : (await _horarioData.Listar()).FirstOrDefault(h => h.idHorario == empleado.idHorario)?.Descrip_Horario,
            Estado = empleado.Estado,
            AFP = empleado.IdAFP is null ? null : (await _afpData.Listar()).FirstOrDefault(a => a.IdAFP == empleado.IdAFP)?.DescripAFP,
            EstadoCivil = empleado.IdEstadoCivil is null ? null : (await _estadoCivilData.Listar()).FirstOrDefault(ec => ec.IdEstadoCivil == empleado.IdEstadoCivil)?.DescripEstCivil,
            TipoSangre = empleado.IdTipoSangre is null ? null : (await _tipoSangreData.Listar()).FirstOrDefault(s => s.IdTipoSangre == empleado.IdTipoSangre)?.DescripTS,
            Discapacidad = empleado.IdDiscapacidad is null ? null : (await _discapacidadData.Listar()).FirstOrDefault(d => d.IdDiscapacidad == empleado.IdDiscapacidad)?.Descrip_Discapacidad,
            ViveCon = empleado.IdViveCon is null ? null : (await _viveConData.Listar()).FirstOrDefault(v => v.IdViveCon == empleado.IdViveCon)?.DescripViveCon,
            Nacionalidad = empleado.IdNacionalidad is null ? null : (await _nacionalidadData.Listar()).FirstOrDefault(n => n.IdNacionalidad == empleado.IdNacionalidad)?.Descripcion,
            TipoTrabajador = empleado.IdTipoTrabajador is null ? null : (await _tipoTrabajadorData.Listar()).FirstOrDefault(t => t.IdTipoTrabajador == empleado.IdTipoTrabajador)?.Descripcion,
            RegimenPension = empleado.IdRegimenPen is null ? null : (await _regimenPensionData.Listar()).FirstOrDefault(r => r.IdRegimenPen == empleado.IdRegimenPen)?.LeyRegimen,
            TieneFoto = empleado.Foto is not null
        };
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var empleado = await _empleadoData.Obtener(id);
        if (empleado is null)
            return NotFound();

        await CargarCatalogosEdicion();
        return View(empleado);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Empleado empleado, IFormFile? foto)
    {
        if (id != empleado.IdEmpleado)
            return NotFound();

        var original = await _empleadoData.Obtener(id);
        if (original is null)
            return NotFound();

        original.Year = empleado.Year;
        original.idAreaOrganiz = empleado.idAreaOrganiz;
        original.ExpSocial = empleado.ExpSocial;
        original.FechaIngreso = empleado.FechaIngreso;
        original.IdAFP = empleado.IdAFP;
        original.Brevete = empleado.Brevete;
        original.IdEstadoCivil = empleado.IdEstadoCivil;
        original.Sexo = empleado.Sexo;
        original.CtaCorriente = empleado.CtaCorriente;
        original.NroAFP = empleado.NroAFP;
        original.FechaAFP = empleado.FechaAFP;
        original.IdSeguro = empleado.IdSeguro;
        original.NroSeguro = empleado.NroSeguro;
        original.IdTipoSangre = empleado.IdTipoSangre;
        original.DecLey = empleado.DecLey;
        original.IdTipoTrabajador = empleado.IdTipoTrabajador;
        original.Estado = empleado.Estado;
        original.IdCargo = empleado.IdCargo;
        original.IdDiscapacidad = empleado.IdDiscapacidad;
        original.Profesion = empleado.Profesion;
        original.JefeHogar = empleado.JefeHogar;
        original.CargaFam = empleado.CargaFam;
        original.NumHijos = empleado.NumHijos;
        original.IdViveCon = empleado.IdViveCon;
        original.ObsFam = empleado.ObsFam;
        original.AspecSalud = empleado.AspecSalud;
        original.AspecEducacion = empleado.AspecEducacion;
        original.AspecRecreacion = empleado.AspecRecreacion;
        original.AspecPsico = empleado.AspecPsico;
        original.AspecSocial = empleado.AspecSocial;
        original.DiagnosSocial = empleado.DiagnosSocial;
        original.IdNacionalidad = empleado.IdNacionalidad;
        original.NumLibretaMilitar = empleado.NumLibretaMilitar;
        original.GradoInstruccion = empleado.GradoInstruccion;
        original.IdRegimenPen = empleado.IdRegimenPen;
        original.NumRUC = empleado.NumRUC;
        original.NumAutogenSalud = empleado.NumAutogenSalud;
        original.NumResolCese = empleado.NumResolCese;
        original.FechaResolCese = empleado.FechaResolCese;
        original.NumResol20530 = empleado.NumResol20530;
        original.FechaResol20530 = empleado.FechaResol20530;
        original.NumResolPenMen = empleado.NumResolPenMen;
        original.FechaResolPenMen = empleado.FechaResolPenMen;
        original.NumResoIngreEsta = empleado.NumResoIngreEsta;
        original.FechaIngreEstado = empleado.FechaIngreEstado;
        original.FechaResolIngreEsta = empleado.FechaResolIngreEsta;
        original.NumResoIngreInstitu = empleado.NumResoIngreInstitu;
        original.FechaResoIngreInstitu = empleado.FechaResoIngreInstitu;
        original.idHorario = empleado.idHorario;
        original.idLocal = empleado.idLocal;
        original.Foto = foto is not null && foto.Length > 0 ? await LeerArchivo(foto) : original.Foto;

        await _empleadoData.Actualizar(original);

        await CargarCatalogosEdicion();
        return RedirectToAction(nameof(Detalles), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo()
    {
        await CargarCatalogosEdicion();
        return View(new EmpleadoNuevoViewModel
        {
            Empleado = new Empleado
            {
                Year = DateTime.Now.Year,
                Estado = "A",
                FechaIngreso = DateTime.Today
            }
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(EmpleadoNuevoViewModel modelo, IFormFile? foto)
    {
        var persona = modelo.Persona;
        if (string.IsNullOrWhiteSpace(persona.Nombres))
            ModelState.AddModelError(nameof(modelo.Persona.Nombres), "El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(persona.Apellido_Paterno))
            ModelState.AddModelError(nameof(modelo.Persona.Apellido_Paterno), "El apellido paterno es obligatorio.");
        if (persona.TipoDocID is not null && string.IsNullOrWhiteSpace(persona.NumDocID))
            ModelState.AddModelError(nameof(modelo.Persona.NumDocID), "Ingrese el número del documento.");

        if (!string.IsNullOrWhiteSpace(persona.NumDocID))
        {
            var personas = await _personaData.Listar();
            if (personas.Any(p => p.NumDocID == persona.NumDocID))
                ModelState.AddModelError(nameof(modelo.Persona.NumDocID), "Ya existe una persona con ese número de documento.");
        }

        if (ModelState.IsValid)
        {
            Empleado? empleado = null;
            var guardo = await _secuencia.EjecutarTransaccionalAsync(async () =>
            {
                var personas = await _personaData.Listar();
                persona.IdPersona = personas.Count > 0 ? personas.Max(p => p.IdPersona) + 1 : 1;
                persona.FechaRegistro = DateTime.Now;
                await _personaData.Crear(persona);

                var empleados = await _empleadoData.Listar();
                empleado = modelo.Empleado;
                empleado.IdEmpleado = empleados.Count > 0 ? empleados.Max(e => e.IdEmpleado) + 1 : 1;
                empleado.IdPersona = persona.IdPersona;
                empleado.Foto = foto is not null && foto.Length > 0 ? await LeerArchivo(foto) : null;
                if (string.IsNullOrWhiteSpace(empleado.Estado))
                    empleado.Estado = "A";
                await _empleadoData.Crear(empleado);
            });
            if (guardo)
            {
                TempData["MensajeExito"] = "Empleado registrado.";
                return RedirectToAction(nameof(Detalles), new { id = empleado!.IdEmpleado });
            }
            ModelState.AddModelError("", "No se pudo registrar el empleado: intente nuevamente.");
        }

        await CargarCatalogosEdicion();
        return View(modelo);
    }

    private async Task CargarCatalogosEdicion()
    {
        ViewBag.Cargos = await _cargoData.Listar();
        ViewBag.Horarios = await _horarioData.Listar();
        ViewBag.Locales = await _localData.Listar();
        ViewBag.Areas = await _estructOrganizData.Listar();
        ViewBag.TiposTrabajador = await _tipoTrabajadorData.Listar();
        ViewBag.Regimenes = await _regimenPensionData.Listar();
        ViewBag.Afps = await _afpData.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilData.Listar();
        ViewBag.TiposSangre = await _tipoSangreData.Listar();
        ViewBag.Discapacidades = await _discapacidadData.Listar();
        ViewBag.ViveCon = await _viveConData.Listar();
        ViewBag.Nacionalidades = await _nacionalidadData.Listar();
        ViewBag.TiposDocumento = await _tipoDocData.Listar();
    }

    [HttpGet]
    public async Task<IActionResult> MostrarFoto(int id)
    {
        var empleado = await _empleadoData.Obtener(id);
        if (empleado?.Foto is null)
            return NotFound();
        return File(empleado.Foto, "image/jpeg");
    }

    private static async Task<byte[]> LeerArchivo(IFormFile archivo)
    {
        using var ms = new MemoryStream();
        await archivo.CopyToAsync(ms);
        return ms.ToArray();
    }
}