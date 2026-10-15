using Capa_Datos;
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

    [Theory]
    [MemberData(nameof(Controladores))]
    public void Todas_las_dependencias_del_controlador_estan_registradas(string controlador)
    {
        var servicios = new ServiceCollection();
        servicios.AddDbContext<EvoRRLDbContext>(o => o.UseSqlite("DataSource=:memory:"));
        servicios.AgregarServiciosDeAplicacion();
        using var proveedor = servicios.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });
        using var alcance = proveedor.CreateScope();

        var tipo = typeof(RegistroCapas).Assembly.GetTypes().Single(t => t.Name == controlador);
        foreach (var parametro in tipo.GetConstructors().Single().GetParameters())
            Assert.NotNull(alcance.ServiceProvider.GetService(parametro.ParameterType));
    }

    [Fact]
    public void Registra_todas_las_clases_de_datos_y_de_logica()
    {
        var servicios = new ServiceCollection().AgregarServiciosDeAplicacion();

        Assert.Equal(77, servicios.Count(s => s.ImplementationType?.Name.EndsWith("Data") == true));
        Assert.Equal(77, servicios.Count(s => s.ImplementationType?.Name.EndsWith("Logica") == true));
    }
}
