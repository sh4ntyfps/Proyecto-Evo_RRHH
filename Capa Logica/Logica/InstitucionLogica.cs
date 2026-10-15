using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class InstitucionLogica : LogicaBase<Institucion>
{
    private readonly IRepositorio<Institucion> _data;

    public InstitucionLogica(IRepositorio<Institucion> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Institucion?> Obtener(int idInstitucion) => await _data.Obtener(idInstitucion);
}
