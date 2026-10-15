using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class UbicacionVivLogica : LogicaBase<UbicacionViv>
{
    private readonly IRepositorio<UbicacionViv> _data;

    public UbicacionVivLogica(IRepositorio<UbicacionViv> data) : base(data) => _data = data;

    public async Task<UbicacionViv?> Obtener(int idUbicacion) => await _data.Obtener(idUbicacion);
}
