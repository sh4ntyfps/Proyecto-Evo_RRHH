using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class EstructOrganizLogica : LogicaBase<EstructOrganiz>
{
    private readonly IRepositorio<EstructOrganiz> _data;

    public EstructOrganizLogica(IRepositorio<EstructOrganiz> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<EstructOrganiz?> Obtener(int year, int idAreaOrganiz) => await _data.Obtener(year, idAreaOrganiz);
}
