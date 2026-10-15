using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class DiscapacidadLogica : LogicaBase<Discapacidad>
{
    private readonly IRepositorio<Discapacidad> _data;

    public DiscapacidadLogica(IRepositorio<Discapacidad> data) : base(data) => _data = data;

    public async Task<Discapacidad?> Obtener(int idDiscapacidad) => await _data.Obtener(idDiscapacidad);
}
