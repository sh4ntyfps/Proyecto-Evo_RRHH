using Capa_Logica;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class FamiliaController : Controller
{
    private readonly FamiliarLogica _familiarLogica;
    private readonly RRHH_DinamicaFamiliarLogica _dinamicaLogica;
    private readonly RRHH_FuncFamLogica _funcFamLogica;
    private readonly PersonaLogica _personaLogica;
    private readonly TipoFamiliarLogica _tipoFamiliarLogica;
    private readonly RRHH_TipoFamiliaLogica _tipoFamiliaLogica;
    private readonly RRHH_RelacParejaLogica _relacParejaLogica;
    private readonly RRHH_RelacPHLogica _relacPHLogica;
    private readonly RRHH_RelacHermanoLogica _relacHermanoLogica;
    private readonly EstadoCivilLogica _estadoCivilLogica;
    private readonly DiscapacidadLogica _discapacidadLogica;
    private readonly TipoSangreLogica _tipoSangreLogica;
    private readonly EmpleadoServicio _empleados;

    public FamiliaController(
        FamiliarLogica familiarLogica,
        RRHH_DinamicaFamiliarLogica dinamicaLogica,
        RRHH_FuncFamLogica funcFamLogica,
        PersonaLogica personaLogica,
        TipoFamiliarLogica tipoFamiliarLogica,
        RRHH_TipoFamiliaLogica tipoFamiliaLogica,
        RRHH_RelacParejaLogica relacParejaLogica,
        RRHH_RelacPHLogica relacPHLogica,
        RRHH_RelacHermanoLogica relacHermanoLogica,
        EstadoCivilLogica estadoCivilLogica,
        DiscapacidadLogica discapacidadLogica,
        TipoSangreLogica tipoSangreLogica,
        EmpleadoServicio empleados)
    {
        _familiarLogica = familiarLogica;
        _dinamicaLogica = dinamicaLogica;
        _funcFamLogica = funcFamLogica;
        _personaLogica = personaLogica;
        _tipoFamiliarLogica = tipoFamiliarLogica;
        _tipoFamiliaLogica = tipoFamiliaLogica;
        _relacParejaLogica = relacParejaLogica;
        _relacPHLogica = relacPHLogica;
        _relacHermanoLogica = relacHermanoLogica;
        _estadoCivilLogica = estadoCivilLogica;
        _discapacidadLogica = discapacidadLogica;
        _tipoSangreLogica = tipoSangreLogica;
        _empleados = empleados;
    }

    // ---------- Familiares ----------

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        var familiares = (await _familiarLogica.Listar())
            .Where(f => idEmpleado is null || f.IdEmpleado == idEmpleado)
            .OrderBy(f => f.IdEmpleado)
            .ThenBy(f => f.IdPersona);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarLogica.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilLogica.Listar();
        ViewBag.Discapacidades = await _discapacidadLogica.Listar();
        ViewBag.TiposSangre = await _tipoSangreLogica.Listar();
        return View(familiares.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> FamiliarNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarLogica.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilLogica.Listar();
        ViewBag.Discapacidades = await _discapacidadLogica.Listar();
        ViewBag.TiposSangre = await _tipoSangreLogica.Listar();
        return View("FamiliarForm", new Familiar { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FamiliarNuevo(Familiar modelo)
    {
        if (await _familiarLogica.Obtener(modelo.IdEmpleado, modelo.IdPersona) is not null)
            ModelState.AddModelError("", "Ese familiar ya está registrado para el empleado.");

        if (ModelState.IsValid)
        {
            await _familiarLogica.Crear(modelo);
            TempData["MensajeExito"] = "Familiar registrado.";
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
        }

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarLogica.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilLogica.Listar();
        ViewBag.Discapacidades = await _discapacidadLogica.Listar();
        ViewBag.TiposSangre = await _tipoSangreLogica.Listar();
        return View("FamiliarForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> FamiliarEditar(int idEmpleado, int idPersona)
    {
        var registro = await _familiarLogica.Obtener(idEmpleado, idPersona);
        if (registro is null)
            return NotFound();

        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarLogica.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilLogica.Listar();
        ViewBag.Discapacidades = await _discapacidadLogica.Listar();
        ViewBag.TiposSangre = await _tipoSangreLogica.Listar();
        return View("FamiliarForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FamiliarEditar(Familiar modelo)
    {
        var actual = await _familiarLogica.Obtener(modelo.IdEmpleado, modelo.IdPersona);
        if (actual is null)
            return NotFound();

        actual.FamSexo = modelo.FamSexo;
        actual.FamLugTrab = modelo.FamLugTrab;
        actual.IdEstadoCivil = modelo.IdEstadoCivil;
        actual.NumAutogenSalud = modelo.NumAutogenSalud;
        actual.FamOcupacion = modelo.FamOcupacion;
        actual.FamGradoInstruc = modelo.FamGradoInstruc;
        actual.IdTipoFam = modelo.IdTipoFam;
        actual.ViveCasa = modelo.ViveCasa;
        actual.LaboraInstitucion = modelo.LaboraInstitucion;
        actual.IdDiscapacidad = modelo.IdDiscapacidad;
        actual.Procedencia = modelo.Procedencia;
        actual.Enfermedad = modelo.Enfermedad;
        actual.TiempoEnfermedad = modelo.TiempoEnfermedad;
        actual.IdTipoSangre = modelo.IdTipoSangre;
        await _familiarLogica.Actualizar(actual);
        TempData["MensajeExito"] = "Familiar actualizado.";
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FamiliarEliminar(int idEmpleado, int idPersona)
    {
        var actual = await _familiarLogica.Obtener(idEmpleado, idPersona);
        if (actual is not null)
        {
            await _familiarLogica.Eliminar(actual);
            TempData["MensajeExito"] = "Familiar eliminado.";
        }
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Dinámica familiar ----------

    [HttpGet]
    public async Task<IActionResult> Dinamica(int? idEmpleado)
    {
        var registros = (await _dinamicaLogica.Listar())
            .Where(d => idEmpleado is null || d.IdEmpleado == idEmpleado)
            .OrderByDescending(d => d.idDinamica);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaLogica.Listar();
        ViewBag.RelacPareja = await _relacParejaLogica.Listar();
        ViewBag.RelacPH = await _relacPHLogica.Listar();
        ViewBag.RelacHermano = await _relacHermanoLogica.Listar();
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> DinamicaNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaLogica.Listar();
        ViewBag.RelacPareja = await _relacParejaLogica.Listar();
        ViewBag.RelacPH = await _relacPHLogica.Listar();
        ViewBag.RelacHermano = await _relacHermanoLogica.Listar();
        return View("DinamicaForm", new RRHH_DinamicaFamiliar { IdEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DinamicaNuevo(RRHH_DinamicaFamiliar modelo)
    {
        if (ModelState.IsValid)
        {
            var guardo = await _dinamicaLogica.Registrar(modelo);
            if (guardo)
            {
                TempData["MensajeExito"] = "Dinámica familiar guardada.";
                return RedirectToAction(nameof(Dinamica), new { idEmpleado = modelo.IdEmpleado });
            }
            ModelState.AddModelError("", "No se pudo guardar la dinámica familiar: intente nuevamente.");
        }

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaLogica.Listar();
        ViewBag.RelacPareja = await _relacParejaLogica.Listar();
        ViewBag.RelacPH = await _relacPHLogica.Listar();
        ViewBag.RelacHermano = await _relacHermanoLogica.Listar();
        return View("DinamicaForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> DinamicaEditar(int id)
    {
        var registro = await _dinamicaLogica.Obtener(id);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = registro.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaLogica.Listar();
        ViewBag.RelacPareja = await _relacParejaLogica.Listar();
        ViewBag.RelacPH = await _relacPHLogica.Listar();
        ViewBag.RelacHermano = await _relacHermanoLogica.Listar();
        return View("DinamicaForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DinamicaEditar(RRHH_DinamicaFamiliar modelo)
    {
        var actual = await _dinamicaLogica.Obtener(modelo.idDinamica);
        if (actual is null)
            return NotFound();

        actual.IdEmpleado = modelo.IdEmpleado;
        actual.idTipoFam = modelo.idTipoFam;
        actual.idRelaPareja = modelo.idRelaPareja;
        actual.idRelaPH = modelo.idRelaPH;
        actual.idRelacHermano = modelo.idRelacHermano;
        actual.Observacion = modelo.Observacion;
        await _dinamicaLogica.Actualizar(actual);
        TempData["MensajeExito"] = "Dinámica familiar actualizada.";
        return RedirectToAction(nameof(Dinamica), new { idEmpleado = actual.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DinamicaEliminar(int id)
    {
        var actual = await _dinamicaLogica.Obtener(id);
        if (actual is not null)
        {
            await _dinamicaLogica.Eliminar(actual);
            TempData["MensajeExito"] = "Dinámica familiar eliminada.";
        }
        return RedirectToAction(nameof(Dinamica), new { idEmpleado = actual?.IdEmpleado });
    }

    // ---------- Funcionalidad familiar ----------

    [HttpGet]
    public async Task<IActionResult> FuncFam(int? idEmpleado)
    {
        var registros = (await _funcFamLogica.Listar())
            .Where(f => idEmpleado is null || f.IdEmpleado == idEmpleado)
            .OrderByDescending(f => f.idFuncFamiliar);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> FuncFamNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("FuncFamForm", new RRHH_FuncFam { IdEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FuncFamNuevo(RRHH_FuncFam modelo)
    {
        if (ModelState.IsValid)
        {
            var guardo = await _funcFamLogica.Registrar(modelo);
            if (guardo)
            {
                TempData["MensajeExito"] = "Funcionalidad familiar guardada.";
                return RedirectToAction(nameof(FuncFam), new { idEmpleado = modelo.IdEmpleado });
            }
            ModelState.AddModelError("", "No se pudo guardar la funcionalidad familiar: intente nuevamente.");
        }

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("FuncFamForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> FuncFamEditar(int id)
    {
        var registro = await _funcFamLogica.Obtener(id);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = registro.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("FuncFamForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FuncFamEditar(RRHH_FuncFam modelo)
    {
        var actual = await _funcFamLogica.Obtener(modelo.idFuncFamiliar);
        if (actual is null)
            return NotFound();

        actual.IdEmpleado = modelo.IdEmpleado;
        actual.Com1 = modelo.Com1;
        actual.Com2 = modelo.Com2;
        actual.Afec1 = modelo.Afec1;
        actual.Afec2 = modelo.Afec2;
        actual.Decis1 = modelo.Decis1;
        actual.Decis2 = modelo.Decis2;
        actual.Soc1 = modelo.Soc1;
        actual.Soc2 = modelo.Soc2;
        actual.Soc3 = modelo.Soc3;
        await _funcFamLogica.Actualizar(actual);
        TempData["MensajeExito"] = "Funcionalidad familiar actualizada.";
        return RedirectToAction(nameof(FuncFam), new { idEmpleado = actual.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FuncFamEliminar(int id)
    {
        var actual = await _funcFamLogica.Obtener(id);
        if (actual is not null)
        {
            await _funcFamLogica.Eliminar(actual);
            TempData["MensajeExito"] = "Funcionalidad familiar eliminada.";
        }
        return RedirectToAction(nameof(FuncFam), new { idEmpleado = actual?.IdEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<List<(int Id, string Nombre)>> PersonasAsync()
    {
        var personas = await _personaLogica.Listar();
        return personas
            .Select(p => (p.IdPersona, p.NombreCompleto()))
            .OrderBy(x => x.Item2)
            .Select(x => (x.Item1, x.Item2))
            .ToList();
    }
}