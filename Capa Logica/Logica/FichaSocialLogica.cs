using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

// Ficha social de un empleado: dinamica familiar, aspectos socioeconomicos y funcionalidad familiar.
// Las tres partes se guardan juntas (todas o ninguna).
public class FichaSocialLogica
{
    private readonly IRepositorio<RRHH_DinamicaFamiliar> _dinamica;
    private readonly IRepositorio<RRHH_AspSocio> _aspSocio;
    private readonly IRepositorio<RRHH_FuncFam> _funcFam;
    private readonly IUnidadDeTrabajo _unidad;

    public FichaSocialLogica(
        IRepositorio<RRHH_DinamicaFamiliar> dinamica,
        IRepositorio<RRHH_AspSocio> aspSocio,
        IRepositorio<RRHH_FuncFam> funcFam,
        IUnidadDeTrabajo unidad)
    {
        _dinamica = dinamica;
        _aspSocio = aspSocio;
        _funcFam = funcFam;
        _unidad = unidad;
    }

    // Crea cada parte si el empleado aun no la tiene, o actualiza la existente.
    public Task<bool> Guardar(int idEmpleado, RRHH_DinamicaFamiliar dinamica, RRHH_AspSocio aspSocio, RRHH_FuncFam funcFam)
        => _unidad.EjecutarAsync(async () =>
        {
            var dinamicaActual = (await _dinamica.Listar()).FirstOrDefault(d => d.IdEmpleado == idEmpleado);
            if (dinamicaActual is null)
            {
                dinamica.IdEmpleado = idEmpleado;
                await _dinamica.Crear(dinamica);
            }
            else
            {
                CopiarDinamica(dinamicaActual, dinamica);
                await _dinamica.Actualizar(dinamicaActual);
            }

            var aspSocioActual = (await _aspSocio.Listar()).FirstOrDefault(a => a.IdEmpleado == idEmpleado);
            if (aspSocioActual is null)
            {
                aspSocio.IdEmpleado = idEmpleado;
                await _aspSocio.Crear(aspSocio);
            }
            else
            {
                CopiarAspSocio(aspSocioActual, aspSocio);
                await _aspSocio.Actualizar(aspSocioActual);
            }

            var funcFamActual = (await _funcFam.Listar()).FirstOrDefault(f => f.IdEmpleado == idEmpleado);
            if (funcFamActual is null)
            {
                funcFam.IdEmpleado = idEmpleado;
                await _funcFam.Crear(funcFam);
            }
            else
            {
                CopiarFuncFam(funcFamActual, funcFam);
                await _funcFam.Actualizar(funcFamActual);
            }
        });

    private static void CopiarDinamica(RRHH_DinamicaFamiliar destino, RRHH_DinamicaFamiliar origen)
    {
        destino.idTipoFam = origen.idTipoFam;
        destino.idRelaPareja = origen.idRelaPareja;
        destino.idRelaPH = origen.idRelaPH;
        destino.idRelacHermano = origen.idRelacHermano;
        destino.Observacion = origen.Observacion;
    }

    private static void CopiarAspSocio(RRHH_AspSocio destino, RRHH_AspSocio origen)
    {
        destino.idPregunta1 = origen.idPregunta1;
        destino.idPregunta2 = origen.idPregunta2;
        destino.idPregunta3 = origen.idPregunta3;
        destino.PrincipProblemas = origen.PrincipProblemas;
        destino.PropMejoraInstitu = origen.PropMejoraInstitu;
        destino.hobby = origen.hobby;
        destino.ActRecreaInstitu = origen.ActRecreaInstitu;
    }

    private static void CopiarFuncFam(RRHH_FuncFam destino, RRHH_FuncFam origen)
    {
        destino.Com1 = origen.Com1;
        destino.Com2 = origen.Com2;
        destino.Afec1 = origen.Afec1;
        destino.Afec2 = origen.Afec2;
        destino.Decis1 = origen.Decis1;
        destino.Decis2 = origen.Decis2;
        destino.Soc1 = origen.Soc1;
        destino.Soc2 = origen.Soc2;
        destino.Soc3 = origen.Soc3;
    }
}
