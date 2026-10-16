using Capa_Logica;
using Capa_Entidades.Constantes;
using Capa_Logica.Exportacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class CatalogoController : Controller
{
    private readonly CatalogoLogica _catalogo;
    private readonly BitacoraLogica _bitacora;
    private readonly ExportadorTablas _exportador;

    public CatalogoController(CatalogoLogica catalogo, BitacoraLogica bitacora, ExportadorTablas exportador)
    {
        _catalogo = catalogo;
        _bitacora = bitacora;
        _exportador = exportador;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string tabla)
    {
        if (!CatalogoNavegacion.Items.ContainsKey(tabla))
            return NotFound();

        var columnas = _catalogo.Columnas(tabla);
        var filas = new List<IReadOnlyDictionary<string, object?>>();

        foreach (var entidad in await _catalogo.Listar(tabla))
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
            Claves = _catalogo.ClavesPrimarias(tabla),
            Editable = CatalogoLogica.EsEditable(tabla)
        });
    }

    [HttpGet]
    [Authorize(Roles = ConstantesSeguridad.RolAdministrador)]
    public async Task<IActionResult> Nuevo(string tabla)
    {
        if (!EsCatalogoEditable(tabla))
            return NotFound();

        return View(new CatalogoViewModel
        {
            Tabla = tabla,
            Titulo = $"Nuevo - {CatalogoNavegacion.Items[tabla]}",
            Columnas = _catalogo.Columnas(tabla),
            EsFormulario = true,
            Valores = new Dictionary<string, object?>()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = ConstantesSeguridad.RolAdministrador)]
    public async Task<IActionResult> Nuevo(string tabla, IFormCollection form)
    {
        if (!EsCatalogoEditable(tabla))
            return NotFound();

        var datos = FormAAccion(form);
        try
        {
            await _catalogo.Crear(tabla, datos);
            TempData["MensajeExito"] = $"{CatalogoNavegacion.Items[tabla]}: registro creado correctamente.";
            await _bitacora.Registrar(User.Identity?.Name ?? "?", "Crear", CatalogoNavegacion.Items[tabla], string.Join(" | ", datos.Select(d => $"{d.Key}={d.Value}")));
        }
        catch (Exception ex)
        {
            TempData["MensajeError"] = $"No se pudo crear el registro: {ex.Message}";
        }
        return RedirectToAction("Index", new { tabla });
    }

    [HttpGet]
    [Authorize(Roles = ConstantesSeguridad.RolAdministrador)]
    public async Task<IActionResult> Editar(string tabla, string ids)
    {
        if (!EsCatalogoEditable(tabla))
            return NotFound();

        var separados = ids.Split('|');
        var entidad = await _catalogo.Obtener(tabla, separados);
        if (entidad is null)
            return NotFound();

        var columnas = _catalogo.Columnas(tabla);
        var valores = new Dictionary<string, object?>();
        foreach (var col in columnas)
            valores[col.Nombre] = entidad.GetType().GetProperty(col.Nombre)?.GetValue(entidad);

        return View("Nuevo", new CatalogoViewModel
        {
            Tabla = tabla,
            Titulo = $"Editar - {CatalogoNavegacion.Items[tabla]}",
            Columnas = columnas,
            Claves = _catalogo.ClavesPrimarias(tabla),
            EsFormulario = true,
            Valores = valores
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = ConstantesSeguridad.RolAdministrador)]
    public async Task<IActionResult> Editar(string tabla, string ids, IFormCollection form)
    {
        if (!EsCatalogoEditable(tabla))
            return NotFound();

        var separados = ids.Split('|');
        var datos = FormAAccion(form);
        try
        {
            await _catalogo.Actualizar(tabla, separados, datos);
            TempData["MensajeExito"] = $"{CatalogoNavegacion.Items[tabla]}: registro actualizado correctamente.";
            await _bitacora.Registrar(User.Identity?.Name ?? "?", "Editar", CatalogoNavegacion.Items[tabla], $"clave(s)={ids}");
        }
        catch (Exception ex)
        {
            TempData["MensajeError"] = $"No se pudo actualizar el registro: {ex.Message}";
        }
        return RedirectToAction("Index", new { tabla });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = ConstantesSeguridad.RolAdministrador)]
    public async Task<IActionResult> Eliminar(string tabla, string ids)
    {
        if (!EsCatalogoEditable(tabla))
            return NotFound();

        try
        {
            await _catalogo.Eliminar(tabla, ids.Split('|'));
            TempData["MensajeExito"] = $"{CatalogoNavegacion.Items[tabla]}: registro eliminado correctamente.";
            await _bitacora.Registrar(User.Identity?.Name ?? "?", "Eliminar", CatalogoNavegacion.Items[tabla], $"clave(s)={ids}");
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

    private static bool EsCatalogoEditable(string tabla)
        => CatalogoNavegacion.Items.ContainsKey(tabla) && CatalogoLogica.EsEditable(tabla);

    [HttpGet]
    [Authorize(Roles = ConstantesSeguridad.RolAdministrador)]
    public async Task<IActionResult> Exportar(string tabla, string formato = "csv")
    {
        if (!CatalogoNavegacion.Items.ContainsKey(tabla) || !_exportador.Soporta(formato))
            return NotFound();

        var columnas = _catalogo.Columnas(tabla);
        var filas = new List<string[]>
        {
            columnas.Select(c => c.Nombre).ToArray()
        };

        foreach (var entidad in await _catalogo.Listar(tabla))
        {
            filas.Add(columnas.Select(c => entidad.GetType().GetProperty(c.Nombre)?.GetValue(entidad)?.ToString() ?? string.Empty).ToArray());
        }

        var archivo = _exportador.Exportar(formato, filas);
        var nombre = $"{tabla}_{DateTime.Now:yyyyMMdd_HHmm}.{archivo.Extension}";
        return File(archivo.Contenido, archivo.TipoContenido, nombre);
    }

}