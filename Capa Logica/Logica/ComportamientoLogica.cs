using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ComportamientoLogica : LogicaBase<Comportamiento>
{
    private readonly ComportamientoData _data;

    public ComportamientoLogica(ComportamientoData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Comportamiento?> Obtener(int idComportamientoParam) => await _data.Obtener(idComportamientoParam);

    public Task CrearConNuevoId(Comportamiento registro) =>
        CrearNumerado(registro, x => x.idComportamiento, id => registro.idComportamiento = id);

    public Task<bool> Registrar(Comportamiento registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
