using Capa_Datos;
using Capa_Entidades;
using Capa_Logica.Tests.Soporte;

namespace Capa_Logica.Tests;

// Fija la regla de numeracion que antes vivia en cada controller: "maximo + 1", o 1 si no hay registros.
public class NumeracionTests
{
    [Fact]
    public async Task Tabla_vacia_empieza_en_1()
    {
        using var bd = new BaseDatosPrueba();
        var logica = new RRHH_FeriadoLogica(new RRHH_FeriadoData(bd.Contexto), new SecuenciaService(bd.Contexto));

        var feriado = new RRHH_Feriado { Motivo = "Año nuevo" };
        Assert.True(await logica.Registrar(feriado));

        Assert.Equal(1, feriado.idFeriado);
    }

    [Fact]
    public async Task Numeracion_global_toma_el_maximo_mas_uno_aunque_haya_huecos()
    {
        using var bd = new BaseDatosPrueba();
        await bd.SembrarAsync(new RRHH_Feriado { idFeriado = 3 }, new RRHH_Feriado { idFeriado = 8 });
        var logica = new RRHH_FeriadoLogica(new RRHH_FeriadoData(bd.Contexto), new SecuenciaService(bd.Contexto));

        var feriado = new RRHH_Feriado();
        await logica.Registrar(feriado);

        Assert.Equal(9, feriado.idFeriado);
    }

    [Fact]
    public async Task Permiso_se_numera_por_empleado()
    {
        using var bd = new BaseDatosPrueba();
        await bd.SembrarAsync(
            new Permiso { IdEmpleado = 1, NPermiso = 1 },
            new Permiso { IdEmpleado = 1, NPermiso = 2 },
            new Permiso { IdEmpleado = 2, NPermiso = 15 });
        var logica = new PermisoLogica(new PermisoData(bd.Contexto), new SecuenciaService(bd.Contexto));

        var delEmpleado1 = new Permiso { IdEmpleado = 1 };
        var delEmpleado3 = new Permiso { IdEmpleado = 3 };
        await logica.Registrar(delEmpleado1);
        await logica.Registrar(delEmpleado3);

        Assert.Equal(3, delEmpleado1.NPermiso);
        Assert.Equal(1, delEmpleado3.NPermiso);
    }

    [Fact]
    public async Task Area_organizacional_se_numera_dentro_del_anio()
    {
        using var bd = new BaseDatosPrueba();
        await bd.SembrarAsync(
            new EstructOrganiz { Year = 2024, idAreaOrganiz = 40, AreaOrganizacional = "A", Abrev = "A", Sigla = "A" },
            new EstructOrganiz { Year = 2025, idAreaOrganiz = 5, AreaOrganizacional = "B", Abrev = "B", Sigla = "B" });
        var logica = new EstructOrganizLogica(new EstructOrganizData(bd.Contexto), new SecuenciaService(bd.Contexto));

        var area = new EstructOrganiz { Year = 2025, AreaOrganizacional = "C", Abrev = "C", Sigla = "C" };
        await logica.Registrar(area);

        Assert.Equal(6, area.idAreaOrganiz);
    }

    [Fact]
    public async Task El_registro_numerado_queda_guardado()
    {
        using var bd = new BaseDatosPrueba();
        var logica = new PermisoLogica(new PermisoData(bd.Contexto), new SecuenciaService(bd.Contexto));

        await logica.Registrar(new Permiso { IdEmpleado = 4, Obs = "cita medica" });

        Assert.Equal("cita medica", (await logica.Obtener(4, 1))?.Obs);
    }
}

public class AltaEmpleadoTests
{
    [Fact]
    public async Task RegistrarConPersona_numera_ambos_los_enlaza_y_marca_activo()
    {
        using var bd = new BaseDatosPrueba();
        await bd.SembrarAsync(new Persona { IdPersona = 50 }, new Empleado { IdEmpleado = 300, IdPersona = 50 });
        var secuencia = new SecuenciaService(bd.Contexto);
        var personaLogica = new PersonaLogica(new PersonaData(bd.Contexto), secuencia);
        var logica = new EmpleadoLogica(new EmpleadoData(bd.Contexto), personaLogica, secuencia);

        var persona = new Persona { Nombres = "Luis" };
        var empleado = new Empleado { Estado = " " };
        Assert.True(await logica.RegistrarConPersona(persona, empleado));

        Assert.Equal(51, persona.IdPersona);
        Assert.NotNull(persona.FechaRegistro);
        Assert.Equal(301, empleado.IdEmpleado);
        Assert.Equal(51, empleado.IdPersona);
        Assert.Equal("A", empleado.Estado);
    }
}
