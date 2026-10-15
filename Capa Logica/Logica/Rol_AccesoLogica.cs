using Capa_Entidades;
using Capa_Datos;

namespace Capa_Logica;

public class Rol_AccesoLogica : LogicaBase<Rol_Acceso>
{
    private readonly Rol_AccesoData _data;

    public Rol_AccesoLogica(Rol_AccesoData data) : base(data) => _data = data;

    public async Task<Rol_Acceso?> Obtener(int IdRolParam, string IdSistemaOpcionParam) => await _data.Obtener(IdRolParam, IdSistemaOpcionParam);
}
