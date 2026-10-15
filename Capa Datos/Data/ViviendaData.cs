using Capa_Entidades;

namespace Capa_Datos;

public class ViviendaData : RepositorioBase<Vivienda>
{
    public ViviendaData(EvoRRLDbContext contexto) : base(contexto) { }
}
