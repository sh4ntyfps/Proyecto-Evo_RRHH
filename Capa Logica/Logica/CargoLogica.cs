using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class CargoLogica : LogicaBase<Cargo>
{
    private readonly IRepositorio<Cargo> _data;

    public CargoLogica(IRepositorio<Cargo> data) : base(data) => _data = data;

    public async Task<Cargo?> Obtener(int idCargo) => await _data.Obtener(idCargo);
}
