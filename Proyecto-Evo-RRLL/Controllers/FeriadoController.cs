using Capa_Logica;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class FeriadoController : Controller
{
    private readonly RRHH_FeriadoLogica _feriadoLogica;

    public FeriadoController(RRHH_FeriadoLogica feriadoLogica)
    {
        _feriadoLogica = feriadoLogica;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var feriados = (await _feriadoLogica.Listar()).OrderBy(f => f.Fecha);
        return View(feriados.ToList());
    }

    [HttpGet]
    public IActionResult Nuevo()
    {
        return View("Form", new RRHH_Feriado { Fecha = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(RRHH_Feriado modelo)
    {
        var guardo = await _feriadoLogica.Registrar(modelo);
        if (!guardo)
            ModelState.AddModelError("", "No se pudo registrar el feriado: intente nuevamente.");
        else
        {
            TempData["MensajeExito"] = "Feriado registrado.";
            return RedirectToAction(nameof(Index));
        }
        return View("Form", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var registro = await _feriadoLogica.Obtener(id);
        if (registro is null)
            return NotFound();
        return View("Form", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(RRHH_Feriado modelo)
    {
        var actual = await _feriadoLogica.Obtener(modelo.idFeriado);
        if (actual is null)
            return NotFound();

        actual.Fecha = modelo.Fecha;
        actual.Motivo = modelo.Motivo;
        await _feriadoLogica.Actualizar(actual);
        TempData["MensajeExito"] = "Feriado actualizado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var actual = await _feriadoLogica.Obtener(id);
        if (actual is not null)
            await _feriadoLogica.Eliminar(actual);
        return RedirectToAction(nameof(Index));
    }
}