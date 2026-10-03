using Capa_Datos;
using Capa_Entidades;

namespace Capa_Logica;

public class AsistenciaLogica : LogicaBase<Asistencia, AsistenciaData>
{
    public AsistenciaLogica(AsistenciaData datos) : base(datos) { }

    public async Task<int> ContarEntreFechas(DateTime desde, DateTime hasta) => await Datos.ContarEntreFechas(desde, hasta);
}
