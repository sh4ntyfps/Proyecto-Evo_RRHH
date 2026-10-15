using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class Motivo_PermLogica : LogicaBase<Motivo_Perm>
{
    private readonly IRepositorio<Motivo_Perm> _data;

    public Motivo_PermLogica(IRepositorio<Motivo_Perm> data) : base(data) => _data = data;

    public async Task<Motivo_Perm?> Obtener(int idMotivo) => await _data.Obtener(idMotivo);
}
