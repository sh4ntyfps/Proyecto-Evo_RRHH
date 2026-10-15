using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class MaterialVivLogica : LogicaBase<MaterialViv>
{
    private readonly MaterialVivData _data;

    public MaterialVivLogica(MaterialVivData data) : base(data) => _data = data;

    public async Task<MaterialViv?> Obtener(int IdMaterialParam) => await _data.Obtener(IdMaterialParam);
}
