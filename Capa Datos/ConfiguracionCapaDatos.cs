using Capa_Entidades.Contratos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Capa_Datos;

// Unico punto donde la aplicacion conoce la Capa Datos: registra la BD y la implementacion
// de cada contrato de Capa_Entidades.Contratos. Cambiar de base de datos = cambiar esta capa.
public static class ConfiguracionCapaDatos
{
    public static IServiceCollection AgregarCapaDatos(this IServiceCollection servicios, string cadenaConexion)
        => servicios.AgregarCapaDatos(o => o.UseSqlServer(cadenaConexion));

    public static IServiceCollection AgregarCapaDatos(this IServiceCollection servicios, Action<DbContextOptionsBuilder> configurarBd)
    {
        servicios.AddDbContext<EvoRRLDbContext>(configurarBd);
        servicios.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

        // Cada clase publica de esta capa se registra por los contratos que implementa:
        // una tabla nueva solo agrega su clase *Data.
        var clases = typeof(ConfiguracionCapaDatos).Assembly.GetExportedTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t != typeof(UnidadDeTrabajo));
        foreach (var clase in clases)
        {
            foreach (var contrato in clase.GetInterfaces().Where(i => i.Namespace == typeof(IUnidadDeTrabajo).Namespace))
                servicios.AddScoped(contrato, clase);
        }
        return servicios;
    }
}
