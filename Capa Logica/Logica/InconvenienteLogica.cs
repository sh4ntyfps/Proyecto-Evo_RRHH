using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class InconvenienteLogica : LogicaBase<Inconveniente>
{
    private readonly InconvenienteData _data;

    public InconvenienteLogica(InconvenienteData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Inconveniente?> Obtener(int IdObjetivoParam, int Id_ActividadParam, int IdInconvenienteParam) => await _data.Obtener(IdObjetivoParam, Id_ActividadParam, IdInconvenienteParam);

    // Numero correlativo por objetivo y actividad.
    public Task CrearConNuevoId(Inconveniente registro) =>
        CrearNumerado(registro, x => x.IdInconveniente, id => registro.IdInconveniente = id, x => x.IdObjetivo == registro.IdObjetivo && x.Id_Actividad == registro.Id_Actividad);

    public Task<bool> Registrar(Inconveniente registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
