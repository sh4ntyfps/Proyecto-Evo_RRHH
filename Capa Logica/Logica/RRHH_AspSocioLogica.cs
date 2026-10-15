using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_AspSocioLogica : LogicaBase<RRHH_AspSocio>
{
    private readonly IRepositorio<RRHH_AspSocio> _data;

    public RRHH_AspSocioLogica(IRepositorio<RRHH_AspSocio> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<RRHH_AspSocio?> Obtener(int idAspSocio) => await _data.Obtener(idAspSocio);
}
