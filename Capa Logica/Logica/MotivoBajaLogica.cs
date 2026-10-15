using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class MotivoBajaLogica : LogicaBase<MotivoBaja>
{
    private readonly MotivoBajaData _data;

    public MotivoBajaLogica(MotivoBajaData data) : base(data) => _data = data;

    public async Task<MotivoBaja?> Obtener(int idMotivoBajaParam) => await _data.Obtener(idMotivoBajaParam);
}
