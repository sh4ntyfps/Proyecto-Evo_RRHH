using Capa_Entidades;

namespace Capa_Datos;

public class MotivoBajaData : RepositorioBase<MotivoBaja>
{
    public MotivoBajaData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<MotivoBaja?> Obtener(int idMotivoBajaParam) => ObtenerPorClave(idMotivoBajaParam);
}
