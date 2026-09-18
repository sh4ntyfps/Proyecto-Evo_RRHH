using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class ComportamientoController : Controller
{
    private readonly ComportamientoData _comportamientoData;
    private readonly TipoComportamientoData _tipoData;
    private readonly EmpleadoData _empleadoData;
    private readonly PersonaData _personaData;
    private readonly SecuenciaService _secuencia;
    private readonly Services.EmpleadoServicio _empleados;

    public ComportamientoController(
        ComportamientoData comportamientoData,
        TipoComportamientoData tipoData,
        EmpleadoData empleadoData,
        PersonaData personaData,
        SecuenciaService secuencia,
        Services.EmpleadoServicio empleados)
    {
        _comportamientoData = comportamientoData;
        _tipoData = tipoData;
        _empleadoData = empleadoData;
        _personaData = personaData;
        _secuencia = secuencia;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        var registros = (await _comportamientoData.Listar())
            .Where(c => idEmpleado is null || c.IdEmpleado == idEmpleado)
            .OrderByDescending(c => c.Fecha);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Tipos = await _tipoData.Listar();
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Tipos = await _tipoData.Listar();
        return View("Form", new Comportamiento { IdEmpleado = idEmpleado, Fecha = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(Comportamiento modelo)
    {
        if (modelo.IdEmpleado is null)
            ModelState.AddModelError("", "Seleccione un empleado.");

        if (ModelState.IsValid)
        {
            var guardo = await _secuencia.EjecutarConBloqueoAsync(
                async () =>
                {
                    var lista = await _comportamientoData.Listar();
                    return lista.Count > 0 ? lista.Max(c => c.idComportamiento) + 1 : 1;
                },
                async id =>
                {
                    modelo.idComportamiento = id;
                    await _comportamientoData.Crear(modelo);
                    return true;
                });
            if (guardo)
            {
                TempData["MensajeExito"] = "Comportamiento registrado.";
                return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
            }
            ModelState.AddModelError("", "No se pudo registrar el comportamiento: intente nuevamente.");
        }

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Tipos = await _tipoData.Listar();
        return View("Form", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var registro = await _comportamientoData.Obtener(id);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = registro.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Tipos = await _tipoData.Listar();
        return View("Form", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(Comportamiento modelo)
    {
        var actual = await _comportamientoData.Obtener(modelo.idComportamiento);
        if (actual is null)
            return NotFound();

        actual.Fecha = modelo.Fecha;
        actual.IdEmpleado = modelo.IdEmpleado;
        actual.NumResol = modelo.NumResol;
        actual.Titulo = modelo.Titulo;
        actual.Descripcion = modelo.Descripcion;
        actual.idTipoComportamiento = modelo.idTipoComportamiento;
        await _comportamientoData.Actualizar(actual);

        TempData["MensajeExito"] = "Comportamiento actualizado.";
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var actual = await _comportamientoData.Obtener(id);
        if (actual is not null)
            await _comportamientoData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado = actual?.IdEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => await _empleados.NombreEmpleadoAsync(idEmpleado);
}