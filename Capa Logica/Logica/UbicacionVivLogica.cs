using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class UbicacionVivLogica : LogicaBase<UbicacionViv>
{
    private readonly UbicacionVivData _data;

    public UbicacionVivLogica(UbicacionVivData data) : base(data) => _data = data;

    public async Task<UbicacionViv?> Obtener(int IdUbicacionParam) => await _data.Obtener(IdUbicacionParam);
}
