using Capa_Logica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Evo_RRLL.Models;
using Proyecto_Evo_RRLL.Models.ViewModels;
using System.Diagnostics;

namespace Proyecto_Evo_RRLL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly EmpleadoLogica _empleadoLogica;
        private readonly PersonaLogica _personaLogica;
        private readonly AsistenciaLogica _asistenciaLogica;
        private readonly EstructOrganizLogica _estructOrganizLogica;
private readonly CargoLogica _cargoLogica;
    private readonly RRHH_FeriadoLogica _feriadoLogica;

    public HomeController(
        EmpleadoLogica empleadoLogica,
        PersonaLogica personaLogica,
        AsistenciaLogica asistenciaLogica,
        EstructOrganizLogica estructuraOrganizLogica,
        CargoLogica cargoLogica,
        RRHH_FeriadoLogica feriadoLogica)
    {
        _empleadoLogica = empleadoLogica;
        _personaLogica = personaLogica;
        _asistenciaLogica = asistenciaLogica;
        _estructOrganizLogica = estructuraOrganizLogica;
        _cargoLogica = cargoLogica;
        _feriadoLogica = feriadoLogica;
    }

        public async Task<IActionResult> Index()
        {
            var empleados = await _empleadoLogica.Listar();
            var estructuras = await _estructOrganizLogica.Listar();
            var personas = await _personaLogica.Listar();
            var cargos = await _cargoLogica.Listar();

            // Empleados por área
            var areaPorKey = estructuras.ToDictionary(e => $"{e.Year}|{e.idAreaOrganiz}", e => e.AreaOrganizacional ?? $"Área {e.idAreaOrganiz}");
            var porArea = empleados
                .GroupBy(e => $"{e.Year}|{e.idAreaOrganiz}")
                .OrderByDescending(g => g.Count())
                .Take(8);
            var areasNombres = new List<string>();
            var areasCounts = new List<int>();
            foreach (var g in porArea)
            {
                areasNombres.Add(areaPorKey.TryGetValue(g.Key, out var nombre) ? nombre : g.Key);
                areasCounts.Add(g.Count());
            }

            // Asistencias en los últimos 7 días (incluido hoy)
            var hoy = DateTime.Today;
            var ultimos7 = new List<string>();
            var asisUltimos7 = new List<int>();
            for (var d = 6; d >= 0; d--)
            {
                var fecha = hoy.AddDays(-d);
                ultimos7.Add(fecha.ToString("dd/MM"));
                asisUltimos7.Add(await _asistenciaLogica.ContarEntreFechas(fecha, fecha));
            }

            // Empleados por estado
            var estadosNombres = new List<string>();
            var estadosCounts = new List<int>();
            foreach (var g in empleados.GroupBy(e => string.IsNullOrWhiteSpace(e.Estado) ? "Sin estado" : e.Estado!).OrderByDescending(x => x.Count()))
            {
                estadosNombres.Add(g.Key);
                estadosCounts.Add(g.Count());
            }

            var model = new DashboardViewModel
            {
                Empleados = empleados.Count,
                EmpleadosActivos = empleados.Count(e => e.Estado == "A"),
                Personas = personas.Count,
                asistenciasHoy = await _asistenciaLogica.ContarEntreFechas(hoy, hoy),
                Areas = estructuras.Count,
                Cargos = cargos.Count,
                AreasNombres = areasNombres,
                EmpleadosPorArea = areasCounts,
                DiasAsistencia = ultimos7,
                AsistenciasPorDia = asisUltimos7,
                EstadosNombres = estadosNombres,
                EmpleadosPorEstado = estadosCounts,
                FeriadosMes = await FeriadosDelMesAsync(hoy)
            };
            return View(model);
        }

        private async Task<List<string>> FeriadosDelMesAsync(DateTime hoy)
        {
            try
            {
                return (await _feriadoLogica.Listar())
                    .Where(f => f.Fecha?.Year == hoy.Year && f.Fecha!.Value.Month == hoy.Month)
                    .OrderBy(f => f.Fecha)
                    .Select(f => string.IsNullOrWhiteSpace(f.Motivo) ? $"{f.Fecha:dd/MM}" : $"{f.Fecha:dd/MM} - {f.Motivo}")
                    .ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public IActionResult Ayuda()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult PaginaNoEncontrada(int statusCode = 404)
        {
            return View(statusCode);
        }

        public IActionResult Privacidad()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}