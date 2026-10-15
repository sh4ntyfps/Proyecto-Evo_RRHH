using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class InstitucionLogica : LogicaBase<Institucion>
{
    private readonly InstitucionData _data;

    public InstitucionLogica(InstitucionData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<Institucion?> Obtener(int IdInstitucionParam) => await _data.Obtener(IdInstitucionParam);

    public Task CrearConNuevoId(Institucion registro) =>
        CrearNumerado(registro, x => x.IdInstitucion, id => registro.IdInstitucion = id);

    public Task<bool> Registrar(Institucion registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
