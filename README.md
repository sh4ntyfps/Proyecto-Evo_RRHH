# Proyecto-Evo_RRLL

Sistema de Recursos Humanos (RR.HH.) en ASP.NET Core MVC (net10.0), arquitectura por capas: Entidades, Datos y Lógica.

## Requisitos

- .NET 10 SDK
- SQL Server LocalDB (`(localdb)\MSSQLLocalDB`) o SQL Server
- Visual Studio 2022+ o SSMS para los scripts

## Cómo levantar el proyecto

1. Clonar el repositorio (o descargarlo como ZIP).
2. Abrir `Scripts/bd-evo-RRLL.sql` en SQL Server Management Studio y ejecutarlo.
   - Crea la base de datos `RRHH_Nuevo` con sus 76 tablas y claves foráneas.
3. Descomprimir `Scripts/bd-Datos.zip` y ejecutar el `bd-Datos.sql` resultante sobre `RRHH_Nuevo`.
   - Carga los datos del sistema (puede tardar unos minutos).
4. Abrir el proyecto en Visual Studio y presionar **F5**
5. Iniciar sesión con el usuario creado automáticamente al arrancar:
   - **Usuario:** `admin`
   - **Contraseña:** `Admin.2026`

## Conexión a la base de datos

El único archivo que hay que editar es `Proyecto-Evo-RRLL/appsettings.json`:

```json
"ConnectionStrings": {
  "RRHHNuevo": "Server=(localdb)\\MSSQLLocalDB;Database=RRHH_Nuevo;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

Cambia el valor de `Server=` según tu caso:

- LocalDB/instalado en tu PC: `Server=LOCALHOST` o `Server=NOMBRE-PC\SQLEXPRESS`
- Otra máquina: `Server=IP_O_NOMBRE,1433`
- Con usuario/contraseña en vez de Windows: `Server=...;User Id=sa;Password=tuclave;`

La clase de conexión (`EvoRRLDbContext`) está en `Capa Datos` y recibe esta cadena automáticamente; no hay que tocar nada más.
