using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ExcretasVivLogica : LogicaBase<ExcretasViv>
{
    private readonly ExcretasVivData _data;

    public ExcretasVivLogica(ExcretasVivData data) : base(data) => _data = data;

    public async Task<ExcretasViv?> Obtener(int IdExcretasParam) => await _data.Obtener(IdExcretasParam);
}
