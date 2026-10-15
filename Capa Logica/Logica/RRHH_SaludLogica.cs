using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_SaludLogica : LogicaBase<RRHH_Salud>
{
    private readonly IRepositorio<RRHH_Salud> _data;

    public RRHH_SaludLogica(IRepositorio<RRHH_Salud> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<RRHH_Salud?> Obtener(int idSalud) => await _data.Obtener(idSalud);
}
