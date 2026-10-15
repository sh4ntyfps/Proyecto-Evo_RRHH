using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class FamiliaLogica : LogicaBase<Familia>
{
    private readonly IRepositorio<Familia> _data;

    public FamiliaLogica(IRepositorio<Familia> data) : base(data) => _data = data;

    public async Task<Familia?> Obtener(string idFamilia, string idClase, string idGrupo) => await _data.Obtener(idFamilia, idClase, idGrupo);
}
