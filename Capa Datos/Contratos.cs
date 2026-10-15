using Capa_Entidades;

namespace Capa_Datos;

// Contratos minimos que necesita EmpleadoServicio: solo declaran lo que se usa (IoC).
// Permiten sustituir la base de datos por un doble en las pruebas.
public interface IEmpleadoData
{
    Task<List<Empleado>> Listar();
}

public interface IPersonaData
{
    Task<List<Persona>> Listar();
}
