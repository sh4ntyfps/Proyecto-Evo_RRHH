using Capa_Logica;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class HistorialLaboralController : Controller
{
    private readonly PeriodoLaboralLogica _periodoLogica;
    private readonly ExpLaboralLogica _expLogica;
    private readonly RotacionLogica _rotacionLogica;
    private readonly ResolucionLogica _resolucionLogica;
    private readonly TipoResolucionLogica _tipoResolucionLogica;
    private readonly TipoInstitucionLogica _tipoInstitucionLogica;
    private readonly EstructOrganizLogica _estructOrganizLogica;
    private readonly EmpleadoServicio _empleados;

    public HistorialLaboralController(
        PeriodoLaboralLogica periodoLogica,
        ExpLaboralLogica expLogica,
        RotacionLogica rotacionLogica,
        ResolucionLogica resolucionLogica,
        TipoResolucionLogica tipoResolucionLogica,
        TipoInstitucionLogica tipoInstitucionLogica,
        EstructOrganizLogica estructuraOrganizLogica,
        EmpleadoServicio empleados)
    {
        _periodoLogica = periodoLogica;
        _expLogica = expLogica;
        _rotacionLogica = rotacionLogica;
        _resolucionLogica = resolucionLogica;
        _tipoResolucionLogica = tipoResolucionLogica;
        _tipoInstitucionLogica = tipoInstitucionLogica;
        _estructOrganizLogica = estructuraOrganizLogica;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        var periodos = (await _periodoLogica.Listar())
            .Where(p => idEmpleado is null || p.IdEmpleado == idEmpleado)
            .OrderByDescending(p => p.FechaIngreso);
        var exp = (await _expLogica.Listar())
            .Where(e => idEmpleado is null || e.IdEmpleado == idEmpleado)
            .OrderBy(e => e.IdExpLab);
        var rotaciones = (await _rotacionLogica.Listar())
            .Where(r => idEmpleado is null || r.idEmpleado == idEmpleado)
            .OrderByDescending(r => r.FechaMemo);
        var resoluciones = (await _resolucionLogica.Listar())
            .Where(r => idEmpleado is null || r.idEmpleado == idEmpleado)
            .OrderByDescending(r => r.FechaResolucion);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposResolucion = await _tipoResolucionLogica.Listar();
        ViewBag.TiposInstitucion = await _tipoInstitucionLogica.Listar();
        ViewBag.Areas = await _estructOrganizLogica.Listar();
        return View(new HistorialLaboralModelo
        {
            Periodos = periodos.ToList(),
            Experiencias = exp.ToList(),
            Rotaciones = rotaciones.ToList(),
            Resoluciones = resoluciones.ToList()
        });
    }

    // ---------- Períodos laborales ----------

    [HttpGet]
    public async Task<IActionResult> PeriodoNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("PeriodoForm", new PeriodoLaboral { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PeriodoNuevo(PeriodoLaboral modelo)
    {
        var guardo = await _periodoLogica.Registrar(modelo);
        if (guardo)
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ModelState.AddModelError("", "No se pudo registrar el período: intente nuevamente.");
        return View("PeriodoForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> PeriodoEditar(int idEmpleado, int nroPeriodo)
    {
        var registro = await _periodoLogica.Obtener(idEmpleado, nroPeriodo);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("PeriodoForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PeriodoEditar(PeriodoLaboral modelo)
    {
        var actual = await _periodoLogica.Obtener(modelo.IdEmpleado, modelo.NroPeriodo);
        if (actual is null)
            return NotFound();

        actual.NumResoIngreInstitu = modelo.NumResoIngreInstitu;
        actual.FechaSalida = modelo.FechaSalida;
        actual.FechaResoIngreInstitu = modelo.FechaResoIngreInstitu;
        actual.FechaIngreso = modelo.FechaIngreso;
        actual.Judicial = modelo.Judicial;
        actual.Obs = modelo.Obs;
        await _periodoLogica.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PeriodoEliminar(int idEmpleado, int nroPeriodo)
    {
        var actual = await _periodoLogica.Obtener(idEmpleado, nroPeriodo);
        if (actual is not null)
            await _periodoLogica.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Experiencia laboral ----------

    [HttpGet]
    public async Task<IActionResult> ExpNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposInstitucion = await _tipoInstitucionLogica.Listar();
        return View("ExpForm", new ExpLaboral { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExpNuevo(ExpLaboral modelo)
    {
        var guardo = await _expLogica.Registrar(modelo);
        if (guardo)
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposInstitucion = await _tipoInstitucionLogica.Listar();
        ModelState.AddModelError("", "No se pudo registrar la experiencia: intente nuevamente.");
        return View("ExpForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> ExpEditar(int idEmpleado, int idExpLab)
    {
        var registro = await _expLogica.Obtener(idEmpleado, idExpLab);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposInstitucion = await _tipoInstitucionLogica.Listar();
        return View("ExpForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExpEditar(ExpLaboral modelo)
    {
        var actual = await _expLogica.Obtener(modelo.IdEmpleado, modelo.IdExpLab);
        if (actual is null)
            return NotFound();

        actual.NomInstitucion = modelo.NomInstitucion;
        actual.Cargo = modelo.Cargo;
        actual.FechaIngreso = modelo.FechaIngreso;
        actual.FechaEgreso = modelo.FechaEgreso;
        actual.UnidadOrganica = modelo.UnidadOrganica;
        actual.IdTipoInstitucion = modelo.IdTipoInstitucion;
        await _expLogica.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExpEliminar(int idEmpleado, int idExpLab)
    {
        var actual = await _expLogica.Obtener(idEmpleado, idExpLab);
        if (actual is not null)
            await _expLogica.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Rotaciones ----------

    [HttpGet]
    public async Task<IActionResult> RotacionNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Areas = await _estructOrganizLogica.Listar();
        return View("RotacionForm", new Rotacion { idEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RotacionNuevo(Rotacion modelo)
    {
        var guardo = await _rotacionLogica.Registrar(modelo);
        if (guardo)
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });

        ViewBag.IdEmpleado = modelo.idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Areas = await _estructOrganizLogica.Listar();
        ModelState.AddModelError("", "No se pudo registrar la rotación: intente nuevamente.");
        return View("RotacionForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> RotacionEditar(int idEmpleado, int idRotacion)
    {
        var registro = await _rotacionLogica.Obtener(idEmpleado, idRotacion);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Areas = await _estructOrganizLogica.Listar();
        return View("RotacionForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RotacionEditar(Rotacion modelo)
    {
        var actual = await _rotacionLogica.Obtener(modelo.idEmpleado, modelo.idRotacion);
        if (actual is null)
            return NotFound();

        actual.FechaMemo = modelo.FechaMemo;
        actual.NroMemo = modelo.NroMemo;
        actual.idAreaOrganiz = modelo.idAreaOrganiz;
        actual.Year = modelo.Year;
        await _rotacionLogica.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RotacionEliminar(int idEmpleado, int idRotacion)
    {
        var actual = await _rotacionLogica.Obtener(idEmpleado, idRotacion);
        if (actual is not null)
            await _rotacionLogica.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Resoluciones ----------

    [HttpGet]
    public async Task<IActionResult> ResolucionNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposResolucion = await _tipoResolucionLogica.Listar();
        return View("ResolucionForm", new Resolucion { idEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolucionNuevo(Resolucion modelo)
    {
        var guardo = await _resolucionLogica.Registrar(modelo);
        if (guardo)
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });

        ViewBag.IdEmpleado = modelo.idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposResolucion = await _tipoResolucionLogica.Listar();
        ModelState.AddModelError("", "No se pudo registrar la resolución: intente nuevamente.");
        return View("ResolucionForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> ResolucionEditar(int idEmpleado, int idResolucion)
    {
        var registro = await _resolucionLogica.Obtener(idEmpleado, idResolucion);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposResolucion = await _tipoResolucionLogica.Listar();
        return View("ResolucionForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolucionEditar(Resolucion modelo)
    {
        var actual = await _resolucionLogica.Obtener(modelo.idEmpleado, modelo.idResolucion);
        if (actual is null)
            return NotFound();

        actual.idTipoResolucion = modelo.idTipoResolucion;
        actual.FechaResolucion = modelo.FechaResolucion;
        actual.NumeroResolucion = modelo.NumeroResolucion;
        await _resolucionLogica.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolucionEliminar(int idEmpleado, int idResolucion)
    {
        var actual = await _resolucionLogica.Obtener(idEmpleado, idResolucion);
        if (actual is not null)
            await _resolucionLogica.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => await _empleados.NombreEmpleadoAsync(idEmpleado);
}