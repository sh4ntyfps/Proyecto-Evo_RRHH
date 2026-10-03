using Capa_Entidades;

namespace Capa_Datos;

public class HorarioData : RepositorioBase<Horario>
{
    public HorarioData(EvoRRLDbContext contexto) : base(contexto) { }
}
