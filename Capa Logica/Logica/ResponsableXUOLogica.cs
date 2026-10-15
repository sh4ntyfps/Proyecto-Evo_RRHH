using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class ResponsableXUOLogica : LogicaBase<ResponsableXUO>
{
    private readonly ResponsableXUOData _data;

    public ResponsableXUOLogica(ResponsableXUOData data, SecuenciaService secuencia) : base(data, secuencia) => _data = data;

    public async Task<ResponsableXUO?> Obtener(int IdResponsableParam) => await _data.Obtener(IdResponsableParam);

    public Task CrearConNuevoId(ResponsableXUO registro) =>
        CrearNumerado(registro, x => x.IdResponsable, id => registro.IdResponsable = id);

    public Task<bool> Registrar(ResponsableXUO registro) => EnTransaccion(() => CrearConNuevoId(registro));
}
