using Microsoft.AspNetCore.Authorization;
using Capa_Entidades.Constantes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Capa_Logica;
using Capa_Entidades;
using Proyecto_Evo_RRLL.Models.ViewModels;

namespace Proyecto_Evo_RRLL.Controllers;

[Authorize(Roles = ConstantesSeguridad.RolAdministrador)]
public class SeguridadAdminController : Controller
{
    private readonly UsuarioLogica _usuarioLogica;
    private readonly Usuario_RolLogica _usuarioRolLogica;
    private readonly RolLogica _rolLogica;
    private readonly Rol_AccesoLogica _rolAccesoLogica;
    private readonly SistemaOpcionLogica _sistemaOpcionLogica;
    private readonly EmpleadoLogica _empleadoLogica;
    private readonly PersonaLogica _personaLogica;
    private readonly EmpleadoServicio _empleados;
    private readonly BitacoraLogica _bitacora;

    public SeguridadAdminController(
        UsuarioLogica usuarioLogica,
        Usuario_RolLogica usuarioRolLogica,
        RolLogica rolLogica,
        Rol_AccesoLogica rolAccesoLogica,
        SistemaOpcionLogica sistemaOpcionLogica,
        EmpleadoLogica empleadoLogica,
        PersonaLogica personaLogica,
        EmpleadoServicio empleados,
        BitacoraLogica bitacora)
    {
        _usuarioLogica = usuarioLogica;
        _usuarioRolLogica = usuarioRolLogica;
        _rolLogica = rolLogica;
        _rolAccesoLogica = rolAccesoLogica;
        _sistemaOpcionLogica = sistemaOpcionLogica;
        _empleadoLogica = empleadoLogica;
        _personaLogica = personaLogica;
        _empleados = empleados;
        _bitacora = bitacora;
    }

    // ---------- Usuarios ----------

    public async Task<IActionResult> Usuarios(string? q)
    {
        var usuarios = await _usuarioLogica.Listar();
        var empleados = await _empleadoLogica.Listar();
        var personas = await _personaLogica.Listar();
        var relaciones = await _usuarioRolLogica.Listar();
        var roles = await _rolLogica.Listar();

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
                : persona.NombreCompleto();

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
            && await _usuarioLogica.ObtenerPorLogin(modelo.Login.Trim()) is not null)
            ModelState.AddModelError("", "Ya existe un usuario con ese login.");

        if (ModelState.IsValid)
        {
            modelo.Login = modelo.Login!.Trim();
            modelo.PasswordHash = new PasswordHasher<Usuario>().HashPassword(modelo, clave);
            modelo.Fecha = DateTime.Now;
            var guardo = await _usuarioLogica.Registrar(modelo);
            if (guardo)
            {
                TempData["MensajeExito"] = "Usuario creado.";
                await _bitacora.Registrar(User.Identity?.Name ?? "?", "Crear", "Usuario", $"login={modelo.Login}");
                return RedirectToAction(nameof(Usuarios));
            }
            ModelState.AddModelError("", "No se pudo crear el usuario: intente nuevamente.");
        }

