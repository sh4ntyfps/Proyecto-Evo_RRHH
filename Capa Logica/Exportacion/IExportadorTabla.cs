namespace Capa_Logica.Exportacion;

public sealed record ArchivoExportado(byte[] Contenido, string TipoContenido, string Extension);

// Contrato de exportacion (OCP): un formato nuevo (Excel, PDF...) es una clase nueva que lo implementa
// y una linea de registro; los controllers y las clases existentes no se modifican.
public interface IExportadorTabla
{
    string Formato { get; }

    // La primera fila son los encabezados.
    ArchivoExportado Exportar(IReadOnlyList<string[]> filas);
}
