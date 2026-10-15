using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoTrabajadorLogica : LogicaBase<TipoTrabajador>
{
    private readonly TipoTrabajadorData _data;

    public TipoTrabajadorLogica(TipoTrabajadorData data) : base(data) => _data = data;

    public async Task<TipoTrabajador?> Obtener(int IdTipoTrabajadorParam) => await _data.Obtener(IdTipoTrabajadorParam);
}
