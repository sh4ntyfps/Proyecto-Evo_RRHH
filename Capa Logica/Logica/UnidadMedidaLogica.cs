using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class UnidadMedidaLogica
{
    private readonly UnidadMedidaData _data;

    public UnidadMedidaLogica(UnidadMedidaData data) => _data = data;

    public async Task<List<UnidadMedida>> Listar() => await _data.Listar();

    public async Task<UnidadMedida?> Obtener(string IdUnidadMedidaParam) => await _data.Obtener(IdUnidadMedidaParam);

    public async Task Crear(UnidadMedida registro) => await _data.Crear(registro);

    public async Task Actualizar(UnidadMedida registro) => await _data.Actualizar(registro);

    public async Task Eliminar(UnidadMedida registro) => await _data.Eliminar(registro);
}
