using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class HistorialLaboralController : Controller
{
    private readonly PeriodoLaboralData _periodoData;
    private readonly ExpLaboralData _expData;
    private readonly RotacionData _rotacionData;
    private readonly ResolucionData _resolucionData;
    private readonly TipoResolucionData _tipoResolucionData;
    private readonly TipoInstitucionData _tipoInstitucionData;
    private readonly EstructOrganizData _estructOrganizData;
    private readonly EmpleadoData _empleadoData;
    private readonly PersonaData _personaData;

    public HistorialLaboralController(
        PeriodoLaboralData periodoData,
        ExpLaboralData expData,
        RotacionData rotacionData,
        ResolucionData resolucionData,
        TipoResolucionData tipoResolucionData,
        TipoInstitucionData tipoInstitucionData,
        EstructOrganizData estructuraOrganizData,
        EmpleadoData empleadoData,
        PersonaData personaData)
    {
        _periodoData = periodoData;
        _expData = expData;
        _rotacionData = rotacionData;
        _resolucionData = resolucionData;
        _tipoResolucionData = tipoResolucionData;
        _tipoInstitucionData = tipoInstitucionData;
        _estructOrganizData = estructuraOrganizData;
        _empleadoData = empleadoData;
        _personaData = personaData;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        var periodos = (await _periodoData.Listar())
            .Where(p => idEmpleado is null || p.IdEmpleado == idEmpleado)
            .OrderByDescending(p => p.FechaIngreso);
        var exp = (await _expData.Listar())
            .Where(e => idEmpleado is null || e.IdEmpleado == idEmpleado)
            .OrderBy(e => e.IdExpLab);
        var rotaciones = (await _rotacionData.Listar())
            .Where(r => idEmpleado is null || r.idEmpleado == idEmpleado)
            .OrderByDescending(r => r.FechaMemo);
        var resoluciones = (await _resolucionData.Listar())
            .Where(r => idEmpleado is null || r.idEmpleado == idEmpleado)
            .OrderByDescending(r => r.FechaResolucion);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposResolucion = await _tipoResolucionData.Listar();
        ViewBag.TiposInstitucion = await _tipoInstitucionData.Listar();
        ViewBag.Areas = await _estructOrganizData.Listar();
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
        var lista = await _periodoData.Listar();
        var maxN = lista.Where(p => p.IdEmpleado == modelo.IdEmpleado).Select(p => p.NroPeriodo).DefaultIfEmpty(0).Max();
        modelo.NroPeriodo = maxN + 1;
        await _periodoData.Crear(modelo);
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> PeriodoEditar(int idEmpleado, int nroPeriodo)
    {
        var registro = await _periodoData.Obtener(idEmpleado, nroPeriodo);
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
        var actual = await _periodoData.Obtener(modelo.IdEmpleado, modelo.NroPeriodo);
        if (actual is null)
            return NotFound();

        actual.NumResoIngreInstitu = modelo.NumResoIngreInstitu;
        actual.FechaSalida = modelo.FechaSalida;
        actual.FechaResoIngreInstitu = modelo.FechaResoIngreInstitu;
        actual.FechaIngreso = modelo.FechaIngreso;
        actual.Judicial = modelo.Judicial;
        actual.Obs = modelo.Obs;
        await _periodoData.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PeriodoEliminar(int idEmpleado, int nroPeriodo)
    {
        var actual = await _periodoData.Obtener(idEmpleado, nroPeriodo);
        if (actual is not null)
            await _periodoData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Experiencia laboral ----------

    [HttpGet]
    public async Task<IActionResult> ExpNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposInstitucion = await _tipoInstitucionData.Listar();
        return View("ExpForm", new ExpLaboral { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExpNuevo(ExpLaboral modelo)
    {
        var lista = await _expData.Listar();
        var maxId = lista.Where(e => e.IdEmpleado == modelo.IdEmpleado).Select(e => e.IdExpLab).DefaultIfEmpty(0).Max();
        modelo.IdExpLab = maxId + 1;
        await _expData.Crear(modelo);
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> ExpEditar(int idEmpleado, int idExpLab)
    {
        var registro = await _expData.Obtener(idEmpleado, idExpLab);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposInstitucion = await _tipoInstitucionData.Listar();
        return View("ExpForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExpEditar(ExpLaboral modelo)
    {
        var actual = await _expData.Obtener(modelo.IdEmpleado, modelo.IdExpLab);
        if (actual is null)
            return NotFound();

        actual.NomInstitucion = modelo.NomInstitucion;
        actual.Cargo = modelo.Cargo;
        actual.FechaIngreso = modelo.FechaIngreso;
        actual.FechaEgreso = modelo.FechaEgreso;
        actual.UnidadOrganica = modelo.UnidadOrganica;
        actual.IdTipoInstitucion = modelo.IdTipoInstitucion;
        await _expData.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExpEliminar(int idEmpleado, int idExpLab)
    {
        var actual = await _expData.Obtener(idEmpleado, idExpLab);
        if (actual is not null)
            await _expData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Rotaciones ----------

    [HttpGet]
    public async Task<IActionResult> RotacionNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Areas = await _estructOrganizData.Listar();
        return View("RotacionForm", new Rotacion { idEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RotacionNuevo(Rotacion modelo)
    {
        var lista = await _rotacionData.Listar();
        var maxId = lista.Where(r => r.idEmpleado == modelo.idEmpleado).Select(r => r.idRotacion).DefaultIfEmpty(0).Max();
        modelo.idRotacion = maxId + 1;
        await _rotacionData.Crear(modelo);
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> RotacionEditar(int idEmpleado, int idRotacion)
    {
        var registro = await _rotacionData.Obtener(idEmpleado, idRotacion);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Areas = await _estructOrganizData.Listar();
        return View("RotacionForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RotacionEditar(Rotacion modelo)
    {
        var actual = await _rotacionData.Obtener(modelo.idEmpleado, modelo.idRotacion);
        if (actual is null)
            return NotFound();

        actual.FechaMemo = modelo.FechaMemo;
        actual.NroMemo = modelo.NroMemo;
        actual.idAreaOrganiz = modelo.idAreaOrganiz;
        actual.Year = modelo.Year;
        await _rotacionData.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RotacionEliminar(int idEmpleado, int idRotacion)
    {
        var actual = await _rotacionData.Obtener(idEmpleado, idRotacion);
        if (actual is not null)
            await _rotacionData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Resoluciones ----------

    [HttpGet]
    public async Task<IActionResult> ResolucionNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposResolucion = await _tipoResolucionData.Listar();
        return View("ResolucionForm", new Resolucion { idEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolucionNuevo(Resolucion modelo)
    {
        var lista = await _resolucionData.Listar();
        var maxId = lista.Where(r => r.idEmpleado == modelo.idEmpleado).Select(r => r.idResolucion).DefaultIfEmpty(0).Max();
        modelo.idResolucion = maxId + 1;
        await _resolucionData.Crear(modelo);
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> ResolucionEditar(int idEmpleado, int idResolucion)
    {
        var registro = await _resolucionData.Obtener(idEmpleado, idResolucion);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposResolucion = await _tipoResolucionData.Listar();
        return View("ResolucionForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolucionEditar(Resolucion modelo)
    {
        var actual = await _resolucionData.Obtener(modelo.idEmpleado, modelo.idResolucion);
        if (actual is null)
            return NotFound();

        actual.idTipoResolucion = modelo.idTipoResolucion;
        actual.FechaResolucion = modelo.FechaResolucion;
        actual.NumeroResolucion = modelo.NumeroResolucion;
        await _resolucionData.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResolucionEliminar(int idEmpleado, int idResolucion)
    {
        var actual = await _resolucionData.Obtener(idEmpleado, idResolucion);
        if (actual is not null)
            await _resolucionData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Utilidades ----------

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

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
    {
        var empleado = await _empleadoData.Obtener(idEmpleado);
        var persona = empleado?.IdPersona is null ? null : (await _personaData.Listar()).FirstOrDefault(p => p.IdPersona == empleado.IdPersona);
        return persona is null
            ? $"(Empleado {idEmpleado})"
            : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
    }
}