using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EstadoCivilLogica : LogicaBase<EstadoCivil>
{
    private readonly EstadoCivilData _data;

    public EstadoCivilLogica(EstadoCivilData data) : base(data) => _data = data;

    public async Task<EstadoCivil?> Obtener(int IdEstadoCivilParam) => await _data.Obtener(IdEstadoCivilParam);
}
