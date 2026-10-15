using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class DiscapacidadLogica : LogicaBase<Discapacidad>
{
    private readonly DiscapacidadData _data;

    public DiscapacidadLogica(DiscapacidadData data) : base(data) => _data = data;

    public async Task<Discapacidad?> Obtener(int IdDiscapacidadParam) => await _data.Obtener(IdDiscapacidadParam);
}
