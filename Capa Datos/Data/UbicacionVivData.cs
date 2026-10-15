using Capa_Entidades;

namespace Capa_Datos;

public class UbicacionVivData : RepositorioBase<UbicacionViv>
{
    public UbicacionVivData(EvoRRLDbContext contexto) : base(contexto) { }

    public Task<UbicacionViv?> Obtener(int IdUbicacionParam) => ObtenerPorClave(IdUbicacionParam);
}
