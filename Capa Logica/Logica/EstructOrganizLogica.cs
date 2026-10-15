using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class EstructOrganizLogica : LogicaBase<EstructOrganiz>
{
    private readonly EstructOrganizData _data;

    public EstructOrganizLogica(EstructOrganizData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<EstructOrganiz?> Obtener(int YearParam, int idAreaOrganizParam) => await _data.Obtener(YearParam, idAreaOrganizParam);

    // Codigo de area correlativo dentro de cada anio.
    public Task CrearConNuevoId(EstructOrganiz registro) =>
        CrearNumerado(registro, x => x.idAreaOrganiz, id => registro.idAreaOrganiz = id, x => x.Year == registro.Year);

    public Task<bool> Registrar(EstructOrganiz registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
