using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class SistemaOpcionLogica : LogicaBase<SistemaOpcion>
{
    private readonly SistemaOpcionData _data;

    public SistemaOpcionLogica(SistemaOpcionData data) : base(data) => _data = data;

    public async Task<SistemaOpcion?> Obtener(string IdSistemaOpcionParam) => await _data.Obtener(IdSistemaOpcionParam);
}
