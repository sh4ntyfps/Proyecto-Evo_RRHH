using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class AFPLogica : LogicaBase<AFP>
{
    private readonly AFPData _data;

    public AFPLogica(AFPData data) : base(data) => _data = data;

    public async Task<AFP?> Obtener(int IdAFPParam) => await _data.Obtener(IdAFPParam);
}
