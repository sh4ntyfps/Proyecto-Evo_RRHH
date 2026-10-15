using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class NacionalidadLogica : LogicaBase<Nacionalidad>
{
    private readonly NacionalidadData _data;

    public NacionalidadLogica(NacionalidadData data) : base(data) => _data = data;

    public async Task<Nacionalidad?> Obtener(int IdNacionalidadParam) => await _data.Obtener(IdNacionalidadParam);
}
