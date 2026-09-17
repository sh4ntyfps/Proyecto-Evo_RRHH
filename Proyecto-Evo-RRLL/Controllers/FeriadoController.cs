using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class FeriadoController : Controller
{
    private readonly RRHH_FeriadoData _feriadoData;

    public FeriadoController(RRHH_FeriadoData feriadoData)
    {
        _feriadoData = feriadoData;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var feriados = (await _feriadoData.Listar()).OrderBy(f => f.Fecha);
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
        var lista = await _feriadoData.Listar();
        modelo.idFeriado = lista.Count > 0 ? lista.Max(f => f.idFeriado) + 1 : 1;
        await _feriadoData.Crear(modelo);
        TempData["MensajeExito"] = "Feriado registrado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var registro = await _feriadoData.Obtener(id);
        if (registro is null)
            return NotFound();
        return View("Form", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(RRHH_Feriado modelo)
    {
        var actual = await _feriadoData.Obtener(modelo.idFeriado);
        if (actual is null)
            return NotFound();

        actual.Fecha = modelo.Fecha;
        actual.Motivo = modelo.Motivo;
        await _feriadoData.Actualizar(actual);
        TempData["MensajeExito"] = "Feriado actualizado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var actual = await _feriadoData.Obtener(id);
        if (actual is not null)
            await _feriadoData.Eliminar(actual);
        return RedirectToAction(nameof(Index));
    }
}