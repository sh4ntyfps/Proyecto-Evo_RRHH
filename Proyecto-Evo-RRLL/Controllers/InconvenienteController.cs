using Capa_Logica;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class InconvenienteController : Controller
{
    private readonly InconvenienteLogica _inconvenienteData;
    private readonly SecuenciaService _secuencia;

    public InconvenienteController(InconvenienteLogica inconvenienteData, SecuenciaService secuencia)
    {
        _inconvenienteData = inconvenienteData;
        _secuencia = secuencia;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? idObjetivo, int? idActividad)
    {
        var registros = (await _inconvenienteData.Listar())
            .Where(i => idObjetivo is null || i.IdObjetivo == idObjetivo)
            .Where(i => idActividad is null || i.Id_Actividad == idActividad)
            .OrderBy(i => i.IdObjetivo)
            .ThenBy(i => i.Id_Actividad)
            .ThenBy(i => i.IdInconveniente);

        ViewBag.IdObjetivo = idObjetivo;
        ViewBag.IdActividad = idActividad;
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo(int? idObjetivo, int? idActividad)
    {
        var modelo = new Inconveniente
        {
            IdObjetivo = idObjetivo ?? 0,
            Id_Actividad = idActividad ?? 0
        };
        ViewBag.IdObjetivo = idObjetivo;
        ViewBag.IdActividad = idActividad;
        ViewBag.EsNuevo = true;
        return View("Form", modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(Inconveniente modelo)
    {
        if (modelo.IdObjetivo <= 0)
            ModelState.AddModelError("IdObjetivo", "Indique el objetivo.");
        if (modelo.Id_Actividad <= 0)
            ModelState.AddModelError("Id_Actividad", "Indique la actividad.");
        if (string.IsNullOrWhiteSpace(modelo.Descripcion))
            ModelState.AddModelError("Descripcion", "Indique el inconveniente.");

        if (ModelState.IsValid)
        {
            var guardo = await _secuencia.EjecutarConBloqueoAsync(
                async () =>
                {
                    var lista = await _inconvenienteData.Listar();
                    return lista
                        .Where(i => i.IdObjetivo == modelo.IdObjetivo && i.Id_Actividad == modelo.Id_Actividad)
                        .Select(i => i.IdInconveniente)
                        .DefaultIfEmpty(0)
                        .Max() + 1;
                },
                async id =>
                {
                    modelo.IdInconveniente = id;
                    await _inconvenienteData.Crear(modelo);
                    return true;
                });
            if (guardo)
            {
                TempData["MensajeExito"] = "Inconveniente registrado.";
                return RedirectToAction(nameof(Index), new { idObjetivo = modelo.IdObjetivo, idActividad = modelo.Id_Actividad });
            }
            ModelState.AddModelError("", "No se pudo registrar el inconveniente: intente nuevamente.");
        }

        ViewBag.IdObjetivo = modelo.IdObjetivo;
        ViewBag.IdActividad = modelo.Id_Actividad;
        ViewBag.EsNuevo = true;
        return View("Form", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int idObjetivo, int idActividad, int idInconveniente)
    {
        var registro = await _inconvenienteData.Obtener(idObjetivo, idActividad, idInconveniente);
        if (registro is null)
            return NotFound();

        ViewBag.IdObjetivo = idObjetivo;
        ViewBag.IdActividad = idActividad;
        ViewBag.EsNuevo = false;
        return View("Form", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(Inconveniente modelo)
    {
        var actual = await _inconvenienteData.Obtener(modelo.IdObjetivo, modelo.Id_Actividad, modelo.IdInconveniente);
        if (actual is null)
            return NotFound();

        actual.Descripcion = modelo.Descripcion;
        actual.Importancia = modelo.Importancia;
        actual.Desventaja = modelo.Desventaja;
        actual.Observacion = modelo.Observacion;
        await _inconvenienteData.Actualizar(actual);

        TempData["MensajeExito"] = "Inconveniente actualizado.";
        return RedirectToAction(nameof(Index), new { idObjetivo = modelo.IdObjetivo, idActividad = modelo.Id_Actividad });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int idObjetivo, int idActividad, int idInconveniente)
    {
        var actual = await _inconvenienteData.Obtener(idObjetivo, idActividad, idInconveniente);
        if (actual is not null)
            await _inconvenienteData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idObjetivo, idActividad });
    }
}