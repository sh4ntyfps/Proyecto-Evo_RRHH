using Capa_Entidades;
using Capa_Entidades.Contratos;

namespace Capa_Logica;

public class BitacoraLogica
{
    private readonly IBitacoraRepositorio _data;

    public BitacoraLogica(IBitacoraRepositorio data) => _data = data;

    public async Task Registrar(string usuario, string accion, string entidad, string? detalle = null) =>
        await _data.Registrar(new Bitacora
        {
            Fecha = DateTime.Now,
            Usuario = usuario,
            Accion = accion,
            Entidad = entidad,
            Detalle = detalle
        });

    public async Task<List<Bitacora>> Ultimas(int cantidad) => await _data.Ultimas(cantidad);
}

