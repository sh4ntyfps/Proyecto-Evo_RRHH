using Capa_Entidades;

namespace Capa_Datos;

public class HorarioTemporalData : RepositorioBase<HorarioTemporal>
{
    public HorarioTemporalData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<HorarioTemporal?> Obtener(int idEmpleadoParam, int NParam) => ObtenerPorClave(idEmpleadoParam, NParam);
}
