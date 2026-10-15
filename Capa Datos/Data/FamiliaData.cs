using Capa_Entidades;

namespace Capa_Datos;

public class FamiliaData : RepositorioBase<Familia>
{
    public FamiliaData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Familia?> Obtener(string IdFamiliaParam, string IdClaseParam, string IdGrupoParam) => ObtenerPorClave(IdFamiliaParam, IdClaseParam, IdGrupoParam);
}
