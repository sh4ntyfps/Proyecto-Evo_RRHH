using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EstudiosRealizadoLogica : LogicaBase<EstudiosRealizado>
{
    private readonly EstudiosRealizadoData _data;

    public EstudiosRealizadoLogica(EstudiosRealizadoData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<EstudiosRealizado?> Obtener(int IdTipoEstudiosParam, int IdEmpleadoParam, int CorrelativoParam) => await _data.Obtener(IdTipoEstudiosParam, IdEmpleadoParam, CorrelativoParam);

    // Correlativo por empleado y tipo de estudio.
    public Task<bool> Registrar(EstudiosRealizado registro) => EnTransaccion(() =>
        CrearNumerado(registro, x => x.Correlativo, id => registro.Correlativo = id,
            x => x.IdTipoEstudios == registro.IdTipoEstudios && x.IdEmpleado == registro.IdEmpleado));
}
