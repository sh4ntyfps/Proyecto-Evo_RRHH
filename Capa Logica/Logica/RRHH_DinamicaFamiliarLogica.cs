using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class RRHH_DinamicaFamiliarLogica : LogicaBase<RRHH_DinamicaFamiliar>
{
    private readonly RRHH_DinamicaFamiliarData _data;

    public RRHH_DinamicaFamiliarLogica(RRHH_DinamicaFamiliarData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<RRHH_DinamicaFamiliar?> Obtener(int idDinamicaParam) => await _data.Obtener(idDinamicaParam);

    public Task CrearConNuevoId(RRHH_DinamicaFamiliar registro) =>
        CrearNumerado(registro, x => x.idDinamica, id => registro.idDinamica = id);

    public Task<bool> Registrar(RRHH_DinamicaFamiliar registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
