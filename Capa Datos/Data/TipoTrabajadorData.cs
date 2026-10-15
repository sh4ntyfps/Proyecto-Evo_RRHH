using Capa_Entidades;

namespace Capa_Datos;

public class TipoTrabajadorData : RepositorioBase<TipoTrabajador>
{
    public TipoTrabajadorData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<TipoTrabajador?> Obtener(int IdTipoTrabajadorParam) => ObtenerPorClave(IdTipoTrabajadorParam);
}
