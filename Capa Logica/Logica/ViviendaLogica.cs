using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ViviendaLogica : LogicaBase<Vivienda>
{
    private readonly ViviendaData _data;

    public ViviendaLogica(ViviendaData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Vivienda?> Obtener(int IdViviendaParam) => await _data.Obtener(IdViviendaParam);

    public Task CrearConNuevoId(Vivienda registro) =>
        CrearNumerado(registro, x => x.IdVivienda, id => registro.IdVivienda = id);

    public Task<bool> Registrar(Vivienda registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
