using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_AseguradoLogica : LogicaBase<RRHH_Asegurado>
{
    private readonly IRepositorio<RRHH_Asegurado> _data;

    public RRHH_AseguradoLogica(IRepositorio<RRHH_Asegurado> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<RRHH_Asegurado?> Obtener(int idEmpleado, int nroBeneficiario) => await _data.Obtener(idEmpleado, nroBeneficiario);
}
