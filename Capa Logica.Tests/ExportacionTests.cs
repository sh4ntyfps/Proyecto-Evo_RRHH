using System.Text;
using Capa_Logica.Exportacion;

namespace Capa_Logica.Tests;

public class ExportadorCsvTests
{
    [Theory]
    [InlineData("simple", "simple")]
    [InlineData("a;b", "\"a;b\"")]
    [InlineData("dice \"hola\"", "\"dice \"\"hola\"\"\"")]
    [InlineData("linea1\nlinea2", "\"linea1\nlinea2\"")]
    public void EscaparCampo_conserva_la_regla_de_la_exportacion_original(string valor, string esperado)
        => Assert.Equal(esperado, ExportadorCsv.EscaparCampo(valor));

    [Fact]
    public void Genera_el_mismo_archivo_que_antes_punto_y_coma_CRLF_y_sin_BOM()
    {
        var archivo = new ExportadorCsv().Exportar(new[] { new[] { "Id", "Nombre" }, new[] { "1", "Peña; Ana" } });

        // Caracterizacion: el codigo original tampoco escribia el BOM.
        var esperado = Encoding.UTF8.GetBytes("Id;Nombre\r\n1;\"Peña; Ana\"");
        Assert.Equal(esperado, archivo.Contenido);
        Assert.Equal("text/csv; charset=utf-8", archivo.TipoContenido);
        Assert.Equal("csv", archivo.Extension);
    }
}

public class ExportadorTablasTests
{
    // Formato ficticio: demuestra que se puede agregar uno nuevo sin modificar ninguna clase existente (OCP).
    private sealed class ExportadorTsv : IExportadorTabla
    {
        public string Formato => "tsv";
        public ArchivoExportado Exportar(IReadOnlyList<string[]> filas)
            => new(Encoding.UTF8.GetBytes(string.Join("\n", filas.Select(f => string.Join("\t", f)))), "text/tab-separated-values", "tsv");
    }

    [Fact]
    public void Elige_el_exportador_por_formato_sin_distinguir_mayusculas()
    {
        var exportador = new ExportadorTablas(new IExportadorTabla[] { new ExportadorCsv(), new ExportadorTsv() });

        Assert.Equal("csv", exportador.Exportar("CSV", new[] { new[] { "a" } }).Extension);
        Assert.Equal("a\tb", Encoding.UTF8.GetString(exportador.Exportar("tsv", new[] { new[] { "a", "b" } }).Contenido));
    }

    [Fact]
    public void Un_formato_no_registrado_no_se_soporta()
        => Assert.False(new ExportadorTablas(new[] { new ExportadorCsv() }).Soporta("pdf"));
}
