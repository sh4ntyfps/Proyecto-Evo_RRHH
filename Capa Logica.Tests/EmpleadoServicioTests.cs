using Capa_Datos;
using Capa_Entidades;
using Capa_Logica.Tests.Soporte;

namespace Capa_Logica.Tests;

// Caracterizacion de EmpleadoServicio: se escribio antes de moverlo a Capa Logica y sigue en verde despues.
public class EmpleadoServicioTests
{
    // Dobles de prueba: implementan los contratos con datos en memoria, sin SQL Server.
    private sealed class EmpleadoDataFalso(params Empleado[] empleados) : IEmpleadoData
    {
        public Task<List<Empleado>> Listar() => Task.FromResult(empleados.ToList());
    }

    private sealed class PersonaDataFalso(params Persona[] personas) : IPersonaData
    {
        public Task<List<Persona>> Listar() => Task.FromResult(personas.ToList());
    }

    [Fact]
    public async Task EmpleadosAsync_arma_nombre_completo_y_ordena_alfabeticamente()
    {
        var servicio = new EmpleadoServicio(
            new EmpleadoDataFalso(new Empleado { IdEmpleado = 10, IdPersona = 1 }, new Empleado { IdEmpleado = 20, IdPersona = 2 }),
            new PersonaDataFalso(
                new Persona { IdPersona = 1, Nombres = "Zoila", Apellido_Paterno = "Rojas", Apellido_Materno = "Paz" },
                new Persona { IdPersona = 2, Nombres = "Ana", Apellido_Paterno = "Lopez", Apellido_Materno = null }));

        Assert.Equal(new[] { (20, "Ana Lopez"), (10, "Zoila Rojas Paz") }, await servicio.EmpleadosAsync());
    }

    [Fact]
    public async Task EmpleadosAsync_sin_persona_usa_texto_con_el_id()
    {
        var servicio = new EmpleadoServicio(
            new EmpleadoDataFalso(new Empleado { IdEmpleado = 7, IdPersona = null }), new PersonaDataFalso());

        Assert.Equal("(Empleado 7)", await servicio.NombreEmpleadoAsync(7));
    }

    [Fact]
    public async Task NombreEmpleadoAsync_de_un_id_inexistente_devuelve_null()
    {
        var servicio = new EmpleadoServicio(new EmpleadoDataFalso(), new PersonaDataFalso());

        Assert.Null(await servicio.NombreEmpleadoAsync(99));
    }

    [Fact]
    public async Task Con_las_clases_reales_de_datos_el_resultado_es_el_mismo()
    {
        using var bd = new BaseDatosPrueba();
        await bd.SembrarAsync(
            new Persona { IdPersona = 1, Nombres = "Zoila", Apellido_Paterno = "Rojas", Apellido_Materno = "Paz" },
            new Empleado { IdEmpleado = 10, IdPersona = 1 });

        var servicio = new EmpleadoServicio(new EmpleadoData(bd.Contexto), new PersonaData(bd.Contexto));

        Assert.Equal("Zoila Rojas Paz", await servicio.NombreEmpleadoAsync(10));
    }
}
