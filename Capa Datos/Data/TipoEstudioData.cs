using Capa_Entidades;

namespace Capa_Datos;

public class TipoEstudioData : RepositorioBase<TipoEstudio>
{
    public TipoEstudioData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<TipoEstudio?> Obtener(int IdTipoEstudiosParam) => ObtenerPorClave(IdTipoEstudiosParam);
}
