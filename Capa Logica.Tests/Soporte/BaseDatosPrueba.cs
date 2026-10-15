using Capa_Datos;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Capa_Logica.Tests.Soporte;

// Base SQLite en memoria: permite probar las capas sin SQL Server.
// Las FKs se desactivan para sembrar solo las tablas que cada prueba necesita.
public sealed class BaseDatosPrueba : IDisposable
{
    private readonly SqliteConnection _conexion;

    public EvoRRLDbContext Contexto { get; }

    public BaseDatosPrueba()
    {
        _conexion = new SqliteConnection("DataSource=:memory:;Foreign Keys=False");
        _conexion.Open();
        var opciones = new DbContextOptionsBuilder<EvoRRLDbContext>().UseSqlite(_conexion).Options;
        Contexto = new EvoRRLDbContext(opciones);
        Contexto.Database.EnsureCreated();
    }

    public async Task SembrarAsync(params object[] entidades)
    {
        Contexto.AddRange(entidades);
        await Contexto.SaveChangesAsync();
        Contexto.ChangeTracker.Clear();
    }

    public void Dispose()
    {
        Contexto.Dispose();
        _conexion.Dispose();
    }
}
