using Capa_Entidades;

namespace Capa_Datos;

public class Rol_AccesoData : RepositorioBase<Rol_Acceso>
{
    public Rol_AccesoData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Rol_Acceso?> Obtener(int IdRolParam, string IdSistemaOpcionParam) => ObtenerPorClave(IdRolParam, IdSistemaOpcionParam);
}
