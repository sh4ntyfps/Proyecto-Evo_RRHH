using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TenenciaVivLogica : LogicaBase<TenenciaViv>
{
    private readonly TenenciaVivData _data;

    public TenenciaVivLogica(TenenciaVivData data) : base(data) => _data = data;

    public async Task<TenenciaViv?> Obtener(int IdTenenciaParam) => await _data.Obtener(IdTenenciaParam);
}
