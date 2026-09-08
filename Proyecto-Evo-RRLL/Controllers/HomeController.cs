using Capa_Datos;
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
        private readonly EmpleadoData _empleadoData;
        private readonly PersonaData _personaData;
        private readonly AsistenciaData _asistenciaData;
        private readonly EstructOrganizData _estructOrganizData;
        private readonly CargoData _cargoData;

        public HomeController(
            EmpleadoData empleadoData,
            PersonaData personaData,
            AsistenciaData asistenciaData,
            EstructOrganizData estructuraOrganizData,
            CargoData cargoData)
        {
            _empleadoData = empleadoData;
            _personaData = personaData;
            _asistenciaData = asistenciaData;
            _estructOrganizData = estructuraOrganizData;
            _cargoData = cargoData;
        }

        public async Task<IActionResult> Index()
        {
            var empleados = await _empleadoData.Listar();
            var estructuras = await _estructOrganizData.Listar();
            var asistencias = await _asistenciaData.Listar();
            var personas = await _personaData.Listar();
            var cargos = await _cargoData.Listar();

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
                asisUltimos7.Add(asistencias.Count(a => a.Fecha.Date == fecha));
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
                Personas = personas.Count,
                asistenciasHoy = asistencias.Count(a => a.Fecha.Date == hoy),
                Areas = estructuras.Count,
                Cargos = cargos.Count,
                AreasNombres = areasNombres,
                EmpleadosPorArea = areasCounts,
                DiasAsistencia = ultimos7,
                AsistenciasPorDia = asisUltimos7,
                EstadosNombres = estadosNombres,
                EmpleadosPorEstado = estadosCounts
            };
            return View(model);
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