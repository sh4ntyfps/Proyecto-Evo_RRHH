using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class UbigeoLogica : LogicaBase<Ubigeo>
{
    private readonly IRepositorio<Ubigeo> _data;

    public UbigeoLogica(IRepositorio<Ubigeo> data) : base(data) => _data = data;

    public async Task<Ubigeo?> Obtener(string idUbigeo) => await _data.Obtener(idUbigeo);
}
