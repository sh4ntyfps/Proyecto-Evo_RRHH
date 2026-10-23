using Capa_Entidades;

namespace Capa_Datos;

// En RRHH_Normalizado el ubigeo se normalizo en Departamento / Provincia / Distrito;
// se lee de la vista vw_Ubigeo y no se edita desde la aplicacion.
public class UbigeoData : RepositorioBase<Ubigeo>
{
    public UbigeoData(EvoRRLDbContext contexto) : base(contexto) { }

    public override Task Crear(Ubigeo registro) => throw SoloLectura();

    public override Task Actualizar(Ubigeo registro) => throw SoloLectura();

    public override Task Eliminar(Ubigeo registro) => throw SoloLectura();

    private static NotSupportedException SoloLectura()
        => new("El ubigeo es de solo lectura: se mantiene en Departamento, Provincia y Distrito.");
}
