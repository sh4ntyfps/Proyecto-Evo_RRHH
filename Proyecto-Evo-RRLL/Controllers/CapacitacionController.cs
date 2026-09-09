using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class CapacitacionController : Controller
{
    private readonly CapacitacionData _capacitacionData;
    private readonly EstudiosRealizadoData _estudiosData;
    private readonly TitulosEmpleadoData _titulosData;
    private readonly InstitucionData _institucionData;
    private readonly TipoEstudioData _tipoEstudioData;
    private readonly EstadoEstudioData _estadoEstudioData;
    private readonly EmpleadoData _empleadoData;
    private readonly PersonaData _personaData;
    private readonly TipoInstitucionData _tipoInstitucionData;

    public CapacitacionController(
        CapacitacionData capacitacionData,
        EstudiosRealizadoData estudiosData,
        TitulosEmpleadoData titulosData,
        InstitucionData institucionData,
        TipoEstudioData tipoEstudioData,
        EstadoEstudioData estadoEstudioData,
        EmpleadoData empleadoData,
        PersonaData personaData,
        TipoInstitucionData tipoInstitucionData)
    {
        _capacitacionData = capacitacionData;
        _estudiosData = estudiosData;
        _titulosData = titulosData;
        _institucionData = institucionData;
        _tipoEstudioData = tipoEstudioData;
        _estadoEstudioData = estadoEstudioData;
        _empleadoData = empleadoData;
        _personaData = personaData;
        _tipoInstitucionData = tipoInstitucionData;
    }

    // ---------- Cursos de capacitación ----------

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado, string? q)
    {
        var cursos = (await _capacitacionData.Listar())
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
        var lista = await _capacitacionData.Listar();
        modelo.IdCapacitacion = lista.Count > 0 ? lista.Max(x => x.IdCapacitacion) + 1 : 1;
        await _capacitacionData.Crear(modelo);
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var registro = await _capacitacionData.Obtener(id);
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

        var actual = await _capacitacionData.Obtener(id);
        if (actual is null)
            return NotFound();

        actual.NombreCurso = modelo.NombreCurso;
        actual.FechaInicio = modelo.FechaInicio;
        actual.FechaFin = modelo.FechaFin;
        actual.NumHoras = modelo.NumHoras;
        actual.InstitucionOrganizadora = modelo.InstitucionOrganizadora;
        actual.IdEmpleado = modelo.IdEmpleado;
        await _capacitacionData.Actualizar(actual);

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id, int? idEmpleado)
    {
        var actual = await _capacitacionData.Obtener(id);
        if (actual is not null)
            await _capacitacionData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Estudios realizados ----------

    [HttpGet]
    public async Task<IActionResult> Estudios(int? idEmpleado)
    {
        var estudios = (await _estudiosData.Listar())
            .Where(e => idEmpleado is null || e.IdEmpleado == idEmpleado)
            .OrderBy(e => e.IdTipoEstudios).ThenBy(e => e.Correlativo);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = idEmpleado is null ? null : await NombreEmpleadoAsync(idEmpleado.Value);
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposEstudio = await _tipoEstudioData.Listar();
        ViewBag.EstadosEstudio = await _estadoEstudioData.Listar();
        return View(estudios.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> EstudioNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposEstudio = await _tipoEstudioData.Listar();
        ViewBag.EstadosEstudio = await _estadoEstudioData.Listar();
        return View("EstudioForm", new EstudiosRealizado { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EstudioNuevo(EstudiosRealizado modelo)
    {
        var lista = await _estudiosData.Listar();
        var maxCorr = lista
            .Where(e => e.IdTipoEstudios == modelo.IdTipoEstudios && e.IdEmpleado == modelo.IdEmpleado)
            .Select(e => e.Correlativo)
            .DefaultIfEmpty(0)
            .Max();
        modelo.Correlativo = maxCorr + 1;
        await _estudiosData.Crear(modelo);
        return RedirectToAction(nameof(Estudios), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> EstudioEditar(int idTipo, int idEmpleado, int correlativo)
    {
        var registro = await _estudiosData.Obtener(idTipo, idEmpleado, correlativo);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposEstudio = await _tipoEstudioData.Listar();
        ViewBag.EstadosEstudio = await _estadoEstudioData.Listar();
        return View("EstudioForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EstudioEditar(EstudiosRealizado modelo)
    {
        var actual = await _estudiosData.Obtener(modelo.IdTipoEstudios, modelo.IdEmpleado, modelo.Correlativo);
        if (actual is null)
            return NotFound();

        actual.CentroEstudios = modelo.CentroEstudios;
        actual.AñoInicio = modelo.AñoInicio;
        actual.Especialidad = modelo.Especialidad;
        actual.AñoTermino = modelo.AñoTermino;
        actual.IdEstadoEstudio = modelo.IdEstadoEstudio;
        await _estudiosData.Actualizar(actual);

        return RedirectToAction(nameof(Estudios), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EstudioEliminar(int idTipo, int idEmpleado, int correlativo)
    {
        var actual = await _estudiosData.Obtener(idTipo, idEmpleado, correlativo);
        if (actual is not null)
            await _estudiosData.Eliminar(actual);
        return RedirectToAction(nameof(Estudios), new { idEmpleado });
    }

    // ---------- Títulos ----------

    [HttpGet]
    public async Task<IActionResult> Titulos(int? idEmpleado)
    {
        var titulos = (await _titulosData.Listar())
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
        await _titulosData.Crear(modelo);
        return RedirectToAction(nameof(Titulos), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> TituloEditar(string numColegiatura)
    {
        var registro = await _titulosData.Obtener(numColegiatura);
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
        var actual = await _titulosData.Obtener(numColegiatura);
        if (actual is null)
            return NotFound();

        actual.DenominacionGrado = modelo.DenominacionGrado;
        actual.Institucion = modelo.Institucion;
        actual.Fecha = modelo.Fecha;
        actual.IdEmpleado = modelo.IdEmpleado;
        await _titulosData.Actualizar(actual);

        return RedirectToAction(nameof(Titulos), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TituloEliminar(string numColegiatura, int? idEmpleado)
    {
        var actual = await _titulosData.Obtener(numColegiatura);
        if (actual is not null)
            await _titulosData.Eliminar(actual);
        return RedirectToAction(nameof(Titulos), new { idEmpleado });
    }

    // ---------- Instituciones ----------

    [HttpGet]
    public async Task<IActionResult> Instituciones()
    {
        var instituciones = (await _institucionData.Listar()).OrderBy(i => i.Descripcion);
        ViewBag.TiposInstitucion = await _tipoInstitucionData.Listar();
        return View(instituciones.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> InstitucionNuevo()
    {
        ViewBag.TiposInstitucion = await _tipoInstitucionData.Listar();
        return View("InstitucionForm", new Institucion());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InstitucionNuevo(Institucion modelo)
    {
        var lista = await _institucionData.Listar();
        modelo.IdInstitucion = lista.Count > 0 ? lista.Max(x => x.IdInstitucion) + 1 : 1;
        await _institucionData.Crear(modelo);
        return RedirectToAction(nameof(Instituciones));
    }

    [HttpGet]
    public async Task<IActionResult> InstitucionEditar(int id)
    {
        var registro = await _institucionData.Obtener(id);
        if (registro is null)
            return NotFound();

        ViewBag.TiposInstitucion = await _tipoInstitucionData.Listar();
        return View("InstitucionForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InstitucionEditar(int id, Institucion modelo)
    {
        var actual = await _institucionData.Obtener(id);
        if (actual is null)
            return NotFound();

        actual.Descripcion = modelo.Descripcion;
        actual.Direccion = modelo.Direccion;
        await _institucionData.Actualizar(actual);

        return RedirectToAction(nameof(Instituciones));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InstitucionEliminar(int id)
    {
        var actual = await _institucionData.Obtener(id);
        if (actual is not null)
            await _institucionData.Eliminar(actual);
        return RedirectToAction(nameof(Instituciones));
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