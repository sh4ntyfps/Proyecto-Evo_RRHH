using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class AlumbradoVivLogica : LogicaBase<AlumbradoViv>
{
    private readonly AlumbradoVivData _data;

    public AlumbradoVivLogica(AlumbradoVivData data) : base(data) => _data = data;

    public async Task<AlumbradoViv?> Obtener(int IdAlumbradoParam) => await _data.Obtener(IdAlumbradoParam);
}
