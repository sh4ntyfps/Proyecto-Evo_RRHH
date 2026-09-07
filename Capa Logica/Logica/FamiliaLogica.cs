using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class FamiliaLogica
{
    private readonly FamiliaData _data;

    public FamiliaLogica(FamiliaData data) => _data = data;

    public async Task<List<Familia>> Listar() => await _data.Listar();

    public async Task<Familia?> Obtener(string IdFamiliaParam, string IdClaseParam, string IdGrupoParam) => await _data.Obtener(IdFamiliaParam, IdClaseParam, IdGrupoParam);

    public async Task Crear(Familia registro) => await _data.Crear(registro);

    public async Task Actualizar(Familia registro) => await _data.Actualizar(registro);

    public async Task Eliminar(Familia registro) => await _data.Eliminar(registro);
}
