using Capa_Entidades;

namespace Capa_Datos;

public class ResolucionData : RepositorioBase<Resolucion>
{
    public ResolucionData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Resolucion?> Obtener(int idEmpleadoParam, int idResolucionParam) => ObtenerPorClave(idEmpleadoParam, idResolucionParam);
}
