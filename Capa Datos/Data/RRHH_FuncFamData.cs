using Capa_Entidades;
using Capa_Entidades.Contratos;
using Microsoft.EntityFrameworkCore;

namespace Capa_Datos;

// En RRHH_Normalizado la funcionalidad familiar es una fila por respuesta
// (RRHH_FuncFamRespuesta, una por pregunta Com1..Soc3) y hay una evaluacion por empleado.
// Este repositorio arma y desarma el RRHH_FuncFam de siempre; su id es el IdEmpleado.
public class RRHH_FuncFamData : IRepositorio<RRHH_FuncFam>
{
    private static readonly string[] Codigos = { "Com1", "Com2", "Afec1", "Afec2", "Decis1", "Decis2", "Soc1", "Soc2", "Soc3" };

    private readonly EvoRRLDbContext _contexto;

    public RRHH_FuncFamData(EvoRRLDbContext contexto) => _contexto = contexto;

    private DbSet<FuncFamRespuestaFila> Respuestas => _contexto.Set<FuncFamRespuestaFila>();

    public async Task<List<RRHH_FuncFam>> Listar() => await LeerAsync(null);

    public async Task<RRHH_FuncFam?> Obtener(params object[] claves)
        => (await LeerAsync(Convert.ToInt32(claves[0]))).FirstOrDefault();

    public async Task Crear(RRHH_FuncFam registro)
    {
        registro.IdEmpleado ??= registro.idFuncFamiliar;
        registro.idFuncFamiliar = registro.IdEmpleado ?? 0;
        await GuardarAsync(registro);
    }

    public Task Actualizar(RRHH_FuncFam registro) => GuardarAsync(registro);

    public async Task Eliminar(RRHH_FuncFam registro)
    {
        var idEmpleado = registro.IdEmpleado ?? registro.idFuncFamiliar;
        Respuestas.RemoveRange(await Respuestas.Where(r => r.IdEmpleado == idEmpleado).ToListAsync());
        await _contexto.SaveChangesAsync();
    }

    private async Task<List<RRHH_FuncFam>> LeerAsync(int? idEmpleado)
    {
        var preguntas = await PreguntasAsync();
        var filas = await Respuestas.AsNoTracking()
            .Where(r => idEmpleado == null || r.IdEmpleado == idEmpleado).ToListAsync();

        return filas.GroupBy(r => r.IdEmpleado).OrderBy(g => g.Key).Select(g =>
        {
            var funcFam = new RRHH_FuncFam { idFuncFamiliar = g.Key, IdEmpleado = g.Key };
            foreach (var r in g)
            {
                if (preguntas.FirstOrDefault(p => p.Value == r.IdPregunta).Key is { } codigo)
                    typeof(RRHH_FuncFam).GetProperty(codigo)!.SetValue(funcFam, r.Respuesta);
            }
            return funcFam;
        }).ToList();
    }

    private async Task GuardarAsync(RRHH_FuncFam registro)
    {
        if (registro.IdEmpleado is not { } idEmpleado)
            throw new InvalidOperationException("La funcionalidad familiar debe indicar el empleado.");

        var preguntas = await PreguntasAsync();
        var actuales = await Respuestas.Where(r => r.IdEmpleado == idEmpleado).ToListAsync();
        foreach (var codigo in Codigos)
        {
            if (!preguntas.TryGetValue(codigo, out var idPregunta)) continue;
            var valor = (typeof(RRHH_FuncFam).GetProperty(codigo)!.GetValue(registro) as string)?.Trim();
            var fila = actuales.FirstOrDefault(r => r.IdPregunta == idPregunta);
            if (string.IsNullOrEmpty(valor))
            {
                if (fila is not null) Respuestas.Remove(fila);
            }
            else if (fila is null)
                Respuestas.Add(new FuncFamRespuestaFila { IdEmpleado = idEmpleado, IdPregunta = idPregunta, Respuesta = valor });
            else
                fila.Respuesta = valor;
        }
        await _contexto.SaveChangesAsync();
    }

    private async Task<Dictionary<string, byte>> PreguntasAsync()
        => await _contexto.Set<FuncFamPreguntaFila>().AsNoTracking().ToDictionaryAsync(p => p.Codigo, p => p.IdPregunta);
}
