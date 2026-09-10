using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class FamiliaController : Controller
{
    private readonly FamiliarData _familiarData;
    private readonly RRHH_DinamicaFamiliarData _dinamicaData;
    private readonly RRHH_FuncFamData _funcFamData;
    private readonly EmpleadoData _empleadoData;
    private readonly PersonaData _personaData;
    private readonly TipoFamiliarData _tipoFamiliarData;
    private readonly RRHH_TipoFamiliaData _tipoFamiliaData;
    private readonly RRHH_RelacParejaData _relacParejaData;
    private readonly RRHH_RelacPHData _relacPHData;
    private readonly RRHH_RelacHermanoData _relacHermanoData;
    private readonly EstadoCivilData _estadoCivilData;
    private readonly DiscapacidadData _discapacidadData;
    private readonly TipoSangreData _tipoSangreData;

    public FamiliaController(
        FamiliarData familiarData,
        RRHH_DinamicaFamiliarData dinamicaData,
        RRHH_FuncFamData funcFamData,
        EmpleadoData empleadoData,
        PersonaData personaData,
        TipoFamiliarData tipoFamiliarData,
        RRHH_TipoFamiliaData tipoFamiliaData,
        RRHH_RelacParejaData relacParejaData,
        RRHH_RelacPHData relacPHData,
        RRHH_RelacHermanoData relacHermanoData,
        EstadoCivilData estadoCivilData,
        DiscapacidadData discapacidadData,
        TipoSangreData tipoSangreData)
    {
        _familiarData = familiarData;
        _dinamicaData = dinamicaData;
        _funcFamData = funcFamData;
        _empleadoData = empleadoData;
        _personaData = personaData;
        _tipoFamiliarData = tipoFamiliarData;
        _tipoFamiliaData = tipoFamiliaData;
        _relacParejaData = relacParejaData;
        _relacPHData = relacPHData;
        _relacHermanoData = relacHermanoData;
        _estadoCivilData = estadoCivilData;
        _discapacidadData = discapacidadData;
        _tipoSangreData = tipoSangreData;
    }

    // ---------- Familiares ----------

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        var familiares = (await _familiarData.Listar())
            .Where(f => idEmpleado is null || f.IdEmpleado == idEmpleado)
            .OrderBy(f => f.IdEmpleado)
            .ThenBy(f => f.IdPersona);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarData.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilData.Listar();
        ViewBag.Discapacidades = await _discapacidadData.Listar();
        ViewBag.TiposSangre = await _tipoSangreData.Listar();
        return View(familiares.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> FamiliarNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarData.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilData.Listar();
        ViewBag.Discapacidades = await _discapacidadData.Listar();
        ViewBag.TiposSangre = await _tipoSangreData.Listar();
        return View("FamiliarForm", new Familiar { IdEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FamiliarNuevo(Familiar modelo)
    {
        if (await _familiarData.Obtener(modelo.IdEmpleado, modelo.IdPersona) is not null)
            ModelState.AddModelError("", "Ese familiar ya está registrado para el empleado.");

        if (ModelState.IsValid)
        {
            await _familiarData.Crear(modelo);
            TempData["MensajeExito"] = "Familiar registrado.";
            return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
        }

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarData.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilData.Listar();
        ViewBag.Discapacidades = await _discapacidadData.Listar();
        ViewBag.TiposSangre = await _tipoSangreData.Listar();
        return View("FamiliarForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> FamiliarEditar(int idEmpleado, int idPersona)
    {
        var registro = await _familiarData.Obtener(idEmpleado, idPersona);
        if (registro is null)
            return NotFound();

        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.TiposFamiliar = await _tipoFamiliarData.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilData.Listar();
        ViewBag.Discapacidades = await _discapacidadData.Listar();
        ViewBag.TiposSangre = await _tipoSangreData.Listar();
        return View("FamiliarForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FamiliarEditar(Familiar modelo)
    {
        var actual = await _familiarData.Obtener(modelo.IdEmpleado, modelo.IdPersona);
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
        await _familiarData.Actualizar(actual);
        TempData["MensajeExito"] = "Familiar actualizado.";
        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FamiliarEliminar(int idEmpleado, int idPersona)
    {
        var actual = await _familiarData.Obtener(idEmpleado, idPersona);
        if (actual is not null)
        {
            await _familiarData.Eliminar(actual);
            TempData["MensajeExito"] = "Familiar eliminado.";
        }
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Dinámica familiar ----------

    [HttpGet]
    public async Task<IActionResult> Dinamica(int? idEmpleado)
    {
        var registros = (await _dinamicaData.Listar())
            .Where(d => idEmpleado is null || d.IdEmpleado == idEmpleado)
            .OrderByDescending(d => d.idDinamica);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaData.Listar();
        ViewBag.RelacPareja = await _relacParejaData.Listar();
        ViewBag.RelacPH = await _relacPHData.Listar();
        ViewBag.RelacHermano = await _relacHermanoData.Listar();
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> DinamicaNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaData.Listar();
        ViewBag.RelacPareja = await _relacParejaData.Listar();
        ViewBag.RelacPH = await _relacPHData.Listar();
        ViewBag.RelacHermano = await _relacHermanoData.Listar();
        return View("DinamicaForm", new RRHH_DinamicaFamiliar { IdEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DinamicaNuevo(RRHH_DinamicaFamiliar modelo)
    {
        if (ModelState.IsValid)
        {
            var lista = await _dinamicaData.Listar();
            modelo.idDinamica = lista.Count == 0 ? 1 : lista.Max(d => d.idDinamica) + 1;
            await _dinamicaData.Crear(modelo);
            TempData["MensajeExito"] = "Dinámica familiar guardada.";
            return RedirectToAction(nameof(Dinamica), new { idEmpleado = modelo.IdEmpleado });
        }

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaData.Listar();
        ViewBag.RelacPareja = await _relacParejaData.Listar();
        ViewBag.RelacPH = await _relacPHData.Listar();
        ViewBag.RelacHermano = await _relacHermanoData.Listar();
        return View("DinamicaForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> DinamicaEditar(int id)
    {
        var registro = await _dinamicaData.Obtener(id);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = registro.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.TiposFamilia = await _tipoFamiliaData.Listar();
        ViewBag.RelacPareja = await _relacParejaData.Listar();
        ViewBag.RelacPH = await _relacPHData.Listar();
        ViewBag.RelacHermano = await _relacHermanoData.Listar();
        return View("DinamicaForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DinamicaEditar(RRHH_DinamicaFamiliar modelo)
    {
        var actual = await _dinamicaData.Obtener(modelo.idDinamica);
        if (actual is null)
            return NotFound();

        actual.IdEmpleado = modelo.IdEmpleado;
        actual.idTipoFam = modelo.idTipoFam;
        actual.idRelaPareja = modelo.idRelaPareja;
        actual.idRelaPH = modelo.idRelaPH;
        actual.idRelacHermano = modelo.idRelacHermano;
        actual.Observacion = modelo.Observacion;
        await _dinamicaData.Actualizar(actual);
        TempData["MensajeExito"] = "Dinámica familiar actualizada.";
        return RedirectToAction(nameof(Dinamica), new { idEmpleado = actual.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DinamicaEliminar(int id)
    {
        var actual = await _dinamicaData.Obtener(id);
        if (actual is not null)
        {
            await _dinamicaData.Eliminar(actual);
            TempData["MensajeExito"] = "Dinámica familiar eliminada.";
        }
        return RedirectToAction(nameof(Dinamica), new { idEmpleado = actual?.IdEmpleado });
    }

    // ---------- Funcionalidad familiar ----------

    [HttpGet]
    public async Task<IActionResult> FuncFam(int? idEmpleado)
    {
        var registros = (await _funcFamData.Listar())
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
            var lista = await _funcFamData.Listar();
            modelo.idFuncFamiliar = lista.Count == 0 ? 1 : lista.Max(f => f.idFuncFamiliar) + 1;
            await _funcFamData.Crear(modelo);
            TempData["MensajeExito"] = "Funcionalidad familiar guardada.";
            return RedirectToAction(nameof(FuncFam), new { idEmpleado = modelo.IdEmpleado });
        }

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        return View("FuncFamForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> FuncFamEditar(int id)
    {
        var registro = await _funcFamData.Obtener(id);
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
        var actual = await _funcFamData.Obtener(modelo.idFuncFamiliar);
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
        await _funcFamData.Actualizar(actual);
        TempData["MensajeExito"] = "Funcionalidad familiar actualizada.";
        return RedirectToAction(nameof(FuncFam), new { idEmpleado = actual.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FuncFamEliminar(int id)
    {
        var actual = await _funcFamData.Obtener(id);
        if (actual is not null)
        {
            await _funcFamData.Eliminar(actual);
            TempData["MensajeExito"] = "Funcionalidad familiar eliminada.";
        }
        return RedirectToAction(nameof(FuncFam), new { idEmpleado = actual?.IdEmpleado });
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

    private async Task<List<(int Id, string Nombre)>> PersonasAsync()
    {
        var personas = await _personaData.Listar();
        return personas
            .Select(p => (p.IdPersona, $"{p.Nombres} {p.Apellido_Paterno} {p.Apellido_Materno}".Trim()))
            .OrderBy(x => x.Item2)
            .Select(x => (x.Item1, x.Item2))
            .ToList();
    }
}