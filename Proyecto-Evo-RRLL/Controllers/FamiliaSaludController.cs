using Capa_Logica;
using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class FamiliaSaludController : Controller
{
    private readonly EmpleadoLogica _empleadoLogica;
    private readonly PersonaLogica _personaLogica;
    private readonly FamiliarLogica _familiarLogica;
    private readonly RRHH_SaludLogica _saludLogica;
    private readonly RRHH_DinamicaFamiliarLogica _dinamicaLogica;
    private readonly RRHH_AspSocioLogica _aspSocioLogica;
    private readonly RRHH_FuncFamLogica _funcFamLogica;
    private readonly RRHH_AseguradoLogica _aseguradoLogica;

    private readonly RRHH_AcudeEnfermLogica _acudeEnfermLogica;
    private readonly RRHH_TipoFamiliaLogica _tipoFamiliaLogica;
    private readonly RRHH_RelacParejaLogica _relacParejaLogica;
    private readonly RRHH_RelacPHLogica _relacPHLogica;
    private readonly RRHH_RelacHermanoLogica _relacHermanoLogica;
    private readonly TipoFamiliarLogica _tipoFamiliarLogica;
    private readonly EstadoCivilLogica _estadoCivilLogica;
    private readonly TipoSangreLogica _tipoSangreLogica;
    private readonly DiscapacidadLogica _discapacidadLogica;
    private readonly SecuenciaService _secuencia;
    private readonly EmpleadoServicio _empleados;

    public FamiliaSaludController(
        EmpleadoLogica empleadoLogica,
        PersonaLogica personaLogica,
        FamiliarLogica familiarLogica,
        RRHH_SaludLogica saludLogica,
        RRHH_DinamicaFamiliarLogica dinamicaLogica,
        RRHH_AspSocioLogica aspSocioLogica,
        RRHH_FuncFamLogica funcFamLogica,
        RRHH_AseguradoLogica aseguradoLogica,
        RRHH_AcudeEnfermLogica acudeEnfermLogica,
        RRHH_TipoFamiliaLogica tipoFamiliaLogica,
        RRHH_RelacParejaLogica relacParejaLogica,
        RRHH_RelacPHLogica relacPHLogica,
        RRHH_RelacHermanoLogica relacHermanoLogica,
        TipoFamiliarLogica tipoFamiliarLogica,
        EstadoCivilLogica estadoCivilLogica,
        TipoSangreLogica tipoSangreLogica,
        DiscapacidadLogica discapacidadLogica,
        SecuenciaService secuencia,
        EmpleadoServicio empleados)
    {
        _empleadoLogica = empleadoLogica;
        _personaLogica = personaLogica;
        _familiarLogica = familiarLogica;
        _saludLogica = saludLogica;
        _dinamicaLogica = dinamicaLogica;
        _aspSocioLogica = aspSocioLogica;
        _funcFamLogica = funcFamLogica;
        _aseguradoLogica = aseguradoLogica;
        _acudeEnfermLogica = acudeEnfermLogica;
        _tipoFamiliaLogica = tipoFamiliaLogica;
        _relacParejaLogica = relacParejaLogica;
        _relacPHLogica = relacPHLogica;
        _relacHermanoLogica = relacHermanoLogica;
        _tipoFamiliarLogica = tipoFamiliarLogica;
        _estadoCivilLogica = estadoCivilLogica;
        _tipoSangreLogica = tipoSangreLogica;
        _discapacidadLogica = discapacidadLogica;
        _secuencia = secuencia;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int idEmpleado)
    {
        if (await _empleadoLogica.Obtener(idEmpleado) is null)
            return NotFound();

        var salud = (await _saludLogica.Listar()).FirstOrDefault(s => s.IdEmpleado == idEmpleado);
        var dinamica = (await _dinamicaLogica.Listar()).FirstOrDefault(d => d.IdEmpleado == idEmpleado);
        var aspSocio = (await _aspSocioLogica.Listar()).FirstOrDefault(a => a.IdEmpleado == idEmpleado);
        var funcFam = (await _funcFamLogica.Listar()).FirstOrDefault(f => f.IdEmpleado == idEmpleado);
        var familiares = (await _familiarLogica.Listar()).Count(f => f.IdEmpleado == idEmpleado);
        var asegurados = (await _aseguradoLogica.Listar()).Count(a => a.idEmpleado == idEmpleado);

        return View(new FamiliaSaludViewModel
        {
            IdEmpleado = idEmpleado,
            NombreEmpleado = await NombreEmpleadoAsync(idEmpleado),
            TieneSalud = salud is not null,
            TieneDinamica = dinamica is not null,
            TieneAspSocio = aspSocio is not null,
            TieneFuncFam = funcFam is not null,
            Familiares = familiares,
            Asegurados = asegurados
        });
    }

    // ---------- Familiares (solo lectura) ----------

    [HttpGet]
    public async Task<IActionResult> Familiares(int idEmpleado)
    {
        var familiarRows = (await _familiarLogica.Listar()).Where(f => f.IdEmpleado == idEmpleado);
        var personas = await _personaLogica.Listar();
        var tipoFams = await _tipoFamiliarLogica.Listar();
        var estadoCiviles = await _estadoCivilLogica.Listar();
        var discapacidades = await _discapacidadLogica.Listar();
        var tipoSangres = await _tipoSangreLogica.Listar();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);

        var filas = familiarRows.Select(f =>
        {
            var persona = personas.FirstOrDefault(p => p.IdPersona == f.IdPersona);
            return new FamiliarFila
            {
                IdEmpleado = f.IdEmpleado,
                IdPersona = f.IdPersona,
                Nombres = persona is null ? null : persona.NombreCompleto(),
                TipoFamiliar = f.IdTipoFam is null ? null : tipoFams.FirstOrDefault(t => t.IdTipoFam == f.IdTipoFam)?.DescripTipoFam,
                Sexo = f.FamSexo,
                Ocupacion = f.FamOcupacion,
                EstadoCivil = f.IdEstadoCivil is null ? null : estadoCiviles.FirstOrDefault(e => e.IdEstadoCivil == f.IdEstadoCivil)?.DescripEstCivil,
                ViveCasa = f.ViveCasa == true ? "Sí" : f.ViveCasa == false ? "No" : null,
                LaboraInstitucion = f.LaboraInstitucion == true ? "Sí" : f.LaboraInstitucion == false ? "No" : null,
                Discapacidad = f.IdDiscapacidad is null ? null : discapacidades.FirstOrDefault(d => d.IdDiscapacidad == f.IdDiscapacidad)?.Descrip_Discapacidad,
                TipoSangre = f.IdTipoSangre is null ? null : tipoSangres.FirstOrDefault(t => t.IdTipoSangre == f.IdTipoSangre)?.DescripTS
            };
        }).ToList();

        return View(filas);
    }

    // ---------- Salud ----------

    [HttpGet]
    public async Task<IActionResult> Salud(int idEmpleado)
    {
        if (await _empleadoLogica.Obtener(idEmpleado) is null)
            return NotFound();

        var registro = (await _saludLogica.Listar()).FirstOrDefault(s => s.IdEmpleado == idEmpleado) ?? new RRHH_Salud();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);
        ViewBag.Acudes = await _acudeEnfermLogica.Listar();
        return View(registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Salud(RRHH_Salud modelo)
    {
        var actual = (await _saludLogica.Listar()).FirstOrDefault(s => s.IdEmpleado == modelo.IdEmpleado);

        if (actual is null)
        {
            var guardo = await _saludLogica.Registrar(modelo);
            if (!guardo)
            {
                TempData["MensajeError"] = "No se pudo guardar la ficha de salud: intente nuevamente.";
                return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado ?? 0 });
            }
        }
        else
        {
            actual.EnferCronica = modelo.EnferCronica;
            actual.EnferCronicaDescrip = modelo.EnferCronicaDescrip;
            actual.FamDiscapacidad = modelo.FamDiscapacidad;
            actual.FamDiscapDescrip = modelo.FamDiscapDescrip;
            actual.Alergico = modelo.Alergico;
            actual.AlergicoDescrip = modelo.AlergicoDescrip;
            actual.idAcudeEnferm = modelo.idAcudeEnferm;
            await _saludLogica.Actualizar(actual);
        }

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarSalud(int idEmpleado)
    {
        var actual = (await _saludLogica.Listar()).FirstOrDefault(s => s.IdEmpleado == idEmpleado);
        if (actual is not null)
            await _saludLogica.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Ficha social (dinámica + aspecto + funcionalidad) ----------

    [HttpGet]
    public async Task<IActionResult> FichaSocial(int idEmpleado)
    {
        var vm = new FichaSocialViewModel { IdEmpleado = idEmpleado, NombreEmpleado = await NombreEmpleadoAsync(idEmpleado) };

        var dinamica = (await _dinamicaLogica.Listar()).FirstOrDefault(d => d.IdEmpleado == idEmpleado);
        var aspSocio = (await _aspSocioLogica.Listar()).FirstOrDefault(a => a.IdEmpleado == idEmpleado);
        var funcFam = (await _funcFamLogica.Listar()).FirstOrDefault(f => f.IdEmpleado == idEmpleado);

        if (dinamica is not null) vm.Dinamica = dinamica;
        if (aspSocio is not null) vm.AspSocio = aspSocio;
        if (funcFam is not null) vm.FuncFam = funcFam;

        ViewBag.TiposFamilia = await _tipoFamiliaLogica.Listar();
        ViewBag.RelacParejas = await _relacParejaLogica.Listar();
        ViewBag.RelacPHs = await _relacPHLogica.Listar();
        ViewBag.RelacHermanos = await _relacHermanoLogica.Listar();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FichaSocial(FichaSocialViewModel modelo)
    {
        var idEmpleado = modelo.IdEmpleado;

        var guardo = await _secuencia.EjecutarTransaccionalAsync(async () =>
        {
            var dinamicaActual = (await _dinamicaLogica.Listar()).FirstOrDefault(d => d.IdEmpleado == idEmpleado);
            if (dinamicaActual is null)
            {
                modelo.Dinamica.IdEmpleado = idEmpleado;
                await _dinamicaLogica.CrearConNuevoId(modelo.Dinamica);
            }
            else
            {
                CopiarDinamica(dinamicaActual, modelo.Dinamica);
                await _dinamicaLogica.Actualizar(dinamicaActual);
            }

            var aspSocioActual = (await _aspSocioLogica.Listar()).FirstOrDefault(a => a.IdEmpleado == idEmpleado);
            if (aspSocioActual is null)
            {
                modelo.AspSocio.IdEmpleado = idEmpleado;
                await _aspSocioLogica.CrearConNuevoId(modelo.AspSocio);
            }
            else
            {
                CopiarAspSocio(aspSocioActual, modelo.AspSocio);
                await _aspSocioLogica.Actualizar(aspSocioActual);
            }

            var funcFamActual = (await _funcFamLogica.Listar()).FirstOrDefault(f => f.IdEmpleado == idEmpleado);
            if (funcFamActual is null)
            {
                modelo.FuncFam.IdEmpleado = idEmpleado;
                await _funcFamLogica.CrearConNuevoId(modelo.FuncFam);
            }
            else
            {
                CopiarFuncFam(funcFamActual, modelo.FuncFam);
                await _funcFamLogica.Actualizar(funcFamActual);
            }
        });

        if (!guardo)
            TempData["MensajeError"] = "No se pudo guardar la ficha social: intente nuevamente.";

        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Asegurados ----------

    [HttpGet]
    public async Task<IActionResult> Asegurados(int idEmpleado)
    {
        var asegurados = (await _aseguradoLogica.Listar()).Where(a => a.idEmpleado == idEmpleado);
        var personas = await _personaLogica.Listar();
        var estadoCiviles = await _estadoCivilLogica.Listar();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);

        var filas = asegurados.Select(a =>
        {
            var persona = a.idPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == a.idPersona);
            return new AseguradoFila
            {
                NroBeneficiario = a.NroBeneficiario,
                Nombre = persona is null ? null : persona.NombreCompleto(),
                EstadoCivil = a.idEstadoCivil is null ? null : estadoCiviles.FirstOrDefault(e => e.IdEstadoCivil == a.idEstadoCivil)?.DescripEstCivil,
                VinculoFamiliar = a.VinculoFamiliar,
                Sexo = a.Sexo,
                FechaInicio = a.FechaInicioVinculo,
                FechaFin = a.FechaFinVinculo
            };
        }).ToList();

        return View(filas);
    }

    [HttpGet]
    public async Task<IActionResult> AseguradoCrear(int idEmpleado)
    {
        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);
        ViewBag.EsEdicion = false;
        ViewBag.EstadosCiviles = await _estadoCivilLogica.Listar();
        return View("AseguradoForm", new RRHH_Asegurado { idEmpleado = idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AseguradoCrear(RRHH_Asegurado modelo)
    {
        await _aseguradoLogica.Crear(modelo);
        return RedirectToAction(nameof(Asegurados), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpGet]
    public async Task<IActionResult> AseguradoEditar(int idEmpleado, int nro)
    {
        var registro = await _aseguradoLogica.Obtener(idEmpleado, nro);
        if (registro is null)
            return NotFound();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);
        ViewBag.EsEdicion = true;
        ViewBag.EstadosCiviles = await _estadoCivilLogica.Listar();
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

        return RedirectToAction(nameof(Asegurados), new { idEmpleado = modelo.idEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AseguradoEliminar(int idEmpleado, int nro)
    {
        var actual = await _aseguradoLogica.Obtener(idEmpleado, nro);
        if (actual is not null)
            await _aseguradoLogica.Eliminar(actual);
        return RedirectToAction(nameof(Asegurados), new { idEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => await _empleados.NombreEmpleadoAsync(idEmpleado);

    private static void CopiarDinamica(RRHH_DinamicaFamiliar destino, RRHH_DinamicaFamiliar origen)
    {
        destino.idTipoFam = origen.idTipoFam;
        destino.idRelaPareja = origen.idRelaPareja;
        destino.idRelaPH = origen.idRelaPH;
        destino.idRelacHermano = origen.idRelacHermano;
        destino.Observacion = origen.Observacion;
    }

    private static void CopiarAspSocio(RRHH_AspSocio destino, RRHH_AspSocio origen)
    {
        destino.idPregunta1 = origen.idPregunta1;
        destino.idPregunta2 = origen.idPregunta2;
        destino.idPregunta3 = origen.idPregunta3;
        destino.PrincipProblemas = origen.PrincipProblemas;
        destino.PropMejoraInstitu = origen.PropMejoraInstitu;
        destino.hobby = origen.hobby;
        destino.ActRecreaInstitu = origen.ActRecreaInstitu;
    }

    private static void CopiarFuncFam(RRHH_FuncFam destino, RRHH_FuncFam origen)
    {
        destino.Com1 = origen.Com1;
        destino.Com2 = origen.Com2;
        destino.Afec1 = origen.Afec1;
        destino.Afec2 = origen.Afec2;
        destino.Decis1 = origen.Decis1;
        destino.Decis2 = origen.Decis2;
        destino.Soc1 = origen.Soc1;
        destino.Soc2 = origen.Soc2;
        destino.Soc3 = origen.Soc3;
    }
}