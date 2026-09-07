using Capa_Datos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class AsistenciaController : Controller
{
    private readonly AsistenciaData _asistenciaData;
    private readonly EmpleadoData _empleadoData;
    private readonly PersonaData _personaData;

    public AsistenciaController(AsistenciaData asistenciaData, EmpleadoData empleadoData, PersonaData personaData)
    {
        _asistenciaData = asistenciaData;
        _empleadoData = empleadoData;
        _personaData = personaData;
    }

    [HttpGet]
    public async Task<IActionResult> Index(DateTime? desde, DateTime? hasta, string? q)
    {
        var registros = await _asistenciaData.Listar();
        var empleados = await _empleadoData.Listar();
        var personas = await _personaData.Listar();

        var desdeEf = desde ?? DateTime.Today.AddMonths(-1);
        var hastaEf = hasta ?? DateTime.Today.AddDays(1);

        var vm = new AsistenciaViewModel { Desde = desde, Hasta = hasta ?? DateTime.Today, Busqueda = q };

        foreach (var r in registros.Where(x => x.Fecha >= desdeEf && x.Fecha < hastaEf))
        {
            var empleado = empleados.FirstOrDefault(e => e.IdEmpleado == r.IdEmpleado);
            var persona = empleado?.IdPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == empleado.IdPersona);
            var nombre = persona is null
                ? $"(Empleado {r.IdEmpleado})"
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();

            if (!string.IsNullOrWhiteSpace(q)
                && !nombre.Contains(q, StringComparison.OrdinalIgnoreCase)
                && !(persona?.NumDocID?.Contains(q, StringComparison.OrdinalIgnoreCase) == true))
                continue;

            vm.Registros.Add(new AsistenciaFila
            {
                Fecha = r.Fecha,
                IdEmpleado = r.IdEmpleado,
                Empleado = nombre,
                HorEntrada = r.Hor_Ent,
                HorSalida = r.Hor_Sal,
                AlmSalida = r.Alm_Sal,
                AlmEntrada = r.Alm_Ent,
                Estado = r.Estado
            });

            if (vm.Registros.Count >= 2000)
                break;
        }

        vm.Registros = vm.Registros.OrderByDescending(x => x.Fecha).ThenBy(x => x.IdEmpleado).ToList();
        return View(vm);
    }
}