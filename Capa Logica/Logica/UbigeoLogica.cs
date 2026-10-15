using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class UbigeoLogica : LogicaBase<Ubigeo>
{
    private readonly UbigeoData _data;

    public UbigeoLogica(UbigeoData data) : base(data) => _data = data;

    public async Task<Ubigeo?> Obtener(string IdUbigeoParam) => await _data.Obtener(IdUbigeoParam);
}
