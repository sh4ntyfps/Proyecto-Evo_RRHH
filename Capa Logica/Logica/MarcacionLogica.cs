using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class MarcacionLogica : LogicaBase<Marcacion>
{
    private readonly MarcacionData _data;

    public MarcacionLogica(MarcacionData data) : base(data) => _data = data;

    public async Task<Marcacion?> Obtener(DateTime FechaParam, int IdEmpleadoParam) => await _data.Obtener(FechaParam, IdEmpleadoParam);
}
