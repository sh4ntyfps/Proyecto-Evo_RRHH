using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class InstitucionLogica
{
    private readonly InstitucionData _data;

    public InstitucionLogica(InstitucionData data) => _data = data;

    public async Task<List<Institucion>> Listar() => await _data.Listar();

    public async Task<Institucion?> Obtener(int IdInstitucionParam) => await _data.Obtener(IdInstitucionParam);

    public async Task Crear(Institucion registro) => await _data.Crear(registro);

    public async Task Actualizar(Institucion registro) => await _data.Actualizar(registro);

    public async Task Eliminar(Institucion registro) => await _data.Eliminar(registro);
}
