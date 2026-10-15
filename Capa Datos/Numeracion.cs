using System.Linq.Expressions;
using System.Reflection;
using Capa_Entidades;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

// Claves que genera el sistema con "maximo + 1" (1 si no hay registros), como en la BD legada.
// Es un detalle de esta base de datos: con una BD que use IDENTITY, este archivo cambia
// y la Capa Logica no se entera.
internal static class Numeracion
{
    private static readonly Dictionary<Type, object> Reglas = new();

    static Numeracion()
    {
        Global<Capacitacion>(x => x.IdCapacitacion);
        Global<Comportamiento>(x => x.idComportamiento);
        Global<Empleado>(x => x.IdEmpleado);
        Global<Institucion>(x => x.IdInstitucion);
        Global<Local>(x => x.idLocal);
        Global<Persona>(x => x.IdPersona);
        Global<ResponsableXUO>(x => x.IdResponsable);
        Global<Rol>(x => x.IdRol);
        Global<RRHH_AcudeEnferm>(x => x.idAcudeEnferm);
        Global<RRHH_AspSocio>(x => x.idAspSocio);
        Global<RRHH_DinamicaFamiliar>(x => x.idDinamica);
        Global<RRHH_Feriado>(x => x.idFeriado);
        Global<RRHH_FuncFam>(x => x.idFuncFamiliar);
        Global<RRHH_Salud>(x => x.idSalud);
        Global<Usuario>(x => x.IdUsuario);
        Global<Vivienda>(x => x.IdVivienda);

        // Correlativos dentro de un grupo: el maximo se toma solo entre los registros del grupo.
        PorGrupo<EstructOrganiz>(x => x.idAreaOrganiz, r => x => x.Year == r.Year);
        PorGrupo<EstudiosRealizado>(x => x.Correlativo, r => x => x.IdTipoEstudios == r.IdTipoEstudios && x.IdEmpleado == r.IdEmpleado);
        PorGrupo<ExpLaboral>(x => x.IdExpLab, r => x => x.IdEmpleado == r.IdEmpleado);
        PorGrupo<HorarioTemporal>(x => x.N, r => x => x.idEmpleado == r.idEmpleado);
        PorGrupo<Inconveniente>(x => x.IdInconveniente, r => x => x.IdObjetivo == r.IdObjetivo && x.Id_Actividad == r.Id_Actividad);
        PorGrupo<PeriodoLaboral>(x => x.NroPeriodo, r => x => x.IdEmpleado == r.IdEmpleado);
        PorGrupo<Permiso>(x => x.NPermiso, r => x => x.IdEmpleado == r.IdEmpleado);
        PorGrupo<Resolucion>(x => x.idResolucion, r => x => x.idEmpleado == r.idEmpleado);
        PorGrupo<Rotacion>(x => x.idRotacion, r => x => x.idEmpleado == r.idEmpleado);
        PorGrupo<RRHH_Asegurado>(x => x.NroBeneficiario, r => x => x.idEmpleado == r.idEmpleado);
    }

    public static ReglaNumeracion<T>? Para<T>() where T : class
        => Reglas.TryGetValue(typeof(T), out var regla) ? (ReglaNumeracion<T>)regla : null;

    private static void Global<T>(Expression<Func<T, int>> campo) where T : class
        => Reglas[typeof(T)] = new ReglaNumeracion<T>(campo, null);

    private static void PorGrupo<T>(Expression<Func<T, int>> campo, Func<T, Expression<Func<T, bool>>> grupo) where T : class
        => Reglas[typeof(T)] = new ReglaNumeracion<T>(campo, grupo);
}

internal sealed class ReglaNumeracion<T> where T : class
{
    private readonly Expression<Func<T, int?>> _campo;
    private readonly PropertyInfo _propiedad;
    private readonly Func<T, Expression<Func<T, bool>>>? _grupo;

    public ReglaNumeracion(Expression<Func<T, int>> campo, Func<T, Expression<Func<T, bool>>>? grupo)
    {
        // Se proyecta como int? para que MAX sobre una tabla vacia devuelva NULL en lugar de fallar.
        _campo = Expression.Lambda<Func<T, int?>>(Expression.Convert(campo.Body, typeof(int?)), campo.Parameters);
        _propiedad = (PropertyInfo)((MemberExpression)campo.Body).Member;
        _grupo = grupo;
    }

    // Siempre recalcula: si la transaccion se reintenta, el registro recibe un numero vigente.
    public async Task AsignarAsync(DbSet<T> tabla, T registro)
    {
        var consulta = _grupo is null ? tabla : tabla.Where(_grupo(registro));
        var maximo = await consulta.MaxAsync(_campo);
        _propiedad.SetValue(registro, (maximo ?? 0) + 1);
    }
}
