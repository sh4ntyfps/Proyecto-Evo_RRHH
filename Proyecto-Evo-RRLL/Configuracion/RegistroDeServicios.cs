using System.Reflection;
using Capa_Datos;
using Capa_Logica;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Proyecto_Evo_RRLL.Servicios;

namespace Proyecto_Evo_RRLL.Configuracion;

/// <summary>
/// Registro de dependencias de la aplicacion, agrupado por responsabilidad.
/// Program.cs antes enumeraba a mano las 76 clases de datos y las 76 de logica: 152
/// lineas identicas que habia que recordar actualizar al agregar una entidad.
/// Aqui se registran por convencion (DRY) y Program.cs vuelve a ocuparse solo de
/// construir y ejecutar la aplicacion (SRP).
/// </summary>
public static class RegistroDeServicios
{
    public static IServiceCollection AgregarBaseDeDatos(
        this IServiceCollection servicios, IConfiguration configuracion)
    {
        servicios.AddDbContext<EvoRRLDbContext>(opciones =>
            opciones.UseSqlServer(configuracion.GetConnectionString("RRHHNuevo")));
        return servicios;
    }

    public static IServiceCollection AgregarAutenticacionPorCookies(this IServiceCollection servicios)
    {
        servicios.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(opciones =>
            {
                opciones.LoginPath = "/Seguridad/Login";
                opciones.AccessDeniedPath = "/Seguridad/AccesoDenegado";
                opciones.ExpireTimeSpan = TimeSpan.FromHours(8);
                opciones.SlidingExpiration = true;
            });
        servicios.AddAuthorization(opciones =>
            opciones.AddPolicy("SoloAdministradores", p => p.RequireRole("Administrador")));
        return servicios;
    }

    /// <summary>
    /// Registra la Capa Datos y la Capa Logica por convencion: toda clase concreta que
    /// herede de RepositorioBase o de LogicaBase. Agregar una entidad nueva ya no
    /// obliga a tocar este archivo.
    /// </summary>
    public static IServiceCollection AgregarCapasDeNegocio(this IServiceCollection servicios)
    {
        foreach (var tipo in DescendientesDe(typeof(RepositorioBase<>), typeof(CatalogoRepositorio).Assembly))
            servicios.AddScoped(tipo);

        // Repositorio generico de catalogos: no deriva de RepositorioBase porque no trabaja
        // sobre una sola entidad, sino sobre la tabla que indique la ruta.
        servicios.AddScoped<CatalogoRepositorio>();
        servicios.AddScoped<SecuenciaService>();

        foreach (var tipo in DescendientesDe(typeof(LogicaBase<,>), typeof(LogicaBase<,>).Assembly))
            servicios.AddScoped(tipo);

        // Bitacora no es un CRUD (solo registra y consulta), por eso no hereda de las bases.
        servicios.AddScoped<BitacoraData>();
        servicios.AddScoped<BitacoraLogica>();

        return servicios;
    }

    public static IServiceCollection AgregarServiciosDeAplicacion(this IServiceCollection servicios)
    {
        servicios.AddSingleton<ServicioClaves>();
        servicios.AddScoped<Proyecto_Evo_RRLL.Services.EmpleadoServicio>();
        servicios.AddScoped<SembradorAdministrador>();
        return servicios;
    }

    private static IEnumerable<Type> DescendientesDe(Type definicionGenerica, Assembly ensamblado) =>
        ensamblado.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && Hereda(t, definicionGenerica));

    private static bool Hereda(Type tipo, Type definicionGenerica)
    {
        for (var padre = tipo.BaseType; padre is not null; padre = padre.BaseType)
            if (padre.IsGenericType && padre.GetGenericTypeDefinition() == definicionGenerica)
                return true;
        return false;
    }
}
