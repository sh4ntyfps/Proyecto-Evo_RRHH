using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class EstructuraOrganizController : Controller
{
    private readonly EstructOrganizData _areaData;
    private readonly ResponsableXUOData _responsableData;
    private readonly Empleado_AreaData _empleadoAreaData;
    private readonly LocalData _localData;
    private readonly EmpleadoData _empleadoData;
    private readonly PersonaData _personaData;
    private readonly SecuenciaService _secuencia;

    public EstructuraOrganizController(
        EstructOrganizData areaData,
        ResponsableXUOData responsableData,
        Empleado_AreaData empleadoAreaData,
        LocalData localData,
        EmpleadoData empleadoData,
        PersonaData personaData,
        SecuenciaService secuencia)
    {
        _areaData = areaData;
        _responsableData = responsableData;
        _empleadoAreaData = empleadoAreaData;
        _localData = localData;
        _empleadoData = empleadoData;
        _personaData = personaData;
        _secuencia = secuencia;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? year)
    {
        var areas = await _areaData.Listar();
        var anios = areas.Select(a => a.Year).Distinct().OrderByDescending(x => x).ToList();
        var anio = year ?? anios.FirstOrDefault();
        var empleados = await EmpleadosAsync();

        var lista = areas
            .Where(a => a.Year == anio)
            .OrderBy(a => a.idAreaOrganiz)
            .Select(a => new AreaOrganizFila
            {
                Year = a.Year,
                IdAreaOrganiz = a.idAreaOrganiz,
                AreaOrganizacional = a.AreaOrganizacional,
                Abrev = a.Abrev,
                Sigla = a.Sigla,
                DependeDe = areas.FirstOrDefault(d => d.Year == a.YearDe && d.idAreaOrganiz == a.idDependeDe)?.AreaOrganizacional,
                Jefe = a.IdEmpleado is null ? null : empleados.FirstOrDefault(e => e.Id == a.IdEmpleado).Nombre,
                NroTrabajaUO = a.NroTrabajaUO,
                PresupAnual = a.PresupAnual
            })
            .ToList();

        ViewBag.Anios = anios;
        ViewBag.Anio = anio;
        return View(lista);
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo(int year)
    {
        await CargarCatalogos(year);
        return View("Form", new EstructOrganiz { Year = year });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(EstructOrganiz modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.AreaOrganizacional))
            ModelState.AddModelError(nameof(modelo.AreaOrganizacional), "El nombre del área es obligatorio.");

        if (ModelState.IsValid)
        {
            var guardo = await _secuencia.EjecutarConBloqueoAsync(
                async () =>
                {
                    var porAnio = (await _areaData.Listar()).Where(a => a.Year == modelo.Year).ToList();
                    return porAnio.Count > 0 ? porAnio.Max(a => a.idAreaOrganiz) + 1 : 1;
                },
                async id =>
                {
                    modelo.idAreaOrganiz = id;
                    await _areaData.Crear(modelo);
                    return true;
                });
            if (guardo)
            {
                TempData["MensajeExito"] = "Unidad orgánica registrada.";
                return RedirectToAction(nameof(Index), new { year = modelo.Year });
            }
            ModelState.AddModelError("", "No se pudo registrar la unidad orgánica: intente nuevamente.");
        }

        await CargarCatalogos(modelo.Year);
        return View("Form", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int year, int idAreaOrganiz)
    {
        var area = await _areaData.Obtener(year, idAreaOrganiz);
        if (area is null)
            return NotFound();

        await CargarCatalogos(year);
        return View("Form", area);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(EstructOrganiz modelo)
    {
        var actual = await _areaData.Obtener(modelo.Year, modelo.idAreaOrganiz);
        if (actual is null)
            return NotFound();

        actual.AreaOrganizacional = modelo.AreaOrganizacional;
        actual.Abrev = modelo.Abrev;
        actual.Sigla = modelo.Sigla;
        actual.PresupAnual = modelo.PresupAnual;
        actual.PromedMensSueld = modelo.PromedMensSueld;
        actual.NroTrabajaUO = modelo.NroTrabajaUO;
        actual.Mision = modelo.Mision;
        actual.Vision = modelo.Vision;
        actual.IdPresupuesto = modelo.IdPresupuesto;
        actual.YearDe = modelo.YearDe;
        actual.idDependeDe = modelo.idDependeDe;
        actual.IdEmpleado = modelo.IdEmpleado;
        actual.InversPublic = modelo.InversPublic;
        actual.ApruebaPedido = modelo.ApruebaPedido;
        actual.MontoAprobacion = modelo.MontoAprobacion;
        actual.PresupCompra = modelo.PresupCompra;
        actual.PresupUtilizado = modelo.PresupUtilizado;
        await _areaData.Actualizar(actual);

        TempData["MensajeExito"] = "Unidad orgánica actualizada.";
        return RedirectToAction(nameof(Index), new { year = modelo.Year });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int year, int idAreaOrganiz)
    {
        var actual = await _areaData.Obtener(year, idAreaOrganiz);
        if (actual is not null)
            await _areaData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { year });
    }

    [HttpGet]
    public async Task<IActionResult> Detalles(int year, int idAreaOrganiz)
    {
        var area = await _areaData.Obtener(year, idAreaOrganiz);
        if (area is null)
            return NotFound();

        var areas = await _areaData.Listar();
        var empleados = await EmpleadosAsync();
        var asignaciones = (await _empleadoAreaData.Listar())
            .Where(ea => ea.Year == year && ea.idAreaOrganiz == idAreaOrganiz)
            .ToList();
        var responsables = (await _responsableData.Listar())
            .Where(r => r.Year == year && r.idAreaOrganiz == idAreaOrganiz)
            .ToList();

        ViewBag.Area = area;
        ViewBag.DependeDe = areas.FirstOrDefault(d => d.Year == area.YearDe && d.idAreaOrganiz == area.idDependeDe)?.AreaOrganizacional;
        ViewBag.Jefe = area.IdEmpleado is null ? null : empleados.FirstOrDefault(e => e.Id == area.IdEmpleado).Nombre;
        ViewBag.Empleados = empleados;
        ViewBag.Asignaciones = asignaciones;
        ViewBag.Responsables = responsables;
        ViewBag.ResponsableData = _responsableData;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarEmpleado(int year, int idAreaOrganiz, int idEmpleado)
    {
        if (await _areaData.Obtener(year, idAreaOrganiz) is null)
            return NotFound();

        var existente = await _empleadoAreaData.Obtener(year, idAreaOrganiz, idEmpleado);
        if (existente is null)
            await _empleadoAreaData.Crear(new Empleado_Area { Year = year, idAreaOrganiz = idAreaOrganiz, IdEmpleado = idEmpleado });

        return RedirectToAction(nameof(Detalles), new { year, idAreaOrganiz });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuitarEmpleado(int year, int idAreaOrganiz, int idEmpleado)
    {
        var existente = await _empleadoAreaData.Obtener(year, idAreaOrganiz, idEmpleado);
        if (existente is not null)
            await _empleadoAreaData.Eliminar(existente);
        return RedirectToAction(nameof(Detalles), new { year, idAreaOrganiz });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResponsableNuevo(ResponsableXUO modelo)
    {
        var guardo = await _secuencia.EjecutarConBloqueoAsync(
            async () =>
            {
                var lista = await _responsableData.Listar();
                return lista.Count > 0 ? lista.Max(r => r.IdResponsable) + 1 : 1;
            },
            async id =>
            {
                modelo.IdResponsable = id;
                modelo.AreaOrganizacional = (await _areaData.Obtener(modelo.Year ?? 0, modelo.idAreaOrganiz ?? 0))?.AreaOrganizacional;
                await _responsableData.Crear(modelo);
                return true;
            });
        if (guardo)
        {
            TempData["MensajeExito"] = "Responsable asignado.";
            return RedirectToAction(nameof(Detalles), new { year = modelo.Year, idAreaOrganiz = modelo.idAreaOrganiz });
        }
        return RedirectToAction(nameof(Detalles), new { year = modelo.Year, idAreaOrganiz = modelo.idAreaOrganiz });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResponsableEliminar(int idResponsable)
    {
        var actual = await _responsableData.Obtener(idResponsable);
        if (actual is not null)
            await _responsableData.Eliminar(actual);
        return RedirectToAction(nameof(Detalles), new { year = actual?.Year, idAreaOrganiz = actual?.idAreaOrganiz });
    }

    [HttpGet]
    public async Task<IActionResult> Locales()
    {
        return View(await _localData.Listar());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LocalNuevo(Local modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.NombreLocal))
            ModelState.AddModelError(nameof(modelo.NombreLocal), "El nombre del local es obligatorio.");

        if (ModelState.IsValid)
        {
            var guardo = await _secuencia.EjecutarConBloqueoAsync(
                async () =>
                {
                    var lista = await _localData.Listar();
                    return lista.Count > 0 ? lista.Max(l => l.idLocal) + 1 : 1;
                },
                async id =>
                {
                    modelo.idLocal = id;
                    await _localData.Crear(modelo);
                    return true;
                });
            if (!guardo)
                ModelState.AddModelError(nameof(modelo.NombreLocal), "No se pudo registrar el local: intente nuevamente.");
        }
        return RedirectToAction(nameof(Locales));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LocalEliminar(int idLocal)
    {
        var actual = await _localData.Obtener(idLocal);
        if (actual is not null)
            await _localData.Eliminar(actual);
        return RedirectToAction(nameof(Locales));
    }

    // ---------- Utilidades ----------

    private async Task CargarCatalogos(int year)
    {
        var areas = await _areaData.Listar();
        ViewBag.Year = year;
        ViewBag.Areas = areas.Where(a => a.Year == year).OrderBy(a => a.AreaOrganizacional).ToList();
        ViewBag.Empleados = await EmpleadosAsync();
    }

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
    {
        var empleados = await _empleadoData.Listar();
        var personas = await _personaData.Listar();
        return empleados.Select(e =>
        {
            var persona = e.IdPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == e.IdPersona);
            var nombre = persona is null
                ? $"(Empleado {e.IdEmpleado})"
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
            return (e.IdEmpleado, nombre);
        }).OrderBy(x => x.nombre).ToList();
    }
}