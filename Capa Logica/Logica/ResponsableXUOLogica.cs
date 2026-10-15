using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ResponsableXUOLogica : LogicaBase<ResponsableXUO>
{
    private readonly IRepositorio<ResponsableXUO> _data;

    public ResponsableXUOLogica(IRepositorio<ResponsableXUO> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<ResponsableXUO?> Obtener(int idResponsable) => await _data.Obtener(idResponsable);
}