        ViewBag.Empleados = await EmpleadosAsync();
        return View("UsuarioForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> UsuarioEditar(int id)
    {
        var usuario = await _usuarioLogica.Obtener(id);
        if (usuario is null)
            return NotFound();

        ViewBag.Empleados = await EmpleadosAsync();
        return View("UsuarioForm", usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UsuarioEditar(Usuario modelo, string? clave)
    {
        var actual = await _usuarioLogica.Obtener(modelo.IdUsuario);
        if (actual is null)
            return NotFound();

        if (!string.IsNullOrEmpty(actual.Login) && !string.IsNullOrEmpty(modelo.Login)
            && actual.Login.Trim() != modelo.Login.Trim()
            && await _usuarioLogica.ObtenerPorLogin(modelo.Login.Trim()) is not null)
            ModelState.AddModelError("", "Ya existe un usuario con ese login.");

        if (ModelState.IsValid)
        {
            actual.Login = modelo.Login?.Trim();
            actual.Descripcion = modelo.Descripcion;
            actual.Estado = modelo.Estado;
            actual.IdEmpleado = modelo.IdEmpleado;
            if (!string.IsNullOrWhiteSpace(clave))
                actual.PasswordHash = new PasswordHasher<Usuario>().HashPassword(actual, clave);
            await _usuarioLogica.Actualizar(actual);
            TempData["MensajeExito"] = "Usuario actualizado.";
            await _bitacora.Registrar(User.Identity?.Name ?? "?", "Editar", "Usuario", $"id={actual.IdUsuario}, login={actual.Login}");
            return RedirectToAction(nameof(Usuarios));
        }

        ViewBag.Empleados = await EmpleadosAsync();
        return View("UsuarioForm", modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UsuarioEliminar(int id)
    {
        var actual = await _usuarioLogica.Obtener(id);
        if (actual is not null)
        {
            var relaciones = (await _usuarioRolLogica.Listar()).Where(r => r.IdUsuario == id).ToList();
            foreach (var r in relaciones)
                await _usuarioRolLogica.Eliminar(r);
            await _usuarioLogica.Eliminar(actual);
            TempData["MensajeExito"] = "Usuario eliminado.";
            await _bitacora.Registrar(User.Identity?.Name ?? "?", "Eliminar", "Usuario", $"id={id}, login={actual.Login}");
        }
        return RedirectToAction(nameof(Usuarios));
    }

    // ---------- Roles del usuario ----------

    [HttpGet]
    public async Task<IActionResult> UsuarioRoles(int id)
    {
        var usuario = await _usuarioLogica.Obtener(id);
        if (usuario is null)
            return NotFound();

        var roles = await _rolLogica.Listar();
        var asignados = (await _usuarioRolLogica.Listar()).Where(r => r.IdUsuario == id).Select(r => r.IdRol).ToHashSet();

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
        var usuario = await _usuarioLogica.Obtener(id);
        if (usuario is null)
            return NotFound();

        var listaRoles = await _rolLogica.Listar();
        var actuales = (await _usuarioRolLogica.Listar()).Where(r => r.IdUsuario == id).ToList();

        foreach (var r in actuales.Where(r => !roles.Contains(r.IdRol)))
            await _usuarioRolLogica.Eliminar(r);

        foreach (var r in roles)
        {
            if (actuales.Any(a => a.IdRol == r))
                continue;
            await _usuarioRolLogica.Crear(new Usuario_Rol { IdUsuario = id, IdRol = r });
        }

        TempData["MensajeExito"] = "Roles del usuario actualizados.";
        return RedirectToAction(nameof(Usuarios));
    }

    // ---------- Roles ----------

    public async Task<IActionResult> Roles()
    {
        var roles = await _rolLogica.Listar();
        var usuarios = await _usuarioLogica.Listar();
        var relaciones = await _usuarioRolLogica.Listar();

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
            var guardo = await _rolLogica.Registrar(modelo);
            if (guardo)
            {
                TempData["MensajeExito"] = "Rol creado.";
                await _bitacora.Registrar(User.Identity?.Name ?? "?", "Crear", "Rol", $"id={modelo.IdRol}, descripcion={modelo.Descripcion}");
                return RedirectToAction(nameof(Roles));
            }
            ModelState.AddModelError("", "No se pudo crear el rol: intente nuevamente.");
        }
        return View("RolForm", modelo);
    }

    [HttpGet]
    public async Task<IActionResult> RolEditar(int id)
    {
        var rol = await _rolLogica.Obtener(id);
        if (rol is null)
            return NotFound();
        return View("RolForm", rol);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RolEditar(Rol modelo)
    {
        var actual = await _rolLogica.Obtener(modelo.IdRol);
        if (actual is null)
            return NotFound();

        actual.Descripcion = modelo.Descripcion;
        actual.IdSistema = modelo.IdSistema;
        await _rolLogica.Actualizar(actual);
        TempData["MensajeExito"] = "Rol actualizado.";
        await _bitacora.Registrar(User.Identity?.Name ?? "?", "Editar", "Rol", $"id={actual.IdRol}, descripcion={actual.Descripcion}");
        return RedirectToAction(nameof(Roles));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RolEliminar(int id)
    {
        var actual = await _rolLogica.Obtener(id);
        if (actual is not null)
        {
            var rel = (await _usuarioRolLogica.Listar()).Where(r => r.IdRol == id).ToList();
            foreach (var r in rel)
                await _usuarioRolLogica.Eliminar(r);

            var accesos = (await _rolAccesoLogica.Listar()).Where(a => a.IdRol == id).ToList();
            foreach (var a in accesos)
                await _rolAccesoLogica.Eliminar(a);

            await _rolLogica.Eliminar(actual);
            TempData["MensajeExito"] = "Rol eliminado.";
            await _bitacora.Registrar(User.Identity?.Name ?? "?", "Eliminar", "Rol", $"id={id}, descripcion={actual.Descripcion}");
        }
        return RedirectToAction(nameof(Roles));
    }

    // ---------- Accesos del rol ----------

    [HttpGet]
    public async Task<IActionResult> RolAccesos(int id)
    {
        var rol = await _rolLogica.Obtener(id);
        if (rol is null)
            return NotFound();

        var opciones = await _sistemaOpcionLogica.Listar();
        var actuales = (await _rolAccesoLogica.Listar()).Where(a => a.IdRol == id).ToList();

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
        var rol = await _rolLogica.Obtener(id);
        if (rol is null)
            return NotFound();

        var actuales = (await _rolAccesoLogica.Listar()).Where(a => a.IdRol == id).ToList();

        foreach (var a in actuales.Where(a => !opciones.Contains(a.IdSistemaOpcion ?? "")))
            await _rolAccesoLogica.Eliminar(a);

        foreach (var o in opciones)
        {
            if (actuales.Any(a => a.IdSistemaOpcion == o))
                continue;
            await _rolAccesoLogica.Crear(new Rol_Acceso { IdRol = id, IdSistemaOpcion = o, Permiso = "A" });
        }

        TempData["MensajeExito"] = "Permisos del rol actualizados.";
        return RedirectToAction(nameof(Roles));
    }

    // ---------- Bitacora de actividad ----------

    public async Task<IActionResult> Actividad()
    {
        var registros = await _bitacora.Ultimas(100);
        return View(registros);
    }

    // ---------- Utilidades ----------

    private async Task<List<(int Id, string Nombre)>> EmpleadosAsync()
        => await _empleados.EmpleadosAsync();
}