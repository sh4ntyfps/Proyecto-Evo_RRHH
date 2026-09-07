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

    public EmpleadoController(
        EmpleadoData empleadoData,
        PersonaData personaData,
        CargoData cargoData,
        EstructOrganizData estructuraOrganizData,
        LocalData localData,
        HorarioData horarioData,
        TipoTrabajadorData tipoTrabajadorData,
        RegimenPensionData regimenPensionData,
        TipoDocIDData tipoDocData)
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
            NombresCompletos = persona is null
                ? null
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim(),
            NumDoc = persona?.NumDocID,
            Sexo = empleado.Sexo,
            Cargo = (await _cargoData.Listar()).FirstOrDefault(c => c.IdCargo == empleado.IdCargo)?.Descripcion,
            Area = (await _estructOrganizData.Listar())
                .FirstOrDefault(a => a.Year == empleado.Year && a.idAreaOrganiz == empleado.idAreaOrganiz)?.AreaOrganizacional,
            Local = empleado.idLocal is null ? null : (await _localData.Listar()).FirstOrDefault(l => l.idLocal == empleado.idLocal)?.NombreLocal,
            Horario = empleado.idHorario is null ? null : (await _horarioData.Listar()).FirstOrDefault(h => h.idHorario == empleado.idHorario)?.Descrip_Horario,
            Estado = empleado.Estado,
            FechaIngreso = empleado.FechaIngreso,
            TipoTrabajador = (await _tipoTrabajadorData.Listar()).FirstOrDefault(t => t.IdTipoTrabajador == empleado.IdTipoTrabajador)?.Descripcion,
            RegimenPension = (await _regimenPensionData.Listar()).FirstOrDefault(r => r.IdRegimenPen == empleado.IdRegimenPen)?.LeyRegimen,
            TieneFoto = empleado.Foto is not null
        };

        var tipoDoc = persona?.TipoDocID is null ? null : (await _tipoDocData.Listar()).FirstOrDefault(t => t.Nombre == persona.TipoDocID)?.Descripcion;
        vm.TipoDoc = tipoDoc;
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var empleado = await _empleadoData.Obtener(id);
        if (empleado is null)
            return NotFound();

        ViewBag.Cargos = await _cargoData.Listar();
        ViewBag.Horarios = await _horarioData.Listar();
        ViewBag.Locales = await _localData.Listar();
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

        original.FechaIngreso = empleado.FechaIngreso;
        original.Estado = empleado.Estado;
        original.idHorario = empleado.idHorario;
        original.idLocal = empleado.idLocal;
        original.IdCargo = empleado.IdCargo;
        original.Foto = foto is not null && foto.Length > 0 ? await LeerArchivo(foto) : original.Foto;

        await _empleadoData.Actualizar(original);

        ViewBag.Cargos = await _cargoData.Listar();
        ViewBag.Horarios = await _horarioData.Listar();
        ViewBag.Locales = await _localData.Listar();
        return RedirectToAction(nameof(Detalles), new { id });
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