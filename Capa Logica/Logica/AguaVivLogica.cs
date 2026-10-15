using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class AguaVivLogica : LogicaBase<AguaViv>
{
    private readonly IRepositorio<AguaViv> _data;

    public AguaVivLogica(IRepositorio<AguaViv> data) : base(data) => _data = data;

    public async Task<AguaViv?> Obtener(int idAgua) => await _data.Obtener(idAgua);
}
