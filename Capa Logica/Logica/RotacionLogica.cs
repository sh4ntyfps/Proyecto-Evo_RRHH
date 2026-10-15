using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RotacionLogica : LogicaBase<Rotacion>
{
    private readonly RotacionData _data;

    public RotacionLogica(RotacionData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Rotacion?> Obtener(int idEmpleadoParam, int idRotacionParam) => await _data.Obtener(idEmpleadoParam, idRotacionParam);

    // Numero de rotacion correlativo por empleado.
    public Task CrearConNuevoId(Rotacion registro) =>
        CrearNumerado(registro, x => x.idRotacion, id => registro.idRotacion = id, x => x.idEmpleado == registro.idEmpleado);

    public Task<bool> Registrar(Rotacion registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
