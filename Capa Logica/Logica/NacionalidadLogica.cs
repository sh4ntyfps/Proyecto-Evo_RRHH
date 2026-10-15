using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class NacionalidadLogica : LogicaBase<Nacionalidad>
{
    private readonly IRepositorio<Nacionalidad> _data;

    public NacionalidadLogica(IRepositorio<Nacionalidad> data) : base(data) => _data = data;

    public async Task<Nacionalidad?> Obtener(int idNacionalidad) => await _data.Obtener(idNacionalidad);
}
