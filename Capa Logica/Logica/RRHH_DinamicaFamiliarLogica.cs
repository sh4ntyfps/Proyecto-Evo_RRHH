using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class RRHH_DinamicaFamiliarLogica : LogicaBase<RRHH_DinamicaFamiliar>
{
    private readonly IRepositorio<RRHH_DinamicaFamiliar> _data;

    public RRHH_DinamicaFamiliarLogica(IRepositorio<RRHH_DinamicaFamiliar> data, IUnidadDeTrabajo unidad) : base(data, unidad) => _data = data;

    public async Task<RRHH_DinamicaFamiliar?> Obtener(int idDinamica) => await _data.Obtener(idDinamica);
}
