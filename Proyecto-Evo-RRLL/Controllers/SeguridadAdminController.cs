using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Capa_Datos;
using Capa_Entidades;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize]
public class SeguridadAdminController : Controller
{
    private readonly UsuarioData _usuarioData;
    private readonly Usuario_RolData _usuarioRolData;
    private readonly RolData _rolData;
    private readonly Rol_AccesoData _rolAccesoData;
    private readonly SistemaOpcionData _sistemaOpcionData;
    private readonly EmpleadoData _empleadoData;
    private readonly PersonaData _personaData;

    public SeguridadAdminController(
        UsuarioData usuarioData,
        Usuario_RolData usuarioRolData,
        RolData rolData,
        Rol_AccesoData rolAccesoData,
        SistemaOpcionData sistemaOpcionData,
        EmpleadoData empleadoData,
        PersonaData personaData)
    {
        _usuarioData = usuarioData;
        _usuarioRolData = usuarioRolData;
        _rolData = rolData;
        _rolAccesoData = rolAccesoData;
        _sistemaOpcionData = sistemaOpcionData;
        _empleadoData = empleadoData;
        _personaData = personaData;
    }

    // ---------- Usuarios ----------

    public async Task<IActionResult> Usuarios(string? q)
    {
        var usuarios = await _usuarioData.Listar();
        var empleados = await _empleadoData.Listar();
        var personas = await _personaData.Listar();
        var relaciones = await _usuarioRolData.Listar();
        var roles = await _rolData.Listar();

        var vm = new List<UsuarioFila>();
        foreach (var u in usuarios)
        {
            if (!string.IsNullOrWhiteSpace(q)
                && !(u.Login?.Contains(q, StringComparison.OrdinalIgnoreCase) == true)
                && !(u.Descripcion?.Contains(q, StringComparison.OrdinalIgnoreCase) == true))
                continue;

            var emp = u.IdEmpleado is null ? null : empleados.FirstOrDefault(e => e.IdEmpleado == u.IdEmpleado);
            var persona = emp?.IdPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == emp.IdPersona);
            var nombre = persona is null
                ? ""
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();

            var rolesUs = relaciones.Where(r => r.IdUsuario == u.IdUsuario)
                .Select(r => roles.FirstOrDefault(x => x.IdRol == r.IdRol)?.Descripcion)
                .Where(x => x is not null)
                .Select(x => x!);

            vm.Add(new UsuarioFila
            {
                Usuario = u,
                Empleado = nombre,
                Roles = string.Join(", ", rolesUs)
            });
        }

