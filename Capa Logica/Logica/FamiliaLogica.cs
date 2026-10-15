using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class FamiliaLogica : LogicaBase<Familia>
{
    private readonly FamiliaData _data;

    public FamiliaLogica(FamiliaData data) : base(data) => _data = data;

    public async Task<Familia?> Obtener(string IdFamiliaParam, string IdClaseParam, string IdGrupoParam) => await _data.Obtener(IdFamiliaParam, IdClaseParam, IdGrupoParam);
}
