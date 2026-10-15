using Capa_Entidades;

namespace Capa_Datos;

public class CapacitacionData : RepositorioBase<Capacitacion>
{
    public CapacitacionData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<Capacitacion?> Obtener(int IdCapacitacionParam) => ObtenerPorClave(IdCapacitacionParam);
}