        ViewBag.Busqueda = q;
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> UsuarioNuevo()
    {
        ViewBag.Empleados = await EmpleadosAsync();
        return View("UsuarioForm", new Usuario { Fecha = DateTime.Now, Estado = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UsuarioNuevo(Usuario modelo, string clave)
    {
        if (string.IsNullOrWhiteSpace(clave))
            ModelState.AddModelError("clave", "La clave es obligatoria.");

        if (string.IsNullOrWhiteSpace(modelo.Login) == false
            && await _usuarioData.ObtenerPorLogin(modelo.Login.Trim()) is not null)
            ModelState.AddModelError("", "Ya existe un usuario con ese login.");

        if (ModelState.IsValid)
        {
            var lista = await _usuarioData.Listar();
            modelo.IdUsuario = lista.Count == 0 ? 1 : lista.Max(u => u.IdUsuario) + 1;
            modelo.Login = modelo.Login!.Trim();
            modelo.PasswordHash = new PasswordHasher<Usuario>().HashPassword(modelo, clave);
            modelo.Fecha = DateTime.Now;
            await _usuarioData.Crear(modelo);
            TempData["MensajeExito"] = "Usuario creado.";
            return RedirectToAction(nameof(Usuarios));
        }

        ViewBag.Empleados = await EmpleadosAsync();
        return View("UsuarioForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> UsuarioEditar(int id)
    {
        var usuario = await _usuarioData.Obtener(id);
        if (usuario is null)
            return NotFound();

        ViewBag.Empleados = await EmpleadosAsync();
        return View("UsuarioForm", usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UsuarioEditar(Usuario modelo, string clave)
    {
        var actual = await _usuarioData.Obtener(modelo.IdUsuario);
        if (actual is null)
            return NotFound();

        if (!string.IsNullOrEmpty(actual.Login) && !string.IsNullOrEmpty(modelo.Login)
            && actual.Login.Trim() != modelo.Login.Trim()
            && await _usuarioData.ObtenerPorLogin(modelo.Login.Trim()) is not null)
            ModelState.AddModelError("", "Ya existe un usuario con ese login.");

        if (ModelState.IsValid)
        {
            actual.Login = modelo.Login?.Trim();
            actual.Descripcion = modelo.Descripcion;
            actual.Estado = modelo.Estado;
            actual.IdEmpleado = modelo.IdEmpleado;
            if (!string.IsNullOrWhiteSpace(clave))
                actual.PasswordHash = new PasswordHasher<Usuario>().HashPassword(actual, clave);
            await _usuarioData.Actualizar(actual);
            TempData["MensajeExito"] = "Usuario actualizado.";
            return RedirectToAction(nameof(Usuarios));
        }

        ViewBag.Empleados = await EmpleadosAsync();
        return View("UsuarioForm", modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UsuarioEliminar(int id)
    {
        var actual = await _usuarioData.Obtener(id);
        if (actual is not null)
        {
            var relaciones = (await _usuarioRolData.Listar()).Where(r => r.IdUsuario == id).ToList();
            foreach (var r in relaciones)
                await _usuarioRolData.Eliminar(r);
            await _usuarioData.Eliminar(actual);
            TempData["MensajeExito"] = "Usuario eliminado.";
        }
        return RedirectToAction(nameof(Usuarios));
    }

    // ---------- Roles del usuario ----------

    [HttpGet]
    public async Task<IActionResult> UsuarioRoles(int id)
    {
        var usuario = await _usuarioData.Obtener(id);
        if (usuario is null)
            return NotFound();

        var roles = await _rolData.Listar();
        var asignados = (await _usuarioRolData.Listar()).Where(r => r.IdUsuario == id).Select(r => r.IdRol).ToHashSet();

        ViewBag.Usuario = usuario;
        ViewBag.Roles = roles
            .Select(r => new OpcionCheck { Valor = r.IdRol, Texto = r.Descripcion ?? "", Seleccionado = asignados.Contains(r.IdRol) })
            .ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UsuarioRoles(int id, int[] roles)
    {
        var usuario = await _usuarioData.Obtener(id);
        if (usuario is null)
            return NotFound();

        var listaRoles = await _rolData.Listar();
        var actuales = (await _usuarioRolData.Listar()).Where(r => r.IdUsuario == id).ToList();

        foreach (var r in actuales.Where(r => !roles.Contains(r.IdRol)))
            await _usuarioRolData.Eliminar(r);

        foreach (var r in roles)
        {
            if (actuales.Any(a => a.IdRol == r))
                continue;
            await _usuarioRolData.Crear(new Usuario_Rol { IdUsuario = id, IdRol = r });
        }

        TempData["MensajeExito"] = "Roles del usuario actualizados.";
        return RedirectToAction(nameof(Usuarios));
    }

    // ---------- Roles ----------

    public async Task<IActionResult> Roles()
    {
        var roles = await _rolData.Listar();
        var usuarios = await _usuarioData.Listar();
        var relaciones = await _usuarioRolData.Listar();

        var vm = roles.Select(r => new RolFila
        {
            Rol = r,
            Usuarios = relaciones.Count(x => x.IdRol == r.IdRol),
            TotalUsuarios = usuarios.Count
        }).ToList();

        return View(vm);
    }

    [HttpGet]
    public IActionResult RolNuevo()
    {
        return View("RolForm", new Rol());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RolNuevo(Rol modelo)
    {
        if (ModelState.IsValid)
        {
            var lista = await _rolData.Listar();
            modelo.IdRol = lista.Count == 0 ? 1 : lista.Max(r => r.IdRol) + 1;
            await _rolData.Crear(modelo);
            TempData["MensajeExito"] = "Rol creado.";
            return RedirectToAction(nameof(Roles));
        }
        return View("RolForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> RolEditar(int id)
    {
        var rol = await _rolData.Obtener(id);
        if (rol is null)
            return NotFound();
        return View("RolForm", rol);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RolEditar(Rol modelo)
    {
        var actual = await _rolData.Obtener(modelo.IdRol);
        if (actual is null)
            return NotFound();

        actual.Descripcion = modelo.Descripcion;
        actual.IdSistema = modelo.IdSistema;
        await _rolData.Actualizar(actual);
        TempData["MensajeExito"] = "Rol actualizado.";
        return RedirectToAction(nameof(Roles));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RolEliminar(int id)
    {
        var actual = await _rolData.Obtener(id);
        if (actual is not null)
        {
            var rel = (await _usuarioRolData.Listar()).Where(r => r.IdRol == id).ToList();
            foreach (var r in rel)
                await _usuarioRolData.Eliminar(r);

            var accesos = (await _rolAccesoData.Listar()).Where(a => a.IdRol == id).ToList();
            foreach (var a in accesos)
                await _rolAccesoData.Eliminar(a);

            await _rolData.Eliminar(actual);
            TempData["MensajeExito"] = "Rol eliminado.";
        }
        return RedirectToAction(nameof(Roles));
    }

    // ---------- Accesos del rol ----------

    [HttpGet]
    public async Task<IActionResult> RolAccesos(int id)
    {
        var rol = await _rolData.Obtener(id);
        if (rol is null)
            return NotFound();

        var opciones = await _sistemaOpcionData.Listar();
        var actuales = (await _rolAccesoData.Listar()).Where(a => a.IdRol == id).ToList();

        ViewBag.Rol = rol;
        ViewBag.Opciones = opciones
            .Select(o => new OpcionCheckTexto
            {
                Valor = o.IdSistemaOpcion ?? "",
                Texto = $"{o.IdSistemaOpcion} - {o.Descripcion}",
                Seleccionado = actuales.Any(a => a.IdSistemaOpcion == o.IdSistemaOpcion)
            })
            .ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RolAccesos(int id, string[] opciones)
    {
        var rol = await _rolData.Obtener(id);
        if (rol is null)
            return NotFound();

        var actuales = (await _rolAccesoData.Listar()).Where(a => a.IdRol == id).ToList();

        foreach (var a in actuales.Where(a => !opciones.Contains(a.IdSistemaOpcion ?? "")))
            await _rolAccesoData.Eliminar(a);

        foreach (var o in opciones)
        {
            if (actuales.Any(a => a.IdSistemaOpcion == o))
                continue;
            await _rolAccesoData.Crear(new Rol_Acceso { IdRol = id, IdSistemaOpcion = o, Permiso = "A" });
        }

        TempData["MensajeExito"] = "Permisos del rol actualizados.";
        return RedirectToAction(nameof(Roles));
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
    {
        var empleados = await _empleadoData.Listar();
        var personas = await _personaData.Listar();
        return empleados.Select(e =>
        {
            var persona = e.IdPersona is null ? null : personas.FirstOrDefault(p => p.IdPersona == e.IdPersona);
            var nombre = persona is null
                ? $"(Empleado {e.IdEmpleado})"
                : $"{persona.Nombres} {persona.Apellido_Paterno} {persona.Apellido_Materno}".Trim();
            return (e.IdEmpleado, nombre);
        }).OrderBy(x => x.nombre).ToList();
    }
}