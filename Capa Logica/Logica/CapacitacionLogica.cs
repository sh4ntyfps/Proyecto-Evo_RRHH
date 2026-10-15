using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class CapacitacionLogica : LogicaBase<Capacitacion>
{
    private readonly CapacitacionData _data;

    public CapacitacionLogica(CapacitacionData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Capacitacion?> Obtener(int IdCapacitacionParam) => await _data.Obtener(IdCapacitacionParam);

    public Task CrearConNuevoId(Capacitacion registro) =>
        CrearNumerado(registro, x => x.IdCapacitacion, id => registro.IdCapacitacion = id);

    public Task<bool> Registrar(Capacitacion registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
