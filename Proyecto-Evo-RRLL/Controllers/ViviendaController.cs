using Capa_Logica;
using Capa_Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class ViviendaController : Controller
{
    private readonly EmpleadoLogica _empleadoLogica;
    private readonly ViviendaLogica _viviendaLogica;
    private readonly TipoVivLogica _tipoVivLogica;
    private readonly TenenciaVivLogica _tenenciaVivLogica;
    private readonly UbicacionVivLogica _ubicacionVivLogica;
    private readonly AguaVivLogica _aguaVivLogica;
    private readonly AlumbradoVivLogica _alumbradoVivLogica;
    private readonly ConservacionVivLogica _conservacionVivLogica;
    private readonly ExcretasVivLogica _excretasVivLogica;
    private readonly MaterialVivLogica _materialVivLogica;
    private readonly EmpleadoServicio _empleados;

    public ViviendaController(
        EmpleadoLogica empleadoLogica,
        ViviendaLogica viviendaLogica,
        TipoVivLogica tipoVivLogica,
        TenenciaVivLogica tenenciaVivLogica,
        UbicacionVivLogica ubicacionVivLogica,
        AguaVivLogica aguaVivLogica,
        AlumbradoVivLogica alumbradoVivLogica,
        ConservacionVivLogica conservacionVivLogica,
        ExcretasVivLogica excretasVivLogica,
        MaterialVivLogica materialVivLogica,
        EmpleadoServicio empleados)
    {
        _empleadoLogica = empleadoLogica;
        _viviendaLogica = viviendaLogica;
        _tipoVivLogica = tipoVivLogica;
        _tenenciaVivLogica = tenenciaVivLogica;
        _ubicacionVivLogica = ubicacionVivLogica;
        _aguaVivLogica = aguaVivLogica;
        _alumbradoVivLogica = alumbradoVivLogica;
        _conservacionVivLogica = conservacionVivLogica;
        _excretasVivLogica = excretasVivLogica;
        _materialVivLogica = materialVivLogica;
        _empleados = empleados;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? idEmpleado)
    {
        ViewBag.Empleados = await EmpleadosAsync();

        if (idEmpleado is null)
            return View("Indice", ViewBag.Empleados);

        int id = idEmpleado.Value;
        if (await _empleadoLogica.Obtener(id) is null)
            return NotFound();

        var registro = (await _viviendaLogica.Listar()).FirstOrDefault(v => v.IdEmpleado == id);

        var tipos = await _tipoVivLogica.Listar();
        var tenencias = await _tenenciaVivLogica.Listar();
        var ubicaciones = await _ubicacionVivLogica.Listar();
        var aguas = await _aguaVivLogica.Listar();
        var alumbrados = await _alumbradoVivLogica.Listar();
        var conservaciones = await _conservacionVivLogica.Listar();
        var excretas = await _excretasVivLogica.Listar();
        var materiales = await _materialVivLogica.Listar();

        var vm = new ViviendaViewModel
        {
            IdEmpleado = id,
            NombreEmpleado = await NombreEmpleadoAsync(id),
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
        if (await _empleadoLogica.Obtener(idEmpleado) is null)
            return NotFound();

        var registro = (await _viviendaLogica.Listar()).FirstOrDefault(v => v.IdEmpleado == idEmpleado) ?? new Vivienda();

        ViewBag.IdEmpleado = idEmpleado;
        ViewBag.NombreEmpleado = await NombreEmpleadoAsync(idEmpleado);
        ViewBag.Tipos = await _tipoVivLogica.Listar();
        ViewBag.Tenencias = await _tenenciaVivLogica.Listar();
        ViewBag.Ubicaciones = await _ubicacionVivLogica.Listar();
        ViewBag.Aguas = await _aguaVivLogica.Listar();
        ViewBag.Alumbrados = await _alumbradoVivLogica.Listar();
        ViewBag.Conservaciones = await _conservacionVivLogica.Listar();
        ViewBag.Excretas = await _excretasVivLogica.Listar();
        ViewBag.Materiales = await _materialVivLogica.Listar();
        return View(registro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ViviendaForm(Vivienda modelo)
    {
        var actual = (await _viviendaLogica.Listar()).FirstOrDefault(v => v.IdEmpleado == modelo.IdEmpleado);

        if (actual is null)
        {
            var guardo = await _viviendaLogica.Registrar(modelo);
            if (!guardo)
            {
                ViewBag.IdEmpleado = modelo.IdEmpleado;
                ViewBag.NombreEmpleado = await NombreEmpleadoAsync(modelo.IdEmpleado ?? 0);
                ViewBag.Tipos = await _tipoVivLogica.Listar();
                ViewBag.Tenencias = await _tenenciaVivLogica.Listar();
                ViewBag.Ubicaciones = await _ubicacionVivLogica.Listar();
                ViewBag.Aguas = await _aguaVivLogica.Listar();
                ViewBag.Alumbrados = await _alumbradoVivLogica.Listar();
                ViewBag.Conservaciones = await _conservacionVivLogica.Listar();
                ViewBag.Excretas = await _excretasVivLogica.Listar();
                ViewBag.Materiales = await _materialVivLogica.Listar();
                ModelState.AddModelError("", "No se pudo guardar la vivienda: intente nuevamente.");
                return View(modelo);
            }
        }
        else
        {
            Copiar(actual, modelo);
            await _viviendaLogica.Actualizar(actual);
        }

        return RedirectToAction(nameof(Index), new { idEmpleado = modelo.IdEmpleado });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int idEmpleado)
    {
        var actual = (await _viviendaLogica.Listar()).FirstOrDefault(v => v.IdEmpleado == idEmpleado);
        if (actual is not null)
            await _viviendaLogica.Eliminar(actual);
        return RedirectToAction(nameof(Index), new { idEmpleado });
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();

    private async Task<string?> NombreEmpleadoAsync(int idEmpleado)
        => await _empleados.NombreEmpleadoAsync(idEmpleado);

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