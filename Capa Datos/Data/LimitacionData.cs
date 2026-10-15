using Capa_Entidades;

namespace Capa_Datos;

public class LimitacionData : RepositorioBase<Limitacion>
{
    public LimitacionData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Limitacion?> Obtener(int IdLimitacionParam) => ObtenerPorClave(IdLimitacionParam);
}
