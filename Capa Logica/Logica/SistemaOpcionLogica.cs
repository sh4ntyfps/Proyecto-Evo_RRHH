using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class SistemaOpcionLogica : LogicaBase<SistemaOpcion>
{
    private readonly IRepositorio<SistemaOpcion> _data;

    public SistemaOpcionLogica(IRepositorio<SistemaOpcion> data) : base(data) => _data = data;

    public async Task<SistemaOpcion?> Obtener(string idSistemaOpcion) => await _data.Obtener(idSistemaOpcion);
}
