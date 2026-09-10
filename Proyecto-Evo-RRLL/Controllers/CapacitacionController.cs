using Capa_Datos;
using Capa_Entidades;
using Capa_Logica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class CapacitacionController : Controller
{
    private readonly CapacitacionLogica _capLogica;
    private readonly EmpleadoData _empleadoData;
    private readonly PersonaData _personaData;

    public CapacitacionController(
        CapacitacionLogica capLogica,
        EmpleadoData empleadoData,
        PersonaData personaData)
    {
        _capLogica = capLogica;
        _empleadoData = empleadoData;
        _personaData = personaData;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 20;

        var totalCount = await _capLogica.CountAsync();
        var lista = await _capLogica.ListarPaged(page, pageSize);

        // Load only empleados referenced in this page
        var empleadoIds = lista.Where(c => c.IdEmpleado is not null).Select(c => c.IdEmpleado!.Value).Distinct().ToList();
        var empleados = empleadoIds.Count > 0 ? await _empleadoData.ListarPorIds(empleadoIds) : new List<Capa_Entidades.Empleado>();

        var personaIds = empleados.Where(e => e.IdPersona is not null).Select(e => e.IdPersona!.Value).Distinct().ToList();
        var personas = personaIds.Count > 0 ? await _personaData.ListarPorIds(personaIds) : new List<Capa_Entidades.Persona>();

        var empleadosById = empleados.ToDictionary(e => e.IdEmpleado);
        var personasById = personas.ToDictionary(p => p.IdPersona);

        var nombres = new Dictionary<int, string?>();
        foreach (var c in lista)
        {
            if (c.IdEmpleado is null)
            {
                nombres[c.IdCapacitacion] = null;
                continue;
            }

            if (!empleadosById.TryGetValue(c.IdEmpleado.Value, out var empleado))
            {
                nombres[c.IdCapacitacion] = $"(Empleado {c.IdEmpleado})";
                continue;
            }

            if (empleado.IdPersona is null || !personasById.TryGetValue(empleado.IdPersona.Value, out var persona))
            {
                nombres[c.IdCapacitacion] = $"(Empleado {c.IdEmpleado})";
                continue;
            }

            nombres[c.IdCapacitacion] = $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
        }

        ViewBag.Nombres = nombres;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ViewBag.PageSize = pageSize;
        ViewBag.TotalCount = totalCount;

        return View(lista);
    }

    [HttpGet]
    public async Task<IActionResult> Detalles(int id)
    {
        var cap = await _capLogica.Obtener(id);
        if (cap is null)
            return NotFound();
        ViewBag.NombreEmpleado = cap.IdEmpleado is null ? null : await NombreEmpleadoAsync(cap.IdEmpleado.Value);
        return View(cap);
    }

    [HttpGet]
    public async Task<IActionResult> CapacitacionForm(int? id)
    {
        Capacitacion modelo;
        if (id is null || id == 0)
            modelo = new Capacitacion();
        else
            modelo = await _capLogica.Obtener(id.Value) ?? new Capacitacion();

        // Build employee select list using batch loads to avoid repeated Persona queries
        var empleados = await _empleadoData.Listar();
        var personas = await _personaData.Listar();
        var personasById = personas.ToDictionary(p => p.IdPersona);

        var opciones = empleados.Select(e =>
        {
            string text;
            if (e.IdPersona is null || !personasById.TryGetValue(e.IdPersona.Value, out var persona))
                text = $"Empleado {e.IdEmpleado}";
            else
                text = $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
            return new SelectListItem { Value = e.IdEmpleado.ToString(), Text = text };
        }).ToList();

        ViewBag.Empleados = opciones;
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CapacitacionForm(Capacitacion modelo)
    {
        if (!ModelState.IsValid)
            return View(modelo);

        if (modelo.IdCapacitacion == 0)
        {
            var lista = await _capLogica.Listar();
            modelo.IdCapacitacion = lista.Count > 0 ? lista.Max(x => x.IdCapacitacion) + 1 : 1;
            await _capLogica.Crear(modelo);
        }
        else
        {
            var actual = await _capLogica.Obtener(modelo.IdCapacitacion);
            if (actual is null)
                return NotFound();
            actual.NombreCurso = modelo.NombreCurso;
            actual.FechaInicio = modelo.FechaInicio;
            actual.FechaFin = modelo.FechaFin;
            actual.NumHoras = modelo.NumHoras;
            actual.InstitucionOrganizadora = modelo.InstitucionOrganizadora;
            actual.IdEmpleado = modelo.IdEmpleado;
            await _capLogica.Actualizar(actual);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var actual = await _capLogica.Obtener(id);
        if (actual is not null)
            await _capLogica.Eliminar(actual);
        return RedirectToAction(nameof(Index));
    }

    // ---------- Utilidades ----------
    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
    {
        var empleado = await _empleadoData.Obtener(idEmpleado);
        if (empleado is null)
            return null;
        if (empleado.IdPersona is null)
            return $"(Empleado {idEmpleado})";

        var persona = await _personaData.Obtener(empleado.IdPersona.Value);
        if (persona is null)
            return $"(Empleado {idEmpleado})";

        return $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
    }
}
