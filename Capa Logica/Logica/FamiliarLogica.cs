using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class FamiliarLogica : LogicaBase<Familiar>
{
    private readonly IRepositorio<Familiar> _data;

    public FamiliarLogica(IRepositorio<Familiar> data) : base(data) => _data = data;

    public async Task<Familiar?> Obtener(int idEmpleado, int idPersona) => await _data.Obtener(idEmpleado, idPersona);
}
