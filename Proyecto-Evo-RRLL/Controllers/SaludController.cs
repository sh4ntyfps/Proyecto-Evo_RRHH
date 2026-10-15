using Capa_Logica;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class SaludController : Controller
{
    private readonly RRHH_SaludLogica _saludLogica;
    private readonly RRHH_AseguradoLogica _aseguradoLogica;
    private readonly RRHH_AcudeEnfermLogica _acudeEnfermLogica;
    private readonly PersonaLogica _personaLogica;
    private readonly EstadoCivilLogica _estadoCivilLogica;
    private readonly MotivoBajaLogica _motivoBajaLogica;
    private readonly EmpleadoServicio _empleados;

    public SaludController(
        RRHH_SaludLogica saludLogica,
        RRHH_AseguradoLogica aseguradoLogica,
        RRHH_AcudeEnfermLogica acudeEnfermLogica,
        PersonaLogica personaLogica,
        EstadoCivilLogica estadoCivilLogica,
        MotivoBajaLogica motivoBajaLogica,
        EmpleadoServicio empleados)
    {
        _saludLogica = saludLogica;
        _aseguradoLogica = aseguradoLogica;
        _acudeEnfermLogica = acudeEnfermLogica;
        _personaLogica = personaLogica;
        _estadoCivilLogica = estadoCivilLogica;
        _motivoBajaLogica = motivoBajaLogica;
        _empleados = empleados;
    }

    // ---------- Ficha de salud ----------

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        var fichas = (await _saludLogica.Listar())
            .Where(f => idEmpleado is null || f.IdEmpleado == idEmpleado)
            .OrderByDescending(f => f.idSalud);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.AcudeEnferm = await _acudeEnfermLogica.Listar();
        return View(fichas.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> SaludNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.AcudeEnferm = await _acudeEnfermLogica.Listar();
        return View("SaludForm", new RRHH_Salud { IdEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaludNuevo(RRHH_Salud modelo)
    {
        if (ModelState.IsValid)
        {
            var guardo = await _saludLogica.Registrar(modelo);
            if (guardo)
            {
                TempData["MensajeExito"] = "Ficha de salud guardada.";
                return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
            }
            ModelState.AddModelError("", "No se pudo guardar la ficha de salud: intente nuevamente.");
        }

        ViewBag.IdEmpleado = modelo.IdEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.AcudeEnferm = await _acudeEnfermLogica.Listar();
        return View("SaludForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> SaludEditar(int id)
    {
        var ficha = await _saludLogica.Obtener(id);
        if (ficha is null)
            return NotFound();

        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.AcudeEnferm = await _acudeEnfermLogica.Listar();
        return View("SaludForm", ficha);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaludEditar(RRHH_Salud modelo)
    {
        var actual = await _saludLogica.Obtener(modelo.idSalud);
        if (actual is null)
            return NotFound();

        actual.IdEmpleado = modelo.IdEmpleado;
        actual.EnferCronica = modelo.EnferCronica;
        actual.EnferCronicaDescrip = modelo.EnferCronicaDescrip;
        actual.FamDiscapacidad = modelo.FamDiscapacidad;
        actual.FamDiscapDescrip = modelo.FamDiscapDescrip;
        actual.Alergico = modelo.Alergico;
        actual.AlergicoDescrip = modelo.AlergicoDescrip;
        actual.idAcudeEnferm = modelo.idAcudeEnferm;
        await _saludLogica.Actualizar(actual);
        TempData["MensajeExito"] = "Ficha de salud actualizada.";
        return RedirectToAction(nameof(Index), new { idEmpleado = actual.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaludEliminar(int id)
    {
        var actual = await _saludLogica.Obtener(id);
        if (actual is not null)
        {
            await _saludLogica.Eliminar(actual);
            TempData["MensajeExito"] = "Ficha de salud eliminada.";
        }
        return RedirectToAction(nameof(Index), new { idEmpleado = actual?.IdEmpleado });
    }

    // ---------- Asegurados (beneficiarios) ----------

    [HttpGet]
    public async Task<IActionResult> Asegurados(int? idEmpleado)
    {
        var registros = (await _aseguradoLogica.Listar())
            .Where(a => idEmpleado is null || a.idEmpleado == idEmpleado)
            .OrderBy(a => a.idEmpleado)
            .ThenBy(a => a.NroBeneficiario);

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        return View(registros.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> AseguradoNuevo(int? idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.EstadosCiviles = await _estadoCivilLogica.Listar();
        ViewBag.MotivosBaja = await _motivoBajaLogica.Listar();
        return View("AseguradoForm", new RRHH_Asegurado { idEmpleado = idEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AseguradoNuevo(RRHH_Asegurado modelo)
    {
        var guardo = await _aseguradoLogica.Registrar(modelo);
        if (!guardo)
            ModelState.AddModelError("", "No se pudo agregar el beneficiario: intente nuevamente.");
        else
        {
            TempData["MensajeExito"] = "Beneficiario agregado.";
            return RedirectToAction(nameof(Asegurados), new { idEmpleado = modelo.idEmpleado });
        }

        ViewBag.IdEmpleado = modelo.idEmpleado;
        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.EstadosCiviles = await _estadoCivilLogica.Listar();
        ViewBag.MotivosBaja = await _motivoBajaLogica.Listar();
        return View("AseguradoForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> AseguradoEditar(int idEmpleado, int nroBeneficiario)
    {
        var registro = await _aseguradoLogica.Obtener(idEmpleado, nroBeneficiario);
        if (registro is null)
            return NotFound();

        ViewBag.Empleados = await EmpleadosAsync();
        ViewBag.Personas = await PersonasAsync();
        ViewBag.EstadosCiviles = await _estadoCivilLogica.Listar();
        ViewBag.MotivosBaja = await _motivoBajaLogica.Listar();
        return View("AseguradoForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AseguradoEditar(RRHH_Asegurado modelo)
    {
        var actual = await _aseguradoLogica.Obtener(modelo.idEmpleado, modelo.NroBeneficiario);
        if (actual is null)
            return NotFound();

        actual.idPersona = modelo.idPersona;
        actual.idEstadoCivil = modelo.idEstadoCivil;
        actual.Sexo = modelo.Sexo;
        actual.VinculoFamiliar = modelo.VinculoFamiliar;
        actual.NroPartida = modelo.NroPartida;
        actual.NroResolMayorIndiscap = modelo.NroResolMayorIndiscap;
        actual.FechaInicioVinculo = modelo.FechaInicioVinculo;
        actual.idMotivoBaja = modelo.idMotivoBaja;
        actual.NroPartidaDef = modelo.NroPartidaDef;
        actual.OtrosMotivos = modelo.OtrosMotivos;
        actual.FechaFinVinculo = modelo.FechaFinVinculo;
        actual.Reevaluar = modelo.Reevaluar;
        await _aseguradoLogica.Actualizar(actual);
        TempData["MensajeExito"] = "Beneficiario actualizado.";
        return RedirectToAction(nameof(Asegurados), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AseguradoEliminar(int idEmpleado, int nroBeneficiario)
    {
        var actual = await _aseguradoLogica.Obtener(idEmpleado, nroBeneficiario);
        if (actual is not null)
        {
            await _aseguradoLogica.Eliminar(actual);
            TempData["MensajeExito"] = "Beneficiario eliminado.";
        }
        return RedirectToAction(nameof(Asegurados), new { idEmpleado });
    }

    // ---------- Catálogo: a quién acudir ----------

    [HttpGet]
    public async Task<IActionResult> AcudeEnferm()
    {
        ViewBag.Empleados = await EmpleadosAsync();
        return View((await _acudeEnfermLogica.Listar()).OrderBy(a => a.idAcudeEnferm).ToList());
    }

    [HttpGet]
    public IActionResult AcudeEnfermNuevo()
    {
        return View("AcudeEnfermForm", new RRHH_AcudeEnferm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcudeEnfermNuevo(RRHH_AcudeEnferm modelo)
    {
        if (ModelState.IsValid)
        {
            var guardo = await _acudeEnfermLogica.Registrar(modelo);
            if (guardo)
            {
                TempData["MensajeExito"] = "Opción agregada.";
                return RedirectToAction(nameof(AcudeEnferm));
            }
            ModelState.AddModelError("", "No se pudo agregar la opción: intente nuevamente.");
        }
        return View("AcudeEnfermForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> AcudeEnfermEditar(int id)
    {
        var registro = await _acudeEnfermLogica.Obtener(id);
        if (registro is null)
            return NotFound();
        return View("AcudeEnfermForm", registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcudeEnfermEditar(RRHH_AcudeEnferm modelo)
    {
        var actual = await _acudeEnfermLogica.Obtener(modelo.idAcudeEnferm);
        if (actual is null)
            return NotFound();

        actual.AcudeEnfermDescrip = modelo.AcudeEnfermDescrip;
        await _acudeEnfermLogica.Actualizar(actual);
        TempData["MensajeExito"] = "Opción actualizada.";
        return RedirectToAction(nameof(AcudeEnferm));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcudeEnfermEliminar(int id)
    {
        var actual = await _acudeEnfermLogica.Obtener(id);
        if (actual is not null)
        {
            var enUso = (await _saludLogica.Listar()).Any(s => s.idAcudeEnferm == id);
            if (enUso)
                TempData["MensajeError"] = "No se puede eliminar: la opción está en uso por fichas de salud.";
            else
            {
                await _acudeEnfermLogica.Eliminar(actual);
                TempData["MensajeExito"] = "Opción eliminada.";
            }
        }
        return RedirectToAction(nameof(AcudeEnferm));
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<List<(int Id, string Nombre)>> PersonasAsync()
    {
        var personas = await _personaLogica.Listar();
        return personas.Select(p => (p.IdPersona, p.NombreCompleto()))
            .OrderBy(x => x.Item2)
            .Select(x => (x.Item1, x.Item2))
            .ToList();
    }
}