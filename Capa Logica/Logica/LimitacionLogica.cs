using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class LimitacionLogica : LogicaBase<Limitacion>
{
    private readonly LimitacionData _data;

    public LimitacionLogica(LimitacionData data) : base(data) => _data = data;

    public async Task<Limitacion?> Obtener(int IdLimitacionParam) => await _data.Obtener(IdLimitacionParam);
}
