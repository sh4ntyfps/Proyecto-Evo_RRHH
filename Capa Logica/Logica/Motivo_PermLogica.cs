using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class Motivo_PermLogica : LogicaBase<Motivo_Perm>
{
    private readonly Motivo_PermData _data;

    public Motivo_PermLogica(Motivo_PermData data) : base(data) => _data = data;

    public async Task<Motivo_Perm?> Obtener(int idMotivoParam) => await _data.Obtener(idMotivoParam);
}
