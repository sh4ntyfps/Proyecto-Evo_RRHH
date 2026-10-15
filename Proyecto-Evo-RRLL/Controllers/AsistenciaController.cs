using Capa_Logica;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class AsistenciaController : Controller
{
    private readonly AsistenciaLogica _asistenciaLogica;
    private readonly MarcacionLogica _marcacionLogica;
    private readonly RegAsisDiarioLogica _regAsisLogica;
    private readonly HorarioTemporalLogica _horarioTemporalLogica;
    private readonly HorarioLogica _horarioLogica;
    private readonly EmpleadoLogica _empleadoLogica;
    private readonly PersonaLogica _personaLogica;
    private readonly EmpleadoServicio _empleados;

    public AsistenciaController(
        AsistenciaLogica asistenciaLogica,
        MarcacionLogica marcacionLogica,
        RegAsisDiarioLogica regAsisLogica,
        HorarioTemporalLogica horarioTemporalLogica,
        HorarioLogica horarioLogica,
        EmpleadoLogica empleadoLogica,
        PersonaLogica personaLogica,
        EmpleadoServicio empleados)
    {
        _asistenciaLogica = asistenciaLogica;
        _marcacionLogica = marcacionLogica;
        _regAsisLogica = regAsisLogica;
        _horarioTemporalLogica = horarioTemporalLogica;
        _horarioLogica = horarioLogica;
        _empleadoLogica = empleadoLogica;
        _personaLogica = personaLogica;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(DateTime? desde, DateTime? hasta, string? q)
    {
        var registros = await _asistenciaLogica.Listar();
        var empleados = await _empleadoLogica.Listar();
        var personas = await _personaLogica.Listar();

        var desdeEf = desde ?? DateTime.Today.AddMonths(-1);
        var hastaEf = hasta ?? DateTime.Today.AddDays(1);

        var vm = new AsistenciaViewModel { Desde = desde, Hasta = hasta ?? DateTime.Today, Busqueda = q };

        foreach (var r in registros.Where(x => x.Fecha >= desdeEf && x.Fecha < hastaEf))
        {
            var empleado = empleados.FirstOrDefault(e => e.IdEmpleado == r.IdEmpleado);
            var persona = empleado?.IdPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == empleado.IdPersona);
            var nombre = persona is null
                ? $"(Empleado {r.IdEmpleado})"
                : persona.NombreCompleto();

            if (!string.IsNullOrWhiteSpace(q)
                && !nombre.Contains(q, StringComparison.OrdinalIgnoreCase)
                && !(persona?.NumDocID?.Contains(q, StringComparison.OrdinalIgnoreCase) == true))
                continue;

            vm.Registros.Add(new AsistenciaFila
            {
                Fecha = r.Fecha,
                IdEmpleado = r.IdEmpleado,
                Empleado = nombre,
                HorEntrada = r.Hor_Ent,
                HorSalida = r.Hor_Sal,
                AlmSalida = r.Alm_Sal,
                AlmEntrada = r.Alm_Ent,
                Estado = r.Estado
            });

            if (vm.Registros.Count >= 2000)
                break;
        }

        vm.Registros = vm.Registros.OrderByDescending(x => x.Fecha).ThenBy(x => x.IdEmpleado).ToList();
        return View(vm);
    }

    // ---------- Marcaciones ----------

    [HttpGet]
    public async Task<IActionResult> Marcaciones(DateTime? desde, DateTime? hasta, int? idEmpleado)
    {
        var registros = (await _marcacionLogica.Listar())
            .Where(m => idEmpleado is null || m.IdEmpleado == idEmpleado)
            .Where(m => !desde.HasValue || m.Fecha.Date >= desde.Value.Date)
            .Where(m => !hasta.HasValue || m.Fecha.Date <= hasta.Value.Date)
            .OrderByDescending(m => m.Fecha)
            .ThenBy(m => m.IdEmpleado)
            .Take(2000);

        var horarios = await _horarioLogica.Listar();

        ViewBag.Desde = desde;
        ViewBag.Hasta = hasta;
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = horarios;
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> MarcacionNuevo()
    {
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioLogica.Listar();
        return View("MarcacionForm", new Marcacion { Fecha = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcacionNuevo(Marcacion modelo)
    {
        if (await _marcacionLogica.Obtener(modelo.Fecha, modelo.IdEmpleado) is not null)
            ModelState.AddModelError("", "Ya existe una marcación para este empleado en esa fecha.");

        if (ModelState.IsValid)
        {
            await _marcacionLogica.Crear(modelo);
            return RedirectToAction(nameof(Marcaciones), new { idEmpleado = modelo.IdEmpleado });
        }

        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioLogica.Listar();
        return View("MarcacionForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> MarcacionEditar(DateTime fecha, int idEmpleado)
    {
        var registro = await _marcacionLogica.Obtener(fecha, idEmpleado);
        if (registro is null)
            return NotFound();

        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioLogica.Listar();
        return View("MarcacionForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcacionEditar(Marcacion modelo)
    {
        var actual = await _marcacionLogica.Obtener(modelo.Fecha, modelo.IdEmpleado);
        if (actual is null)
            return NotFound();

        actual.Estado = modelo.Estado;
        actual.Lugar = modelo.Lugar;
        actual.idHorario = modelo.idHorario;
        await _marcacionLogica.Actualizar(actual);

        return RedirectToAction(nameof(Marcaciones), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcacionEliminar(DateTime fecha, int idEmpleado)
    {
        var actual = await _marcacionLogica.Obtener(fecha, idEmpleado);
        if (actual is not null)
            await _marcacionLogica.Eliminar(actual);
        return RedirectToAction(nameof(Marcaciones), new { idEmpleado });
    }

    // ---------- Registro diario ----------

    [HttpGet]
    public async Task<IActionResult> RegDiario(DateTime? desde, DateTime? hasta, int? idEmpleado)
    {
        var registros = (await _regAsisLogica.Listar())
            .Where(m => idEmpleado is null || m.IdEmpleado == idEmpleado)
            .Where(m => !desde.HasValue || m.Fecha.Date >= desde.Value.Date)
            .Where(m => !hasta.HasValue || m.Fecha.Date <= hasta.Value.Date)
            .OrderByDescending(m => m.Fecha)
            .ThenBy(m => m.IdEmpleado)
            .Take(2000);

        ViewBag.Desde = desde;
        ViewBag.Hasta = hasta;
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> RegDiarioNuevo()
    {
        ViewBag.Empleados = await EmpleadosAsync();
        return View("RegDiarioForm", new RegAsisDiario { Fecha = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegDiarioNuevo(RegAsisDiario modelo)
    {
        if (await _regAsisLogica.Obtener(modelo.Fecha, modelo.IdEmpleado) is not null)
            ModelState.AddModelError("", "Ya existe un registro diario para este empleado en esa fecha.");

        if (ModelState.IsValid)
        {
            await _regAsisLogica.Crear(modelo);
            return RedirectToAction(nameof(RegDiario), new { idEmpleado = modelo.IdEmpleado });
        }

        ViewBag.Empleados = await EmpleadosAsync();
        return View("RegDiarioForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> RegDiarioEditar(DateTime fecha, int idEmpleado)
    {
        var registro = await _regAsisLogica.Obtener(fecha, idEmpleado);
        if (registro is null)
            return NotFound();

        ViewBag.Empleados = await EmpleadosAsync();
        return View("RegDiarioForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegDiarioEditar(RegAsisDiario modelo)
    {
        var actual = await _regAsisLogica.Obtener(modelo.Fecha, modelo.IdEmpleado);
        if (actual is null)
            return NotFound();

        actual.MinNormales = modelo.MinNormales;
        actual.MinTarde = modelo.MinTarde;
        actual.Estado = modelo.Estado;
        await _regAsisLogica.Actualizar(actual);

        return RedirectToAction(nameof(RegDiario), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegDiarioEliminar(DateTime fecha, int idEmpleado)
    {
        var actual = await _regAsisLogica.Obtener(fecha, idEmpleado);
        if (actual is not null)
            await _regAsisLogica.Eliminar(actual);
        return RedirectToAction(nameof(RegDiario), new { idEmpleado });
    }

    // ---------- Horarios temporales ----------

    [HttpGet]
    public async Task<IActionResult> HorariosTemporal(int? idEmpleado)
    {
        var registros = (await _horarioTemporalLogica.Listar())
            .Where(h => idEmpleado is null || h.idEmpleado == idEmpleado)
            .OrderByDescending(h => h.FechaInicio);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioLogica.Listar();
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> HorarioTemporalNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioLogica.Listar();
        return View("HorarioTemporalForm", new HorarioTemporal { idEmpleado = idEmpleado ?? 0, FechaRegistro = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HorarioTemporalNuevo(HorarioTemporal modelo)
    {
        var guardo = await _horarioTemporalLogica.Registrar(modelo);
        if (guardo)
            return RedirectToAction(nameof(HorariosTemporal), new { idEmpleado = modelo.idEmpleado });

        ViewBag.IdEmpleado = modelo.idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioLogica.Listar();
        ModelState.AddModelError("", "No se pudo registrar el horario temporal: intente nuevamente.");
        return View("HorarioTemporalForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> HorarioTemporalEditar(int idEmpleado, int n)
    {
        var registro = await _horarioTemporalLogica.Obtener(idEmpleado, n);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Horarios = await _horarioLogica.Listar();
        return View("HorarioTemporalForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HorarioTemporalEditar(HorarioTemporal modelo)
    {
        var actual = await _horarioTemporalLogica.Obtener(modelo.idEmpleado, modelo.N);
        if (actual is null)
            return NotFound();

        actual.FechaInicio = modelo.FechaInicio;
        actual.FechaFin = modelo.FechaFin;
        actual.HorarioAsignado = modelo.HorarioAsignado;
        actual.NroDocumento = modelo.NroDocumento;
        actual.Sisgedo = modelo.Sisgedo;
        actual.FechaRegistro = modelo.FechaRegistro;
        await _horarioTemporalLogica.Actualizar(actual);

        return RedirectToAction(nameof(HorariosTemporal), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HorarioTemporalEliminar(int idEmpleado, int n)
    {
        var actual = await _horarioTemporalLogica.Obtener(idEmpleado, n);
        if (actual is not null)
            await _horarioTemporalLogica.Eliminar(actual);
        return RedirectToAction(nameof(HorariosTemporal), new { idEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();
}