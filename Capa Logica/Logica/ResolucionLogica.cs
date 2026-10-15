using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ResolucionLogica : LogicaBase<Resolucion>
{
    private readonly ResolucionData _data;

    public ResolucionLogica(ResolucionData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Resolucion?> Obtener(int idEmpleadoParam, int idResolucionParam) => await _data.Obtener(idEmpleadoParam, idResolucionParam);

    // Numero de resolucion correlativo por empleado.
    public Task CrearConNuevoId(Resolucion registro) =>
        CrearNumerado(registro, x => x.idResolucion, id => registro.idResolucion = id, x => x.idEmpleado == registro.idEmpleado);

    public Task<bool> Registrar(Resolucion registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
