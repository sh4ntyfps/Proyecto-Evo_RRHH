using Capa_Logica;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class CapacitacionController : Controller
{
    private readonly CapacitacionLogica _capacitacionLogica;
    private readonly EstudiosRealizadoLogica _estudiosLogica;
    private readonly TitulosEmpleadoLogica _titulosLogica;
    private readonly InstitucionLogica _institucionLogica;
    private readonly TipoEstudioLogica _tipoEstudioLogica;
    private readonly EstadoEstudioLogica _estadoEstudioLogica;
    private readonly TipoInstitucionLogica _tipoInstitucionLogica;
    private readonly EmpleadoServicio _empleados;

    public CapacitacionController(
        CapacitacionLogica capacitacionLogica,
        EstudiosRealizadoLogica estudiosLogica,
        TitulosEmpleadoLogica titulosLogica,
        InstitucionLogica institucionLogica,
        TipoEstudioLogica tipoEstudioLogica,
        EstadoEstudioLogica estadoEstudioLogica,
        TipoInstitucionLogica tipoInstitucionLogica,
        EmpleadoServicio empleados)
    {
        _capacitacionLogica = capacitacionLogica;
        _estudiosLogica = estudiosLogica;
        _titulosLogica = titulosLogica;
        _institucionLogica = institucionLogica;
        _tipoEstudioLogica = tipoEstudioLogica;
        _estadoEstudioLogica = estadoEstudioLogica;
        _tipoInstitucionLogica = tipoInstitucionLogica;
        _empleados = empleados;
    }

    // ---------- Cursos de capacitación ----------

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado, string? q)
    {
        var cursos = (await _capacitacionLogica.Listar())
            .Where(c => idEmpleado is null || c.IdEmpleado == idEmpleado)
            .OrderByDescending(c => c.FechaInicio);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Busqueda = q;
        return View(cursos.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> Nuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("Form", new Capacitacion { IdEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nuevo(Capacitacion modelo)
    {
        var guardo = await _capacitacionLogica.Registrar(modelo);
        if (guardo)
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ModelState.AddModelError("", "No se pudo registrar la capacitación: intente nuevamente.");
        return View("Form", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var registro = await _capacitacionLogica.Obtener(id);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = registro.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("Form", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Capacitacion modelo)
    {
        if (id != modelo.IdCapacitacion)
            return NotFound();

        var actual = await _capacitacionLogica.Obtener(id);
        if (actual is null)
            return NotFound();

        actual.NombreCurso = modelo.NombreCurso;
        actual.FechaInicio = modelo.FechaInicio;
        actual.FechaFin = modelo.FechaFin;
        actual.NumHoras = modelo.NumHoras;
        actual.InstitucionOrganizadora = modelo.InstitucionOrganizadora;
        actual.IdEmpleado = modelo.IdEmpleado;
        await _capacitacionLogica.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id, int? idEmpleado)
    {
        var actual = await _capacitacionLogica.Obtener(id);
        if (actual is not null)
            await _capacitacionLogica.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Estudios realizados ----------

    [HttpGet]
    public async Task<IActionResult> Estudios(int? idEmpleado)
    {
        var estudios = (await _estudiosLogica.Listar())
            .Where(e => idEmpleado is null || e.IdEmpleado == idEmpleado)
            .OrderBy(e => e.IdTipoEstudios).ThenBy(e => e.Correlativo);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposEstudio = await _tipoEstudioLogica.Listar();
        ViewBag.EstadosEstudio = await _estadoEstudioLogica.Listar();
        return View(estudios.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> EstudioNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposEstudio = await _tipoEstudioLogica.Listar();
        ViewBag.EstadosEstudio = await _estadoEstudioLogica.Listar();
        return View("EstudioForm", new EstudiosRealizado { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EstudioNuevo(EstudiosRealizado modelo)
    {
        var guardo = await _estudiosLogica.Registrar(modelo);
        if (guardo)
            return RedirectToAction(nameof(Estudios), new { idEmpleado = modelo.IdEmpleado });

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposEstudio = await _tipoEstudioLogica.Listar();
        ViewBag.EstadosEstudio = await _estadoEstudioLogica.Listar();
        ModelState.AddModelError("", "No se pudo registrar el estudio: intente nuevamente.");
        return View("EstudioForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> EstudioEditar(int idTipo, int idEmpleado, int correlativo)
    {
        var registro = await _estudiosLogica.Obtener(idTipo, idEmpleado, correlativo);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposEstudio = await _tipoEstudioLogica.Listar();
        ViewBag.EstadosEstudio = await _estadoEstudioLogica.Listar();
        return View("EstudioForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EstudioEditar(EstudiosRealizado modelo)
    {
        var actual = await _estudiosLogica.Obtener(modelo.IdTipoEstudios, modelo.IdEmpleado, modelo.Correlativo);
        if (actual is null)
            return NotFound();

        actual.CentroEstudios = modelo.CentroEstudios;
        actual.AñoInicio = modelo.AñoInicio;
        actual.Especialidad = modelo.Especialidad;
        actual.AñoTermino = modelo.AñoTermino;
        actual.IdEstadoEstudio = modelo.IdEstadoEstudio;
        await _estudiosLogica.Actualizar(actual);

        return RedirectToAction(nameof(Estudios), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EstudioEliminar(int idTipo, int idEmpleado, int correlativo)
    {
        var actual = await _estudiosLogica.Obtener(idTipo, idEmpleado, correlativo);
        if (actual is not null)
            await _estudiosLogica.Eliminar(actual);
        return RedirectToAction(nameof(Estudios), new { idEmpleado });
    }

    // ---------- Títulos ----------

    [HttpGet]
    public async Task<IActionResult> Titulos(int? idEmpleado)
    {
        var titulos = (await _titulosLogica.Listar())
            .Where(t => idEmpleado is null || t.IdEmpleado == idEmpleado)
            .OrderBy(t => t.NumColegiatura);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        return View(titulos.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> TituloNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("TituloForm", new TitulosEmpleado { IdEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TituloNuevo(TitulosEmpleado modelo)
    {
        await _titulosLogica.Crear(modelo);
        return RedirectToAction(nameof(Titulos), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> TituloEditar(string numColegiatura)
    {
        var registro = await _titulosLogica.Obtener(numColegiatura);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = registro.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("TituloForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TituloEditar(string numColegiatura, TitulosEmpleado modelo)
    {
        var actual = await _titulosLogica.Obtener(numColegiatura);
        if (actual is null)
            return NotFound();

        actual.DenominacionGrado = modelo.DenominacionGrado;
        actual.Institucion = modelo.Institucion;
        actual.Fecha = modelo.Fecha;
        actual.IdEmpleado = modelo.IdEmpleado;
        await _titulosLogica.Actualizar(actual);

        return RedirectToAction(nameof(Titulos), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TituloEliminar(string numColegiatura, int? idEmpleado)
    {
        var actual = await _titulosLogica.Obtener(numColegiatura);
        if (actual is not null)
            await _titulosLogica.Eliminar(actual);
        return RedirectToAction(nameof(Titulos), new { idEmpleado });
    }

    // ---------- Instituciones ----------

    [HttpGet]
    public async Task<IActionResult> Instituciones()
    {
        var instituciones = (await _institucionLogica.Listar()).OrderBy(i => i.Descripcion);
        ViewBag.TiposInstitucion = await _tipoInstitucionLogica.Listar();
        return View(instituciones.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> InstitucionNuevo()
    {
        ViewBag.TiposInstitucion = await _tipoInstitucionLogica.Listar();
        return View("InstitucionForm", new Institucion());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InstitucionNuevo(Institucion modelo)
    {
        var guardo = await _institucionLogica.Registrar(modelo);
        if (guardo)
            return RedirectToAction(nameof(Instituciones));

        ViewBag.TiposInstitucion = await _tipoInstitucionLogica.Listar();
        ModelState.AddModelError("", "No se pudo registrar la institución: intente nuevamente.");
        return View("InstitucionForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> InstitucionEditar(int id)
    {
        var registro = await _institucionLogica.Obtener(id);
        if (registro is null)
            return NotFound();

        ViewBag.TiposInstitucion = await _tipoInstitucionLogica.Listar();
        return View("InstitucionForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InstitucionEditar(int id, Institucion modelo)
    {
        var actual = await _institucionLogica.Obtener(id);
        if (actual is null)
            return NotFound();

        actual.Descripcion = modelo.Descripcion;
        actual.Direccion = modelo.Direccion;
        await _institucionLogica.Actualizar(actual);

        return RedirectToAction(nameof(Instituciones));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InstitucionEliminar(int id)
    {
        var actual = await _institucionLogica.Obtener(id);
        if (actual is not null)
            await _institucionLogica.Eliminar(actual);
        return RedirectToAction(nameof(Instituciones));
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => await _empleados.NombreEmpleadoAsync(idEmpleado);
}