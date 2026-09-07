using Capa_Datos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class CatalogoController : Controller
{
    private readonly CatalogoRepositorio _repositorio;

    public CatalogoController(CatalogoRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string tabla)
    {
        if (!CatalogoNavegacion.Items.ContainsKey(tabla))
            return NotFound();

        var columnas = _repositorio.Columnas(tabla);
        var filas = new List<IReadOnlyDictionary<string, object?>>();

        foreach (var entidad in await _repositorio.Listar(tabla))
        {
            var fila = new Dictionary<string, object?>();
            foreach (var col in columnas)
            {
                var valor = entidad.GetType().GetProperty(col.Nombre)?.GetValue(entidad);
                fila[col.Nombre] = valor is null ? null : valor.ToString();
            }
            filas.Add(fila);
        }

        return View(new CatalogoViewModel
        {
            Tabla = tabla,
            Titulo = CatalogoNavegacion.Items[tabla],
            Columnas = columnas,
            Filas = filas,
            Claves = _repositorio.ClavesPrimarias(tabla),
            Editable = CatalogoRepositorio.EsEditable(tabla)
        });
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo(string tabla)
    {
        if (!CatalogoNavegacion.Items.ContainsKey(tabla))
            return NotFound();

        return View(new CatalogoViewModel
        {
            Tabla = tabla,
            Titulo = $"Nuevo - {CatalogoNavegacion.Items[tabla]}",
            Columnas = _repositorio.Columnas(tabla),
            EsFormulario = true,
            Valores = new Dictionary<string, object?>()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(string tabla, IFormCollection form)
    {
        var datos = FormAAccion(form);
        try
        {
            await _repositorio.Crear(tabla, datos);
            TempData["MensajeExito"] = $"{CatalogoNavegacion.Items[tabla]}: registro creado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["MensajeError"] = $"No se pudo crear el registro: {ex.Message}";
        }
        return RedirectToAction("Index", new { tabla });
    }

    [HttpGet]
    public async Task<IActionResult> Editar(string tabla, string ids)
    {
        var separados = ids.Split('|');
        var entidad = await _repositorio.Obtener(tabla, separados);
        if (entidad is null)
            return NotFound();

        var columnas = _repositorio.Columnas(tabla);
        var valores = new Dictionary<string, object?>();
        foreach (var col in columnas)
            valores[col.Nombre] = entidad.GetType().GetProperty(col.Nombre)?.GetValue(entidad);

        return View("Nuevo", new CatalogoViewModel
        {
            Tabla = tabla,
            Titulo = $"Editar - {CatalogoNavegacion.Items[tabla]}",
            Columnas = columnas,
            Claves = _repositorio.ClavesPrimarias(tabla),
            EsFormulario = true,
            Valores = valores
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(string tabla, string ids, IFormCollection form)
    {
        var separados = ids.Split('|');
        var datos = FormAAccion(form);
        try
        {
            await _repositorio.Actualizar(tabla, separados, datos);
            TempData["MensajeExito"] = $"{CatalogoNavegacion.Items[tabla]}: registro actualizado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["MensajeError"] = $"No se pudo actualizar el registro: {ex.Message}";
        }
        return RedirectToAction("Index", new { tabla });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(string tabla, string ids)
    {
        try
        {
            await _repositorio.Eliminar(tabla, ids.Split('|'));
            TempData["MensajeExito"] = $"{CatalogoNavegacion.Items[tabla]}: registro eliminado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["MensajeError"] = $"No se pudo eliminar el registro: {ex.Message}";
        }
        return RedirectToAction("Index", new { tabla });
    }

    private static IReadOnlyDictionary<string, string> FormAAccion(IFormCollection form)
    {
        var dict = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var clave in form.Keys)
            dict[clave] = form[clave].ToString();
        return dict;
    }
}