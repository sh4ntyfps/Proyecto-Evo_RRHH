using Capa_Datos;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class ViviendaController : Controller
{
    private readonly EmpleadoData _empleadoData;
    private readonly PersonaData _personaData;
    private readonly ViviendaData _viviendaData;
    private readonly TipoVivData _tipoVivData;
    private readonly TenenciaVivData _tenenciaVivData;
    private readonly UbicacionVivData _ubicacionVivData;
    private readonly AguaVivData _aguaVivData;
    private readonly AlumbradoVivData _alumbradoVivData;
    private readonly ConservacionVivData _conservacionVivData;
    private readonly ExcretasVivData _excretasVivData;
    private readonly MaterialVivData _materialVivData;

    public ViviendaController(
        EmpleadoData empleadoData,
        PersonaData personaData,
        ViviendaData viviendaData,
        TipoVivData tipoVivData,
        TenenciaVivData tenenciaVivData,
        UbicacionVivData ubicacionVivData,
        AguaVivData aguaVivData,
        AlumbradoVivData alumbradoVivData,
        ConservacionVivData conservacionVivData,
        ExcretasVivData excretasVivData,
        MaterialVivData materialVivData)
    {
        _empleadoData = empleadoData;
        _personaData = personaData;
        _viviendaData = viviendaData;
        _tipoVivData = tipoVivData;
        _tenenciaVivData = tenenciaVivData;
        _ubicacionVivData = ubicacionVivData;
        _aguaVivData = aguaVivData;
        _alumbradoVivData = alumbradoVivData;
        _conservacionVivData = conservacionVivData;
        _excretasVivData = excretasVivData;
        _materialVivData = materialVivData;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int idEmpleado)
    {
        if (await _empleadoData.Obtener(idEmpleado) is null)
            return NotFound();

        var registro = (await _viviendaData.Listar()).FirstOrDefault(v => v.IdEmpleado == idEmpleado);

        var tipos = await _tipoVivData.Listar();
        var tenencias = await _tenenciaVivData.Listar();
        var ubicaciones = await _ubicacionVivData.Listar();
        var aguas = await _aguaVivData.Listar();
        var alumbrados = await _alumbradoVivData.Listar();
        var conservaciones = await _conservacionVivData.Listar();
        var excretas = await _excretasVivData.Listar();
        var materiales = await _materialVivData.Listar();

        var vm = new ViviendaViewModel
        {
            IdEmpleado = idEmpleado,
            NombreEmpleado = await NombreEmpleadoAsync(idEmpleado),
            Registro = registro,
            Tenencia = registro?.IdTenencia is null ? null : tenencias.FirstOrDefault(t => t.IdTenencia == registro.IdTenencia)?.DescripTenencia,
            Tipo = registro?.IdTipo is null ? null : tipos.FirstOrDefault(t => t.IdTipo == registro.IdTipo)?.DescripTipo,
            Material = registro?.IdMaterial is null ? null : materiales.FirstOrDefault(m => m.IdMaterial == registro.IdMaterial)?.DescripMaterial,
            Conservacion = registro?.IdConservacion is null ? null : conservaciones.FirstOrDefault(c => c.IdConservacion == registro.IdConservacion)?.DescripConserva,
            Ubicacion = registro?.IdUbicacion is null ? null : ubicaciones.FirstOrDefault(u => u.IdUbicacion == registro.IdUbicacion)?.DescripUbicacion,
            Alumbrado = registro?.IdAlumbrado is null ? null : alumbrados.FirstOrDefault(a => a.IdAlumbrado == registro.IdAlumbrado)?.DescripAlumbrado,
            Agua = registro?.IdAgua is null ? null : aguas.FirstOrDefault(a => a.IdAgua == registro.IdAgua)?.DescripAgua,
            Excretas = registro?.IdExcretas is null ? null : excretas.FirstOrDefault(e => e.IdExcretas == registro.IdExcretas)?.DescripExcretas
        };

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> ViviendaForm(int idEmpleado)
    {
        if (await _empleadoData.Obtener(idEmpleado) is null)
            return NotFound();

        var registro = (await _viviendaData.Listar()).FirstOrDefault(v => v.IdEmpleado == idEmpleado) ?? new Vivienda();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);
        ViewBag.Tipos = await _tipoVivData.Listar();
        ViewBag.Tenencias = await _tenenciaVivData.Listar();
        ViewBag.Ubicaciones = await _ubicacionVivData.Listar();
        ViewBag.Aguas = await _aguaVivData.Listar();
        ViewBag.Alumbrados = await _alumbradoVivData.Listar();
        ViewBag.Conservaciones = await _conservacionVivData.Listar();
        ViewBag.Excretas = await _excretasVivData.Listar();
        ViewBag.Materiales = await _materialVivData.Listar();
        return View(registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ViviendaForm(Vivienda modelo)
    {
        var actual = (await _viviendaData.Listar()).FirstOrDefault(v => v.IdEmpleado == modelo.IdEmpleado);

        if (actual is null)
        {
            var lista = await _viviendaData.Listar();
            modelo.IdVivienda = lista.Count > 0 ? lista.Max(x => x.IdVivienda) + 1 : 1;
            await _viviendaData.Crear(modelo);
        }
        else
        {
            Copiar(actual, modelo);
            await _viviendaData.Actualizar(actual);
        }

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int idEmpleado)
    {
        var actual = (await _viviendaData.Listar()).FirstOrDefault(v => v.IdEmpleado == idEmpleado);
        if (actual is not null)
            await _viviendaData.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
    {
        var empleado = await _empleadoData.Obtener(idEmpleado);
        if (empleado?.IdPersona is null)
            return empleado is null ? null : $"(Empleado {idEmpleado})";
        var persona = (await _personaData.Listar()).FirstOrDefault(p => p.IdPersona == empleado.IdPersona);
        return persona is null
            ? $"(Empleado {idEmpleado})"
            : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
    }

    private static void Copiar(Vivienda destino, Vivienda origen)
    {
        destino.IdEmpleado = origen.IdEmpleado;
        destino.IdTenencia = origen.IdTenencia;
        destino.IdTipo = origen.IdTipo;
        destino.IdMaterial = origen.IdMaterial;
        destino.IdConservacion = origen.IdConservacion;
        destino.NumDormitorio = origen.NumDormitorio;
        destino.NumServHig = origen.NumServHig;
        destino.IdUbicacion = origen.IdUbicacion;
        destino.IdAlumbrado = origen.IdAlumbrado;
        destino.OtroAlumbrado = origen.OtroAlumbrado;
        destino.IdAgua = origen.IdAgua;
        destino.OtroAgua = origen.OtroAgua;
        destino.IdExcretas = origen.IdExcretas;
        destino.OtroExcreta = origen.OtroExcreta;
        destino.NumCompart = origen.NumCompart;
        destino.Observacion = origen.Observacion;
        destino.NumPerDorm = origen.NumPerDorm;
        destino.Telefono = origen.Telefono;
        destino.Cable = origen.Cable;
        destino.Internet = origen.Internet;
        destino.Otros = origen.Otros;
    }
}