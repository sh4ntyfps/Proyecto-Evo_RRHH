using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EstadoCivilLogica : LogicaBase<EstadoCivil>
{
    private readonly IRepositorio<EstadoCivil> _data;

    public EstadoCivilLogica(IRepositorio<EstadoCivil> data) : base(data) => _data = data;

    public async Task<EstadoCivil?> Obtener(int idEstadoCivil) => await _data.Obtener(idEstadoCivil);
}
