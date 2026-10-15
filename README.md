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
   - Carga los datos del sistema (puede tardar unos minutos). Por su tamaño, conviene ejecutarlo con
     `sqlcmd -S "(localdb)\MSSQLLocalDB" -E -f 65001 -i bd-Datos.sql`.
4. Ejecutar `Scripts/crear_bitacora.sql` sobre `RRHH_Nuevo` (tabla de la bitácora de actividad).
5. Abrir el proyecto en Visual Studio y presionar **F5**
6. Iniciar sesión con el usuario creado automáticamente al arrancar:
   - **Usuario:** `admin`
   - **Contraseña:** la de `Seguridad:AdminClave` en `appsettings.Development.json` (`Admin.2026` en desarrollo).
     Fuera de desarrollo se define con la variable de entorno `Seguridad__AdminClave`; si no existe, no se crea el usuario.

## Arquitectura

```
Proyecto-Evo-RRLL (controllers, vistas) ──► Capa Logica ──► Capa Entidades (entidades + contratos)
                                                                   ▲
                                          Capa Datos (EF, SQL) ────┘ implementa los contratos
```

- **Capa Entidades**: clases del negocio (`Empleado`, `Permiso`…) sin nada de base de datos, y los contratos de acceso a datos (`Contratos/IRepositorio<T>`, `IUnidadDeTrabajo`…).
- **Capa Logica**: reglas de negocio. Solo conoce los contratos, no referencia `Capa Datos` ni Entity Framework.
- **Capa Datos**: única capa ligada a la BD. Ahí están el `EvoRRLDbContext` con el mapeo de tablas, los repositorios (`*Data`) y la generación de claves (`Numeracion.cs`, "máximo + 1").
- **Proyecto-Evo-RRLL**: controllers y vistas usan la lógica. Solo `Program.cs` conoce la Capa Datos (`AgregarCapaDatos`).

**Cambiar de base de datos** (por ejemplo a `RRHH_Normalizado`) significa cambiar solo `Capa Datos` y la cadena de conexión. El detalle está en `Scripts/bd-Normalizada/NORMALIZACION.md`, sección 9. Las pruebas `ArquitecturaTests` fallan si alguna capa vuelve a depender de la base de datos.

Cómo se aplica cada tema del curso (semanas 2 a 9), con su evidencia en el código: [`docs/TEMAS-DEL-CURSO.md`](docs/TEMAS-DEL-CURSO.md). Gestión de la configuración: [`docs/GESTION-CONFIGURACION.md`](docs/GESTION-CONFIGURACION.md). Gestión del cambio: [`docs/GESTION-DEL-CAMBIO.md`](docs/GESTION-DEL-CAMBIO.md).

## Pruebas

```
dotnet test
```

El proyecto `Capa Logica.Tests` (xUnit) usa SQLite en memoria, así que no necesita SQL Server.

## Conexión a la base de datos

El archivo que hay que editar es `Proyecto-Evo-RRLL/appsettings.json`:

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
