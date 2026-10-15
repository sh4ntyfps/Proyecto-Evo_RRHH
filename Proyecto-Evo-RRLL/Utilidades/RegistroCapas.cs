using System.Reflection;
using Capa_Logica;
using Capa_Logica.Exportacion;

namespace Proyecto_Evo_RRLL.Utilidades;

internal static class RegistroCapas
{
    // Composicion de la logica y de los servicios web (IoC). La Capa Datos se registra aparte,
    // en Program.cs con AgregarCapaDatos: es la unica pieza que cambia si cambia la base de datos.
    internal static IServiceCollection AgregarServiciosDeAplicacion(this IServiceCollection servicios)
    {
        // Toda clase *Logica se registra sola: una tabla nueva no obliga a editar este archivo.
        servicios.AgregarClasesDeCapa(typeof(LogicaBase<>).Assembly, "Logica");

        servicios.AddScoped<EmpleadoServicio>();

        // Formatos de exportacion: un formato nuevo solo agrega su clase y su registro aqui (OCP).
        servicios.AddSingleton<IExportadorTabla, ExportadorCsv>();
        servicios.AddSingleton<ExportadorTablas>();
        return servicios;
    }

    // Registra como Scoped toda clase publica concreta cuyo nombre termine en el sufijo indicado.
    internal static IServiceCollection AgregarClasesDeCapa(this IServiceCollection servicios, Assembly ensamblado, string sufijo)
    {
        var tipos = ensamblado.GetExportedTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith(sufijo, StringComparison.Ordinal));

        foreach (var tipo in tipos)
            servicios.AddScoped(tipo);

        return servicios;
    }
}
