using Capa_Entidades;

namespace Capa_Datos;

public class RotacionData : RepositorioBase<Rotacion>
{
    public RotacionData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Rotacion?> Obtener(int idEmpleadoParam, int idRotacionParam) => ObtenerPorClave(idEmpleadoParam, idRotacionParam);
}
