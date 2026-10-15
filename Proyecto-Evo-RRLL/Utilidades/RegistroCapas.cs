using System.Reflection;
using Capa_Datos;
using Capa_Logica;
using Capa_Logica.Exportacion;

namespace Proyecto_Evo_RRLL.Utilidades;

internal static class RegistroCapas
{
    // Unico punto de composicion de dependencias (IoC): Program.cs y las pruebas usan este mismo registro.
    internal static IServiceCollection AgregarServiciosDeAplicacion(this IServiceCollection servicios)
    {
        // Toda clase *Data y *Logica se registra sola: una tabla nueva no obliga a editar este archivo.
        servicios.AgregarClasesDeCapa(typeof(RepositorioBase<>).Assembly, "Data");
        servicios.AgregarClasesDeCapa(typeof(LogicaBase<>).Assembly, "Logica");

        servicios.AddScoped<CatalogoRepositorio>();
        servicios.AddScoped<SecuenciaService>();
        servicios.AddScoped<IEmpleadoData>(sp => sp.GetRequiredService<EmpleadoData>());
        servicios.AddScoped<IPersonaData>(sp => sp.GetRequiredService<PersonaData>());
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
