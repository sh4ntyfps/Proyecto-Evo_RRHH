using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class MarcacionLogica : LogicaBase<Marcacion, MarcacionData>
{
    public MarcacionLogica(MarcacionData datos) : base(datos) { }
}
