using Capa_Logica;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class EstructuraOrganizController : Controller
{
    private readonly EstructOrganizLogica _areaLogica;
    private readonly ResponsableXUOLogica _responsableLogica;
    private readonly Empleado_AreaLogica _empleadoAreaLogica;
    private readonly LocalLogica _localLogica;
    private readonly EmpleadoServicio _empleados;

    public EstructuraOrganizController(
        EstructOrganizLogica areaLogica,
        ResponsableXUOLogica responsableLogica,
        Empleado_AreaLogica empleadoAreaLogica,
        LocalLogica localLogica,
        EmpleadoServicio empleados)
    {
        _areaLogica = areaLogica;
        _responsableLogica = responsableLogica;
        _empleadoAreaLogica = empleadoAreaLogica;
        _localLogica = localLogica;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? year)
    {
        var areas = await _areaLogica.Listar();
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
            var guardo = await _areaLogica.Registrar(modelo);
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
        var area = await _areaLogica.Obtener(year, idAreaOrganiz);
        if (area is null)
            return NotFound();

        await CargarCatalogos(year);
        return View("Form", area);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(EstructOrganiz modelo)
    {
        var actual = await _areaLogica.Obtener(modelo.Year, modelo.idAreaOrganiz);
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
        await _areaLogica.Actualizar(actual);

        TempData["MensajeExito"] = "Unidad orgánica actualizada.";
        return RedirectToAction(nameof(Index), new { year = modelo.Year });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int year, int idAreaOrganiz)
    {
        var actual = await _areaLogica.Obtener(year, idAreaOrganiz);
        if (actual is not null)
            await _areaLogica.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { year });
    }

    [HttpGet]
    public async Task<IActionResult> Detalles(int year, int idAreaOrganiz)
    {
        var area = await _areaLogica.Obtener(year, idAreaOrganiz);
        if (area is null)
            return NotFound();

        var areas = await _areaLogica.Listar();
        var empleados = await EmpleadosAsync();
        var asignaciones = (await _empleadoAreaLogica.Listar())
            .Where(ea => ea.Year == year && ea.idAreaOrganiz == idAreaOrganiz)
            .ToList();
        var responsables = (await _responsableLogica.Listar())
            .Where(r => r.Year == year && r.idAreaOrganiz == idAreaOrganiz)
            .ToList();

        ViewBag.Area = area;
        ViewBag.DependeDe = areas.FirstOrDefault(d => d.Year == area.YearDe && d.idAreaOrganiz == area.idDependeDe)?.AreaOrganizacional;
        ViewBag.Jefe = area.IdEmpleado is null ? null : empleados.FirstOrDefault(e => e.Id == area.IdEmpleado).Nombre;
        ViewBag.Empleados = empleados;
        ViewBag.Asignaciones = asignaciones;
        ViewBag.Responsables = responsables;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarEmpleado(int year, int idAreaOrganiz, int idEmpleado)
    {
        if (await _areaLogica.Obtener(year, idAreaOrganiz) is null)
            return NotFound();

        var existente = await _empleadoAreaLogica.Obtener(year, idAreaOrganiz, idEmpleado);
        if (existente is null)
            await _empleadoAreaLogica.Crear(new Empleado_Area { Year = year, idAreaOrganiz = idAreaOrganiz, IdEmpleado = idEmpleado });

        return RedirectToAction(nameof(Detalles), new { year, idAreaOrganiz });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuitarEmpleado(int year, int idAreaOrganiz, int idEmpleado)
    {
        var existente = await _empleadoAreaLogica.Obtener(year, idAreaOrganiz, idEmpleado);
        if (existente is not null)
            await _empleadoAreaLogica.Eliminar(existente);
        return RedirectToAction(nameof(Detalles), new { year, idAreaOrganiz });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResponsableNuevo(ResponsableXUO modelo)
    {
        modelo.AreaOrganizacional = (await _areaLogica.Obtener(modelo.Year ?? 0, modelo.idAreaOrganiz ?? 0))?.AreaOrganizacional;
        var guardo = await _responsableLogica.Registrar(modelo);
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
        var actual = await _responsableLogica.Obtener(idResponsable);
        if (actual is not null)
            await _responsableLogica.Eliminar(actual);
        return RedirectToAction(nameof(Detalles), new { year = actual?.Year, idAreaOrganiz = actual?.idAreaOrganiz });
    }

    [HttpGet]
    public async Task<IActionResult> Locales()
    {
        return View(await _localLogica.Listar());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LocalNuevo(Local modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo.NombreLocal))
            ModelState.AddModelError(nameof(modelo.NombreLocal), "El nombre del local es obligatorio.");

        if (ModelState.IsValid)
        {
            var guardo = await _localLogica.Registrar(modelo);
            if (!guardo)
                ModelState.AddModelError(nameof(modelo.NombreLocal), "No se pudo registrar el local: intente nuevamente.");
        }
        return RedirectToAction(nameof(Locales));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LocalEliminar(int idLocal)
    {
        var actual = await _localLogica.Obtener(idLocal);
        if (actual is not null)
            await _localLogica.Eliminar(actual);
        return RedirectToAction(nameof(Locales));
    }

    // ---------- Utilidades ----------

    private async Task CargarCatalogos(int year)
    {
        var areas = await _areaLogica.Listar();
        ViewBag.Year = year;
        ViewBag.Areas = areas.Where(a => a.Year == year).OrderBy(a => a.AreaOrganizacional).ToList();
        ViewBag.Empleados = await EmpleadosAsync();
    }

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();
}