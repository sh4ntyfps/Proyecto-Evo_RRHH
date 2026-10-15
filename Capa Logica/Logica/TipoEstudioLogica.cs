using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class TipoEstudioLogica : LogicaBase<TipoEstudio>
{
    private readonly TipoEstudioData _data;

    public TipoEstudioLogica(TipoEstudioData data) : base(data) => _data = data;

    public async Task<TipoEstudio?> Obtener(int IdTipoEstudiosParam) => await _data.Obtener(IdTipoEstudiosParam);
}
