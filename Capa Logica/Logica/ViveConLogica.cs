using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ViveConLogica : LogicaBase<ViveCon>
{
    private readonly ViveConData _data;

    public ViveConLogica(ViveConData data) : base(data) => _data = data;

    public async Task<ViveCon?> Obtener(int IdViveConParam) => await _data.Obtener(IdViveConParam);
}
