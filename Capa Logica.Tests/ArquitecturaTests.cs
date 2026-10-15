using System.Reflection;
using Capa_Datos;
using Capa_Entidades;
using Proyecto_Evo_RRLL.Utilidades;

namespace Capa_Logica.Tests;

// Reglas de dependencia entre capas. Si alguna falla, cambiar de base de datos ya no
// quedaria limitado a la Capa Datos.
public class ArquitecturaTests
{
    private static readonly Assembly Entidades = typeof(Empleado).Assembly;
    private static readonly Assembly Logica = typeof(LogicaBase<>).Assembly;
    private static readonly Assembly Datos = typeof(EvoRRLDbContext).Assembly;
    private static readonly Assembly Web = typeof(RegistroCapas).Assembly;

    private static IEnumerable<string> Referencias(Assembly ensamblado)
        => ensamblado.GetReferencedAssemblies().Select(a => a.Name!);

    [Fact]
    public void Entidades_no_depende_de_ninguna_otra_capa_ni_de_EF()
        => Assert.DoesNotContain(Referencias(Entidades),
            r => r == Datos.GetName().Name || r == Logica.GetName().Name || r.StartsWith("Microsoft.EntityFrameworkCore"));

    [Fact]
    public void Logica_no_depende_de_la_Capa_Datos_ni_de_EF()
        => Assert.DoesNotContain(Referencias(Logica),
            r => r == Datos.GetName().Name || r.StartsWith("Microsoft.EntityFrameworkCore"));

    [Fact]
    public void Las_entidades_no_llevan_atributos_de_mapeo_a_tablas()
    {
        var conMapeo = Entidades.GetTypes()
            .Where(t => t.Namespace == typeof(Empleado).Namespace)
            .Where(t => t.GetCustomAttributes(true).Concat(t.GetProperties().SelectMany(p => p.GetCustomAttributes(true)))
                .Any(a => a is System.ComponentModel.DataAnnotations.KeyAttribute
                    || a.GetType().Namespace == "System.ComponentModel.DataAnnotations.Schema"))
            .Select(t => t.Name);

        Assert.Empty(conMapeo);
    }

    [Fact]
    public void Controllers_y_vistas_no_usan_tipos_de_la_Capa_Datos()
    {
        // Program.cs (la raiz de composicion) es el unico punto web que conoce la Capa Datos.
        var acoplados = Web.GetTypes()
            .Where(t => t.Namespace?.StartsWith("Proyecto_Evo_RRLL") == true && !t.Name.Contains("Program"))
            .Where(t => t.GetConstructors().SelectMany(c => c.GetParameters()).Select(p => p.ParameterType)
                .Concat(t.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Select(f => f.FieldType))
                .Concat(t.GetProperties().Select(p => p.PropertyType))
                .Any(tipo => tipo.Assembly == Datos))
            .Select(t => t.Name);

        Assert.Empty(acoplados);
    }
}
