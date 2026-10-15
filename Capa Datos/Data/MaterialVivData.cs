using Capa_Entidades;

namespace Capa_Datos;

public class MaterialVivData : RepositorioBase<MaterialViv>
{
    public MaterialVivData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<MaterialViv?> Obtener(int IdMaterialParam) => ObtenerPorClave(IdMaterialParam);
}
