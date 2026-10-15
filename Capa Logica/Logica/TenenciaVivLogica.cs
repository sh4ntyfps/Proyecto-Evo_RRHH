using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class TenenciaVivLogica : LogicaBase<TenenciaViv>
{
    private readonly IRepositorio<TenenciaViv> _data;

    public TenenciaVivLogica(IRepositorio<TenenciaViv> data) : base(data) => _data = data;

    public async Task<TenenciaViv?> Obtener(int idTenencia) => await _data.Obtener(idTenencia);
}
