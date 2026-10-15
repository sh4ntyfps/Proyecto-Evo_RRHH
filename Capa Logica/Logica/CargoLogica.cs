using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class CargoLogica : LogicaBase<Cargo>
{
    private readonly CargoData _data;

    public CargoLogica(CargoData data) : base(data) => _data = data;

    public async Task<Cargo?> Obtener(int IdCargoParam) => await _data.Obtener(IdCargoParam);
}
