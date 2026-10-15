using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class MaterialVivLogica : LogicaBase<MaterialViv>
{
    private readonly IRepositorio<MaterialViv> _data;

    public MaterialVivLogica(IRepositorio<MaterialViv> data) : base(data) => _data = data;

    public async Task<MaterialViv?> Obtener(int idMaterial) => await _data.Obtener(idMaterial);
}
