using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_AspSocioLogica : LogicaBase<RRHH_AspSocio>
{
    private readonly RRHH_AspSocioData _data;

    public RRHH_AspSocioLogica(RRHH_AspSocioData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<RRHH_AspSocio?> Obtener(int idAspSocioParam) => await _data.Obtener(idAspSocioParam);

    public Task CrearConNuevoId(RRHH_AspSocio registro) =>
        CrearNumerado(registro, x => x.idAspSocio, id => registro.idAspSocio = id);

    public Task<bool> Registrar(RRHH_AspSocio registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
