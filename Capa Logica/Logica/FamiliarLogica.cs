using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class FamiliarLogica : LogicaBase<Familiar>
{
    private readonly FamiliarData _data;

    public FamiliarLogica(FamiliarData data) : base(data) => _data = data;

    public async Task<Familiar?> Obtener(int IdEmpleadoParam, int IdPersonaParam) => await _data.Obtener(IdEmpleadoParam, IdPersonaParam);
}
