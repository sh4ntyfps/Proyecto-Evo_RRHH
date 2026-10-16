using Capa_Logica;
using Capa_Entidades.Constantes;
using Proyecto_Evo_RRLL.Utilidades;
using Capa_Logica.Exportacion;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class EmpleadoController : Controller
{
    private readonly EmpleadoLogica _empleadoLogica;
    private readonly PersonaLogica _personaLogica;
    private readonly CargoLogica _cargoLogica;
    private readonly EstructOrganizLogica _estructOrganizLogica;
    private readonly LocalLogica _localLogica;
    private readonly HorarioLogica _horarioLogica;
    private readonly TipoTrabajadorLogica _tipoTrabajadorLogica;
    private readonly RegimenPensionLogica _regimenPensionLogica;
    private readonly TipoDocIDLogica _tipoDocLogica;
    private readonly AFPLogica _afpLogica;
    private readonly EstadoCivilLogica _estadoCivilLogica;
    private readonly TipoSangreLogica _tipoSangreLogica;
    private readonly DiscapacidadLogica _discapacidadLogica;
    private readonly ViveConLogica _viveConLogica;
    private readonly NacionalidadLogica _nacionalidadLogica;
    private readonly ExportadorTablas _exportador;

    public EmpleadoController(
        EmpleadoLogica empleadoLogica,
        PersonaLogica personaLogica,
        CargoLogica cargoLogica,
        EstructOrganizLogica estructuraOrganizLogica,
        LocalLogica localLogica,
        HorarioLogica horarioLogica,
        TipoTrabajadorLogica tipoTrabajadorLogica,
        RegimenPensionLogica regimenPensionLogica,
        TipoDocIDLogica tipoDocLogica,
        AFPLogica afpLogica,
        EstadoCivilLogica estadoCivilLogica,
        TipoSangreLogica tipoSangreLogica,
        DiscapacidadLogica discapacidadLogica,
        ViveConLogica viveConLogica,
        NacionalidadLogica nacionalidadLogica,
        ExportadorTablas exportador)
    {
        _empleadoLogica = empleadoLogica;
        _personaLogica = personaLogica;
        _cargoLogica = cargoLogica;
        _estructOrganizLogica = estructuraOrganizLogica;
        _localLogica = localLogica;
        _horarioLogica = horarioLogica;
        _tipoTrabajadorLogica = tipoTrabajadorLogica;
        _regimenPensionLogica = regimenPensionLogica;
        _tipoDocLogica = tipoDocLogica;
        _afpLogica = afpLogica;
        _estadoCivilLogica = estadoCivilLogica;
        _tipoSangreLogica = tipoSangreLogica;
        _discapacidadLogica = discapacidadLogica;
        _viveConLogica = viveConLogica;
        _nacionalidadLogica = nacionalidadLogica;
        _exportador = exportador;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? q, int pagina = 1)
    {
        var (filtrados, personasPorId, cargosPorId, areasPorKey) = await FiltrarEmpleadosAsync(q);

        var total = filtrados.Count;
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)Paginacion.TamanoPagina));
        if (pagina < 1) pagina = 1;
        if (pagina > totalPaginas) pagina = totalPaginas;

        var vm = new EmpleadoViewModel { Busqueda = q, Pagina = pagina, TotalPaginas = totalPaginas, Total = total };

        foreach (var e in filtrados.Skip((pagina - 1) * Paginacion.TamanoPagina).Take(Paginacion.TamanoPagina))
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null
                ? null
                : persona.NombreCompleto();
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
    public async Task<IActionResult> Exportar(string? q, string formato = "csv")
    {
        if (!_exportador.Soporta(formato))
            return NotFound();

        var (filtrados, personasPorId, cargosPorId, areasPorKey) = await FiltrarEmpleadosAsync(q);

        var filas = new List<string[]>
        {
            new[] { "IdEmpleado", "Nombres", "NumDoc", "Cargo", "Area", "Estado", "FechaIngreso" }
        };

        foreach (var e in filtrados)
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null ? null : persona.NombreCompleto();
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

        var archivo = _exportador.Exportar(formato, filas);
        var nombreArchivo = $"empleados_{(string.IsNullOrWhiteSpace(q) ? "todos" : "filtro")}_{DateTime.Now:yyyyMMdd_HHmm}.{archivo.Extension}";
        return File(archivo.Contenido, archivo.TipoContenido, nombreArchivo);
    }

    private async Task<(List<Empleado> Filtrados, Dictionary<int, Persona> Personas, Dictionary<int, Cargo> Cargos, Dictionary<string, EstructOrganiz> Areas)> FiltrarEmpleadosAsync(string? q)
    {
        var empleados = await _empleadoLogica.Listar();
        var personas = await _personaLogica.Listar();
        var cargos = await _cargoLogica.Listar();
        var areas = await _estructOrganizLogica.Listar();

        var personasPorId = personas.ToDictionary(p => p.IdPersona);
        var cargosPorId = cargos.ToDictionary(c => c.IdCargo);
        var areasPorKey = areas.ToDictionary(a => $"{a.Year}|{a.idAreaOrganiz}");

        if (string.IsNullOrWhiteSpace(q))
            return (empleados.OrderBy(e => e.IdEmpleado).ToList(), personasPorId, cargosPorId, areasPorKey);

        var filtrados = new List<Empleado>();
        foreach (var e in empleados)
        {
            var persona = e.IdPersona is null ? null : personasPorId.GetValueOrDefault(e.IdPersona.Value);
            var nombre = persona is null ? null : persona.NombreCompleto();
            if (nombre?.Contains(q, StringComparison.OrdinalIgnoreCase) == true
                || persona?.NumDocID?.Contains(q, StringComparison.OrdinalIgnoreCase) == true)
                filtrados.Add(e);
        }

        return (filtrados.OrderBy(e => e.IdEmpleado).ToList(), personasPorId, cargosPorId, areasPorKey);
    }


    [HttpGet]
    public async Task<IActionResult> Detalles(int id)
    {
        var empleado = await _empleadoLogica.Obtener(id);
        if (empleado is null)
            return NotFound();

        var personas = await _personaLogica.Listar();
        var persona = empleado.IdPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == empleado.IdPersona);

        var vm = new EmpleadoDetalleViewModel
        {
            IdEmpleado = empleado.IdEmpleado,
            Empleado = empleado,
            Persona = persona,
            NombresCompletos = persona is null
                ? null
                : persona.NombreCompleto(),
            TipoDoc = persona?.TipoDocID is null ? null : (await _tipoDocLogica.Listar()).FirstOrDefault(t => t.Nombre == persona.TipoDocID)?.Descripcion,
            Cargo = empleado.IdCargo is null ? null : (await _cargoLogica.Listar()).FirstOrDefault(c => c.IdCargo == empleado.IdCargo)?.Descripcion,
            Area = (await _estructOrganizLogica.Listar())
                .FirstOrDefault(a => a.Year == empleado.Year && a.idAreaOrganiz == empleado.idAreaOrganiz)?.AreaOrganizacional,
            Local = empleado.idLocal is null ? null : (await _localLogica.Listar()).FirstOrDefault(l => l.idLocal == empleado.idLocal)?.NombreLocal,
            Horario = empleado.idHorario is null ? null : (await _horarioLogica.Listar()).FirstOrDefault(h => h.idHorario == empleado.idHorario)?.Descrip_Horario,
            Estado = empleado.Estado,
            AFP = empleado.IdAFP is null ? null : (await _afpLogica.Listar()).FirstOrDefault(a => a.IdAFP == empleado.IdAFP)?.DescripAFP,
            EstadoCivil = empleado.IdEstadoCivil is null ? null : (await _estadoCivilLogica.Listar()).FirstOrDefault(ec => ec.IdEstadoCivil == empleado.IdEstadoCivil)?.DescripEstCivil,
            TipoSangre = empleado.IdTipoSangre is null ? null : (await _tipoSangreLogica.Listar()).FirstOrDefault(s => s.IdTipoSangre == empleado.IdTipoSangre)?.DescripTS,
            Discapacidad = empleado.IdDiscapacidad is null ? null : (await _discapacidadLogica.Listar()).FirstOrDefault(d => d.IdDiscapacidad == empleado.IdDiscapacidad)?.Descrip_Discapacidad,
            ViveCon = empleado.IdViveCon is null ? null : (await _viveConLogica.Listar()).FirstOrDefault(v => v.IdViveCon == empleado.IdViveCon)?.DescripViveCon,
            Nacionalidad = empleado.IdNacionalidad is null ? null : (await _nacionalidadLogica.Listar()).FirstOrDefault(n => n.IdNacionalidad == empleado.IdNacionalidad)?.Descripcion,
            TipoTrabajador = empleado.IdTipoTrabajador is null ? null : (await _tipoTrabajadorLogica.Listar()).FirstOrDefault(t => t.IdTipoTrabajador == empleado.IdTipoTrabajador)?.Descripcion,
            RegimenPension = empleado.IdRegimenPen is null ? null : (await _regimenPensionLogica.Listar()).FirstOrDefault(r => r.IdRegimenPen == empleado.IdRegimenPen)?.LeyRegimen,
            TieneFoto = empleado.Foto is not null
        };
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var empleado = await _empleadoLogica.Obtener(id);
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

        var original = await _empleadoLogica.Obtener(id);
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

        await _empleadoLogica.Actualizar(original);

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
                Estado = EstadosEmpleado.Activo,
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
            var personas = await _personaLogica.Listar();
            if (personas.Any(p => p.NumDocID == persona.NumDocID))
                ModelState.AddModelError(nameof(modelo.Persona.NumDocID), "Ya existe una persona con ese número de documento.");
        }

        if (ModelState.IsValid)
        {
            var empleado = modelo.Empleado;
            empleado.Foto = foto is not null && foto.Length > 0 ? await LeerArchivo(foto) : null;
            var guardo = await _empleadoLogica.RegistrarConPersona(persona, empleado);
            if (guardo)
            {
                TempData["MensajeExito"] = "Empleado registrado.";
                return RedirectToAction(nameof(Detalles), new { id = empleado.IdEmpleado });
            }
            ModelState.AddModelError("", "No se pudo registrar el empleado: intente nuevamente.");
        }

        await CargarCatalogosEdicion();
        return View(modelo);
    }

    private async Task CargarCatalogosEdicion()
    {
        ViewBag.Cargos = await _cargoLogica.Listar();
        ViewBag.Horarios = await _horarioLogica.Listar();
        ViewBag.Locales = await _localLogica.Listar();
        ViewBag.Areas = await _estructOrganizLogica.Listar();
        ViewBag.TiposTrabajador = await _tipoTrabajadorLogica.Listar();
        ViewBag.Regimenes = await _regimenPensionLogica.Listar();
        ViewBag.Afps = await _afpLogica.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilLogica.Listar();
        ViewBag.TiposSangre = await _tipoSangreLogica.Listar();
        ViewBag.Discapacidades = await _discapacidadLogica.Listar();
        ViewBag.ViveCon = await _viveConLogica.Listar();
        ViewBag.Nacionalidades = await _nacionalidadLogica.Listar();
        ViewBag.TiposDocumento = await _tipoDocLogica.Listar();
    }

    [HttpGet]
    public async Task<IActionResult> MostrarFoto(int id)
    {
        var empleado = await _empleadoLogica.Obtener(id);
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