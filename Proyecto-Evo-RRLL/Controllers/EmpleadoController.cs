using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class EmpleadoController : Controller
{
    private readonly EmpleadoData _empleadoData;
    private readonly PersonaData _personaData;
    private readonly CargoData _cargoData;
    private readonly EstructOrganizData _estructOrganizData;
    private readonly LocalData _localData;
    private readonly HorarioData _horarioData;
    private readonly TipoTrabajadorData _tipoTrabajadorData;
    private readonly RegimenPensionData _regimenPensionData;
    private readonly TipoDocIDData _tipoDocData;
    private readonly AFPData _afpData;
    private readonly EstadoCivilData _estadoCivilData;
    private readonly TipoSangreData _tipoSangreData;
    private readonly DiscapacidadData _discapacidadData;
    private readonly ViveConData _viveConData;
    private readonly NacionalidadData _nacionalidadData;

    public EmpleadoController(
        EmpleadoData empleadoData,
        PersonaData personaData,
        CargoData cargoData,
        EstructOrganizData estructuraOrganizData,
        LocalData localData,
        HorarioData horarioData,
        TipoTrabajadorData tipoTrabajadorData,
        RegimenPensionData regimenPensionData,
        TipoDocIDData tipoDocData,
        AFPData afpData,
        EstadoCivilData estadoCivilData,
        TipoSangreData tipoSangreData,
        DiscapacidadData discapacidadData,
        ViveConData viveConData,
        NacionalidadData nacionalidadData)
    {
        _empleadoData = empleadoData;
        _personaData = personaData;
        _cargoData = cargoData;
        _estructOrganizData = estructuraOrganizData;
        _localData = localData;
        _horarioData = horarioData;
        _tipoTrabajadorData = tipoTrabajadorData;
        _regimenPensionData = regimenPensionData;
        _tipoDocData = tipoDocData;
        _afpData = afpData;
        _estadoCivilData = estadoCivilData;
        _tipoSangreData = tipoSangreData;
        _discapacidadData = discapacidadData;
        _viveConData = viveConData;
        _nacionalidadData = nacionalidadData;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? q)
    {
        var empleados = await _empleadoData.Listar();
        var personas = await _personaData.Listar();
        var cargos = await _cargoData.Listar();
        var areas = await _estructOrganizData.Listar();

        var vm = new EmpleadoViewModel { Busqueda = q };

        foreach (var e in empleados)
        {
            var persona = e.IdPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == e.IdPersona);
            var nombre = persona is null
                ? null
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
            var cargo = e.IdCargo is null ? null : cargos.FirstOrDefault(c => c.IdCargo == e.IdCargo)?.Descripcion;
            var area = areas.FirstOrDefault(a => a.Year == e.Year && a.idAreaOrganiz == e.idAreaOrganiz)?.AreaOrganizacional;

            if (!string.IsNullOrWhiteSpace(q)
                && !(nombre?.Contains(q, StringComparison.OrdinalIgnoreCase) == true)
                && !(persona?.NumDocID?.Contains(q, StringComparison.OrdinalIgnoreCase) == true))
                continue;

            vm.Empleados.Add(new EmpleadoFila
            {
                IdEmpleado = e.IdEmpleado,
                Nombres = nombre,
                NumDoc = persona?.NumDocID,
                Cargo = cargo,
                Area = area,
                Estado = e.Estado,
                FechaIngreso = e.FechaIngreso,
                TieneFoto = e.Foto is not null
            });
        }

        vm.Empleados = vm.Empleados.OrderBy(x => x.IdEmpleado).Take(500).ToList();
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Detalles(int id)
    {
        var empleado = await _empleadoData.Obtener(id);
        if (empleado is null)
            return NotFound();

        var personas = await _personaData.Listar();
        var persona = empleado.IdPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == empleado.IdPersona);

        var vm = new EmpleadoDetalleViewModel
        {
            IdEmpleado = empleado.IdEmpleado,
            Empleado = empleado,
            Persona = persona,
            NombresCompletos = persona is null
                ? null
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim(),
            TipoDoc = persona?.TipoDocID is null ? null : (await _tipoDocData.Listar()).FirstOrDefault(t => t.Nombre == persona.TipoDocID)?.Descripcion,
            Cargo = empleado.IdCargo is null ? null : (await _cargoData.Listar()).FirstOrDefault(c => c.IdCargo == empleado.IdCargo)?.Descripcion,
            Area = (await _estructOrganizData.Listar())
                .FirstOrDefault(a => a.Year == empleado.Year && a.idAreaOrganiz == empleado.idAreaOrganiz)?.AreaOrganizacional,
            Local = empleado.idLocal is null ? null : (await _localData.Listar()).FirstOrDefault(l => l.idLocal == empleado.idLocal)?.NombreLocal,
            Horario = empleado.idHorario is null ? null : (await _horarioData.Listar()).FirstOrDefault(h => h.idHorario == empleado.idHorario)?.Descrip_Horario,
            Estado = empleado.Estado,
            AFP = empleado.IdAFP is null ? null : (await _afpData.Listar()).FirstOrDefault(a => a.IdAFP == empleado.IdAFP)?.DescripAFP,
            EstadoCivil = empleado.IdEstadoCivil is null ? null : (await _estadoCivilData.Listar()).FirstOrDefault(ec => ec.IdEstadoCivil == empleado.IdEstadoCivil)?.DescripEstCivil,
            TipoSangre = empleado.IdTipoSangre is null ? null : (await _tipoSangreData.Listar()).FirstOrDefault(s => s.IdTipoSangre == empleado.IdTipoSangre)?.DescripTS,
            Discapacidad = empleado.IdDiscapacidad is null ? null : (await _discapacidadData.Listar()).FirstOrDefault(d => d.IdDiscapacidad == empleado.IdDiscapacidad)?.Descrip_Discapacidad,
            ViveCon = empleado.IdViveCon is null ? null : (await _viveConData.Listar()).FirstOrDefault(v => v.IdViveCon == empleado.IdViveCon)?.DescripViveCon,
            Nacionalidad = empleado.IdNacionalidad is null ? null : (await _nacionalidadData.Listar()).FirstOrDefault(n => n.IdNacionalidad == empleado.IdNacionalidad)?.Descripcion,
            TipoTrabajador = empleado.IdTipoTrabajador is null ? null : (await _tipoTrabajadorData.Listar()).FirstOrDefault(t => t.IdTipoTrabajador == empleado.IdTipoTrabajador)?.Descripcion,
            RegimenPension = empleado.IdRegimenPen is null ? null : (await _regimenPensionData.Listar()).FirstOrDefault(r => r.IdRegimenPen == empleado.IdRegimenPen)?.LeyRegimen,
            TieneFoto = empleado.Foto is not null
        };
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var empleado = await _empleadoData.Obtener(id);
        if (empleado is null)
            return NotFound();

        await CargarCatalogosEdicion();
        return View(empleado);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Empleado empleado, IFormFile? foto)
    {
        if (id != empleado.IdEmpleado)
            return NotFound();

        var original = await _empleadoData.Obtener(id);
        if (original is null)
            return NotFound();

        original.Year = empleado.Year;
        original.idAreaOrganiz = empleado.idAreaOrganiz;
        original.ExpSocial = empleado.ExpSocial;
        original.FechaIngreso = empleado.FechaIngreso;
        original.IdAFP = empleado.IdAFP;
        original.Brevete = empleado.Brevete;
        original.IdEstadoCivil = empleado.IdEstadoCivil;
        original.Sexo = empleado.Sexo;
        original.CtaCorriente = empleado.CtaCorriente;
        original.NroAFP = empleado.NroAFP;
        original.FechaAFP = empleado.FechaAFP;
        original.IdSeguro = empleado.IdSeguro;
        original.NroSeguro = empleado.NroSeguro;
        original.IdTipoSangre = empleado.IdTipoSangre;
        original.DecLey = empleado.DecLey;
        original.IdTipoTrabajador = empleado.IdTipoTrabajador;
        original.Estado = empleado.Estado;
        original.IdCargo = empleado.IdCargo;
        original.IdDiscapacidad = empleado.IdDiscapacidad;
        original.Profesion = empleado.Profesion;
        original.JefeHogar = empleado.JefeHogar;
        original.CargaFam = empleado.CargaFam;
        original.NumHijos = empleado.NumHijos;
        original.IdViveCon = empleado.IdViveCon;
        original.ObsFam = empleado.ObsFam;
        original.AspecSalud = empleado.AspecSalud;
        original.AspecEducacion = empleado.AspecEducacion;
        original.AspecRecreacion = empleado.AspecRecreacion;
        original.AspecPsico = empleado.AspecPsico;
        original.AspecSocial = empleado.AspecSocial;
        original.DiagnosSocial = empleado.DiagnosSocial;
        original.IdNacionalidad = empleado.IdNacionalidad;
        original.NumLibretaMilitar = empleado.NumLibretaMilitar;
        original.GradoInstruccion = empleado.GradoInstruccion;
        original.IdRegimenPen = empleado.IdRegimenPen;
        original.NumRUC = empleado.NumRUC;
        original.NumAutogenSalud = empleado.NumAutogenSalud;
        original.NumResolCese = empleado.NumResolCese;
        original.FechaResolCese = empleado.FechaResolCese;
        original.NumResol20530 = empleado.NumResol20530;
        original.FechaResol20530 = empleado.FechaResol20530;
        original.NumResolPenMen = empleado.NumResolPenMen;
        original.FechaResolPenMen = empleado.FechaResolPenMen;
        original.NumResoIngreEsta = empleado.NumResoIngreEsta;
        original.FechaIngreEstado = empleado.FechaIngreEstado;
        original.FechaResolIngreEsta = empleado.FechaResolIngreEsta;
        original.NumResoIngreInstitu = empleado.NumResoIngreInstitu;
        original.FechaResoIngreInstitu = empleado.FechaResoIngreInstitu;
        original.idHorario = empleado.idHorario;
        original.idLocal = empleado.idLocal;
        original.Foto = foto is not null && foto.Length > 0 ? await LeerArchivo(foto) : original.Foto;

        await _empleadoData.Actualizar(original);

        await CargarCatalogosEdicion();
        return RedirectToAction(nameof(Detalles), new { id });
    }

    private async Task CargarCatalogosEdicion()
    {
        ViewBag.Cargos = await _cargoData.Listar();
        ViewBag.Horarios = await _horarioData.Listar();
        ViewBag.Locales = await _localData.Listar();
        ViewBag.Areas = await _estructOrganizData.Listar();
        ViewBag.TiposTrabajador = await _tipoTrabajadorData.Listar();
        ViewBag.Regimenes = await _regimenPensionData.Listar();
        ViewBag.Afps = await _afpData.Listar();
        ViewBag.EstadosCiviles = await _estadoCivilData.Listar();
        ViewBag.TiposSangre = await _tipoSangreData.Listar();
        ViewBag.Discapacidades = await _discapacidadData.Listar();
        ViewBag.ViveCon = await _viveConData.Listar();
        ViewBag.Nacionalidades = await _nacionalidadData.Listar();
    }

    [HttpGet]
    public async Task<IActionResult> MostrarFoto(int id)
    {
        var empleado = await _empleadoData.Obtener(id);
        if (empleado?.Foto is null)
            return NotFound();
        return File(empleado.Foto, "image/jpeg");
    }

    private static async Task<byte[]> LeerArchivo(IFormFile archivo)
    {
        using var ms = new MemoryStream();
        await archivo.CopyToAsync(ms);
        return ms.ToArray();
    }
}