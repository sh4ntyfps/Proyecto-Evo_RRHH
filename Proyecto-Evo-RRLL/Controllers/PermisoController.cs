using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class PermisoController : Controller
{
    private readonly PermisoData _permisoData;
    private readonly Motivo_PermData _motivoData;
    private readonly EmpleadoData _empleadoData;
    private readonly PersonaData _personaData;

    public PermisoController(
        PermisoData permisoData,
        Motivo_PermData motivoData,
        EmpleadoData empleadoData,
        PersonaData personaData)
    {
        _permisoData = permisoData;
        _motivoData = motivoData;
        _empleadoData = empleadoData;
        _personaData = personaData;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        var permisos = (await _permisoData.Listar())
            .Where(p => idEmpleado is null || p.IdEmpleado == idEmpleado)
            .OrderByDescending(p => p.FechaInicio);

        var motivos = await _motivoData.Listar();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Motivos = motivos;
        return View(permisos.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Motivos = await _motivoData.Listar();
        return View("Form", new Permiso { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(Permiso modelo)
    {
        var lista = await _permisoData.Listar();
        var maxN = lista
            .Where(p => p.IdEmpleado == modelo.IdEmpleado)
            .Select(p => p.NPermiso)
            .DefaultIfEmpty(0)
            .Max();
        modelo.NPermiso = maxN + 1;
        await _permisoData.Crear(modelo);
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int idEmpleado, int npermiso)
    {
        var registro = await _permisoData.Obtener(idEmpleado, npermiso);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Motivos = await _motivoData.Listar();
        return View("Form", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(Permiso modelo)
    {
        var actual = await _permisoData.Obtener(modelo.IdEmpleado, modelo.NPermiso);
        if (actual is null)
            return NotFound();

        actual.FechaInicio = modelo.FechaInicio;
        actual.FechaFin = modelo.FechaFin;
        actual.Hora_Sal = modelo.Hora_Sal;
        actual.Hora_Ret = modelo.Hora_Ret;
        actual.Dia = modelo.Dia;
        actual.Retorno = modelo.Retorno;
        actual.idMotivo = modelo.idMotivo;
        actual.Lugar = modelo.Lugar;
        actual.Referencia = modelo.Referencia;
        actual.Autorizacion = modelo.Autorizacion;
        actual.Obs = modelo.Obs;
        actual.AutorizacionRRHH = modelo.AutorizacionRRHH;
        actual.Autorizado = modelo.Autorizado;
        await _permisoData.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int idEmpleado, int npermiso)
    {
        var actual = await _permisoData.Obtener(idEmpleado, npermiso);
        if (actual is not null)
            await _permisoData.Eliminar(actual);
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