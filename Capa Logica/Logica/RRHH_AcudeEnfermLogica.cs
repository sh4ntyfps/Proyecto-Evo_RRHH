using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_AcudeEnfermLogica : LogicaBase<RRHH_AcudeEnferm>
{
    private readonly IRepositorio<RRHH_AcudeEnferm> _data;

    public RRHH_AcudeEnfermLogica(IRepositorio<RRHH_AcudeEnferm> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<RRHH_AcudeEnferm?> Obtener(int idAcudeEnferm) => await _data.Obtener(idAcudeEnferm);
}
