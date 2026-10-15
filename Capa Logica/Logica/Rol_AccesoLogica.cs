using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class Rol_AccesoLogica : LogicaBase<Rol_Acceso>
{
    private readonly IRepositorio<Rol_Acceso> _data;

    public Rol_AccesoLogica(IRepositorio<Rol_Acceso> data) : base(data) => _data = data;

    public async Task<Rol_Acceso?> Obtener(int idRol, string idSistemaOpcion) => await _data.Obtener(idRol, idSistemaOpcion);
}
