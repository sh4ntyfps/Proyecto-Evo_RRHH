using Capa_Entidades;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Capa_Datos;

// En RRHH_Nuevo Marcacion.Estado juntaba dos datos ('1N'); en RRHH_Normalizado se guardan
// separados: NroMarcacion = 1 y TipoMarcacion = 'N'. Lugar es la columna CodDispositivo.
public class MarcacionData : RepositorioBase<Marcacion>
{
    private const byte NroPorDefecto = 1;
    private const string TipoPorDefecto = "N";

    public MarcacionData(EvoRRLDbContext contexto) : base(contexto) { }

    protected override Task Completar(List<Marcacion> marcaciones)
    {
        foreach (var m in marcaciones)
        {
            var entrada = _contexto.Entry(m);
            var nro = entrada.Property<byte>("NroMarcacion").CurrentValue;
            var tipo = entrada.Property<string>("TipoMarcacion").CurrentValue;
            m.Estado = $"{nro}{tipo?.Trim()}";
        }
        return Task.CompletedTask;
    }

    protected override void AntesDeGuardar(EntityEntry<Marcacion> entrada, bool esNuevo)
    {
        var estado = entrada.Entity.Estado?.Trim() ?? "";
        var nro = estado.Length > 0 && char.IsDigit(estado[0]) ? (byte)(estado[0] - '0') : NroPorDefecto;
        var tipo = estado.Length > 1 ? estado.Substring(1, 1).ToUpperInvariant() : TipoPorDefecto;
        Asignar(entrada, "NroMarcacion", nro);
        Asignar(entrada, "TipoMarcacion", tipo);
        if (string.IsNullOrWhiteSpace(entrada.Entity.Lugar))
            entrada.Entity.Lugar = null;
    }
}
