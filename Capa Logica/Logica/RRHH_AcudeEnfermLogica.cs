using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_AcudeEnfermLogica : LogicaBase<RRHH_AcudeEnferm>
{
    private readonly RRHH_AcudeEnfermData _data;

    public RRHH_AcudeEnfermLogica(RRHH_AcudeEnfermData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<RRHH_AcudeEnferm?> Obtener(int idAcudeEnfermParam) => await _data.Obtener(idAcudeEnfermParam);

    public Task CrearConNuevoId(RRHH_AcudeEnferm registro) =>
        CrearNumerado(registro, x => x.idAcudeEnferm, id => registro.idAcudeEnferm = id);

    public Task<bool> Registrar(RRHH_AcudeEnferm registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
