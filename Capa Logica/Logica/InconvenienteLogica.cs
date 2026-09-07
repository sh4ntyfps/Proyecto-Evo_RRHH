using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class InconvenienteLogica
{
    private readonly InconvenienteData _data;

    public InconvenienteLogica(InconvenienteData data) => _data = data;

    public async Task<List<Inconveniente>> Listar() => await _data.Listar();

    public async Task<Inconveniente?> Obtener(int IdObjetivoParam, int Id_ActividadParam, int IdInconvenienteParam) => await _data.Obtener(IdObjetivoParam, Id_ActividadParam, IdInconvenienteParam);

    public async Task Crear(Inconveniente registro) => await _data.Crear(registro);

    public async Task Actualizar(Inconveniente registro) => await _data.Actualizar(registro);

    public async Task Eliminar(Inconveniente registro) => await _data.Eliminar(registro);
}
