using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class LocalLogica : LogicaBase<Local>
{
    private readonly IRepositorio<Local> _data;

    public LocalLogica(IRepositorio<Local> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<Local?> Obtener(int idLocal) => await _data.Obtener(idLocal);
}
