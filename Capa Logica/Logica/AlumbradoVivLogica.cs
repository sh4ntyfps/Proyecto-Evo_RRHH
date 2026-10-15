using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class AlumbradoVivLogica : LogicaBase<AlumbradoViv>
{
    private readonly IRepositorio<AlumbradoViv> _data;

    public AlumbradoVivLogica(IRepositorio<AlumbradoViv> data) : base(data) => _data = data;

    public async Task<AlumbradoViv?> Obtener(int idAlumbrado) => await _data.Obtener(idAlumbrado);
}
