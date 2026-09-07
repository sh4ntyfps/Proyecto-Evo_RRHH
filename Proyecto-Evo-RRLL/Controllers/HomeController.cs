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
            var model = new DashboardViewModel
            {
                Empleados = (await _empleadoData.Listar()).Count,
                Personas = (await _personaData.Listar()).Count,
                asistenciasHoy = (await _asistenciaData.Listar())
                    .Count(a => a.Fecha.Date == DateTime.Today),
                Areas = (await _estructOrganizData.Listar()).Count,
                Cargos = (await _cargoData.Listar()).Count
            };
            return View(model);
        }

        public IActionResult Ayuda()
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