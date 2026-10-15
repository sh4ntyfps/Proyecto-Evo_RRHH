using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class ExcretasVivLogica : LogicaBase<ExcretasViv>
{
    private readonly IRepositorio<ExcretasViv> _data;

    public ExcretasVivLogica(IRepositorio<ExcretasViv> data) : base(data) => _data = data;

    public async Task<ExcretasViv?> Obtener(int idExcretas) => await _data.Obtener(idExcretas);
}
