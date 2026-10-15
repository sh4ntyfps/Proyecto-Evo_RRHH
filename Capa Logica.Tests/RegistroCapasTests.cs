using Capa_Datos;
using Capa_Entidades.Contratos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_Evo_RRLL.Utilidades;

namespace Capa_Logica.Tests;

// Protege el registro automatico: cada dependencia que piden los controllers debe poder resolverse.
public class RegistroCapasTests
{
    public static IEnumerable<object[]> Controladores() =>
        typeof(RegistroCapas).Assembly.GetTypes()
            .Where(t => typeof(Controller).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(t => new object[] { t.Name });

    private static ServiceCollection ServiciosCompletos()
    {
        var servicios = new ServiceCollection();
        servicios.AgregarCapaDatos(o => o.UseSqlite("DataSource=:memory:"));
        servicios.AgregarServiciosDeAplicacion();
        return servicios;
    }

    [Theory]
    [MemberData(nameof(Controladores))]
    public void Todas_las_dependencias_del_controlador_estan_registradas(string controlador)
    {
        using var proveedor = ServiciosCompletos().BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });
        using var alcance = proveedor.CreateScope();

        var tipo = typeof(RegistroCapas).Assembly.GetTypes().Single(t => t.Name == controlador);
        foreach (var parametro in tipo.GetConstructors().Single().GetParameters())
            Assert.NotNull(alcance.ServiceProvider.GetService(parametro.ParameterType));
    }

    [Fact]
    public void Registra_todas_las_clases_de_datos_y_de_logica()
    {
        var servicios = ServiciosCompletos();

        // 76 repositorios genericos + bitacora + catalogo, cada uno por los contratos que implementa.
        Assert.Equal(78, servicios.Where(s => s.ServiceType.Namespace == typeof(IRepositorio<>).Namespace
            && s.ServiceType != typeof(IUnidadDeTrabajo)).Select(s => s.ImplementationType).Distinct().Count());
        Assert.Equal(79, servicios.Count(s => s.ImplementationType?.Name.EndsWith("Logica") == true));
    }
}
