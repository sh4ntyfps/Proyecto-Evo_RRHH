using Capa_Datos;
using Capa_Entidades;
using Capa_Logica.Tests.Soporte;
using Microsoft.EntityFrameworkCore;

namespace Capa_Logica.Tests;

// Fija la regla de numeracion que antes vivia en cada controller: "maximo + 1", o 1 si no hay registros.
public class NumeracionTests
{
    [Fact]
    public async Task Tabla_vacia_empieza_en_1()
    {
        using var bd = new BaseDatosPrueba();
        var logica = new RRHH_FeriadoLogica(new RRHH_FeriadoData(bd.Contexto), new UnidadDeTrabajo(bd.Contexto));

        var feriado = new RRHH_Feriado { Motivo = "Año nuevo" };
        Assert.True(await logica.Registrar(feriado));

        Assert.Equal(1, feriado.idFeriado);
    }

    [Fact]
    public async Task Numeracion_global_toma_el_maximo_mas_uno_aunque_haya_huecos()
    {
        using var bd = new BaseDatosPrueba();
        await bd.SembrarAsync(new RRHH_Feriado { idFeriado = 3 }, new RRHH_Feriado { idFeriado = 8 });
        var logica = new RRHH_FeriadoLogica(new RRHH_FeriadoData(bd.Contexto), new UnidadDeTrabajo(bd.Contexto));

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
        var logica = new PermisoLogica(new PermisoData(bd.Contexto), new UnidadDeTrabajo(bd.Contexto));

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
        var logica = new EstructOrganizLogica(new EstructOrganizData(bd.Contexto), new UnidadDeTrabajo(bd.Contexto));

        var area = new EstructOrganiz { Year = 2025, AreaOrganizacional = "C", Abrev = "C", Sigla = "C" };
        await logica.Registrar(area);

        Assert.Equal(6, area.idAreaOrganiz);
    }

    [Fact]
    public async Task El_registro_numerado_queda_guardado()
    {
        using var bd = new BaseDatosPrueba();
        var logica = new PermisoLogica(new PermisoData(bd.Contexto), new UnidadDeTrabajo(bd.Contexto));

        await logica.Registrar(new Permiso { IdEmpleado = 4, Obs = "cita medica" });

        Assert.Equal("cita medica", (await logica.Obtener(4, 1))?.Obs);
    }

    [Fact]
    public async Task Crear_tambien_numera_porque_la_clave_la_genera_la_capa_de_datos()
    {
        using var bd = new BaseDatosPrueba();
        await bd.SembrarAsync(new RRHH_Asegurado { idEmpleado = 7, NroBeneficiario = 2 });
        var logica = new RRHH_AseguradoLogica(new RRHH_AseguradoData(bd.Contexto), new UnidadDeTrabajo(bd.Contexto));

        var asegurado = new RRHH_Asegurado { idEmpleado = 7 };
        await logica.Crear(asegurado);

        Assert.Equal(3, asegurado.NroBeneficiario);
    }
}

public class AltaEmpleadoTests
{
    [Fact]
    public async Task RegistrarConPersona_numera_ambos_los_enlaza_y_marca_activo()
    {
        using var bd = new BaseDatosPrueba();
        await bd.SembrarAsync(new Persona { IdPersona = 50 }, new Empleado { IdEmpleado = 300, IdPersona = 50 });
        var unidad = new UnidadDeTrabajo(bd.Contexto);
        var personaLogica = new PersonaLogica(new PersonaData(bd.Contexto), unidad);
        var logica = new EmpleadoLogica(new EmpleadoData(bd.Contexto), personaLogica, unidad);

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

public class FichaSocialTests
{
    [Fact]
    public async Task Guardar_crea_las_tres_partes_y_luego_las_actualiza()
    {
        using var bd = new BaseDatosPrueba();
        // Catalogo de preguntas de RRHH_Normalizado (tabla interna de la Capa Datos).
        await bd.Contexto.Database.ExecuteSqlRawAsync("INSERT INTO RRHH_FuncFamPregunta (IdPregunta, Codigo) VALUES (1, 'Com1')");
        var logica = new FichaSocialLogica(new RRHH_DinamicaFamiliarData(bd.Contexto), new RRHH_AspSocioData(bd.Contexto),
            new RRHH_FuncFamData(bd.Contexto), new UnidadDeTrabajo(bd.Contexto));

        Assert.True(await logica.Guardar(5, new RRHH_DinamicaFamiliar { Observacion = "primera" }, new RRHH_AspSocio { hobby = "futbol" }, new RRHH_FuncFam { Com1 = "S" }));
        Assert.True(await logica.Guardar(5, new RRHH_DinamicaFamiliar { Observacion = "segunda" }, new RRHH_AspSocio { hobby = "ajedrez" }, new RRHH_FuncFam { Com1 = "N" }));

        var dinamica = Assert.Single(await new RRHH_DinamicaFamiliarData(bd.Contexto).Listar());
        Assert.Equal((5, 1, "segunda"), (dinamica.IdEmpleado ?? 0, dinamica.idDinamica, dinamica.Observacion));
        Assert.Equal("ajedrez", Assert.Single(await new RRHH_AspSocioData(bd.Contexto).Listar()).hobby);
        // Una evaluacion por empleado, guardada como una fila por respuesta.
        Assert.Equal((5, "N"), Assert.Single(await new RRHH_FuncFamData(bd.Contexto).Listar()) is var f ? (f.IdEmpleado ?? 0, f.Com1) : default);
    }
}
