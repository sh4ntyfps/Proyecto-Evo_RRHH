using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RegimenPensionLogica : LogicaBase<RegimenPension>
{
    private readonly IRepositorio<RegimenPension> _data;

    public RegimenPensionLogica(IRepositorio<RegimenPension> data) : base(data) => _data = data;

    public async Task<RegimenPension?> Obtener(int idRegimenPen) => await _data.Obtener(idRegimenPen);
}
