using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class LocalLogica : LogicaBase<Local>
{
    private readonly LocalData _data;

    public LocalLogica(LocalData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Local?> Obtener(int idLocalParam) => await _data.Obtener(idLocalParam);

    public Task CrearConNuevoId(Local registro) =>
        CrearNumerado(registro, x => x.idLocal, id => registro.idLocal = id);

    public Task<bool> Registrar(Local registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
