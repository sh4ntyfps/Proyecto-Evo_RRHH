/* ============================================================
   02-migracion-desde-RRHH_Nuevo.sql
   Copia y transforma los datos de [RRHH_Nuevo] -> [RRHH_Normalizado].

   Requisitos:
     - RRHH_Nuevo creada y cargada (bd-evo-RRLL.sql + bd-Datos.sql)
     - RRHH_Normalizado recien creada (01-esquema-RRHH_Normalizado.sql)

   - Todo corre en UNA transaccion: si algo falla no queda nada a medias.
   - Las FKs estan activas durante la carga, asi que quedan "confiables".
   - Cada dato que se corrige, descarta o mueve queda registrado en
     [mig].[Incidencia] para poder auditarlo.
   - RRHH_Nuevo no se modifica.
   ============================================================ */
USE [RRHH_Normalizado]
GO
SET NOCOUNT ON
SET XACT_ABORT ON
-- requeridos por la columna calculada de Distrito y el indice filtrado de Persona
-- (SSMS ya los trae en ON; sqlcmd no)
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
SET ARITHABORT ON
SET CONCAT_NULL_YIELDS_NULL ON
SET NUMERIC_ROUNDABORT OFF
GO

IF EXISTS (SELECT 1 FROM dbo.Persona) OR EXISTS (SELECT 1 FROM dbo.Empleado)
BEGIN
	RAISERROR('RRHH_Normalizado ya tiene datos. Ejecute primero 01-esquema-RRHH_Normalizado.sql para recrearla.', 16, 1);
	SET NOEXEC ON;   -- en SSMS evita que se ejecuten los lotes siguientes
END
GO

BEGIN TRY
BEGIN TRANSACTION;

/* ============================================================
   1. CATALOGOS (se conservan los mismos Id)
   ============================================================ */
SET IDENTITY_INSERT dbo.TipoDocID ON;
INSERT dbo.TipoDocID (TipoDocID, Descripcion) SELECT TipoDocID, Descripcion FROM RRHH_Nuevo.dbo.TipoDocID;
SET IDENTITY_INSERT dbo.TipoDocID OFF;

SET IDENTITY_INSERT dbo.EstadoCivil ON;
INSERT dbo.EstadoCivil (IdEstadoCivil, DescripEstCivil, AbrevEstCiv) SELECT IdEstadoCivil, DescripEstCivil, AbrevEstCiv FROM RRHH_Nuevo.dbo.EstadoCivil;
SET IDENTITY_INSERT dbo.EstadoCivil OFF;

SET IDENTITY_INSERT dbo.Nacionalidad ON;
INSERT dbo.Nacionalidad (IdNacionalidad, Descripcion, Abreviatura) SELECT IdNacionalidad, Descripcion, Abreviatura FROM RRHH_Nuevo.dbo.Nacionalidad;
SET IDENTITY_INSERT dbo.Nacionalidad OFF;

SET IDENTITY_INSERT dbo.TipoSangre ON;
INSERT dbo.TipoSangre (IdTipoSangre, DescripTS) SELECT IdTipoSangre, DescripTS FROM RRHH_Nuevo.dbo.TipoSangre;
SET IDENTITY_INSERT dbo.TipoSangre OFF;

SET IDENTITY_INSERT dbo.Discapacidad ON;
INSERT dbo.Discapacidad (IdDiscapacidad, Descrip_Discapacidad) SELECT IdDiscapacidad, Descrip_Discapacidad FROM RRHH_Nuevo.dbo.Discapacidad;
SET IDENTITY_INSERT dbo.Discapacidad OFF;

SET IDENTITY_INSERT dbo.AFP ON;
INSERT dbo.AFP (IdAFP, DescripAFP, DireccAFP, TelefAFP, NomContAFP) SELECT IdAFP, DescripAFP, DireccAFP, TelefAFP, NomContAFP FROM RRHH_Nuevo.dbo.AFP;
SET IDENTITY_INSERT dbo.AFP OFF;

SET IDENTITY_INSERT dbo.RegimenPension ON;
INSERT dbo.RegimenPension (IdRegimenPen, LeyRegimen) SELECT IdRegimenPen, LeyRegimen FROM RRHH_Nuevo.dbo.RegimenPension;
SET IDENTITY_INSERT dbo.RegimenPension OFF;

SET IDENTITY_INSERT dbo.TipoTrabajador ON;
INSERT dbo.TipoTrabajador (IdTipoTrabajador, Descripcion, Observaciones) SELECT IdTipoTrabajador, Descripcion, Observaciones FROM RRHH_Nuevo.dbo.TipoTrabajador;
SET IDENTITY_INSERT dbo.TipoTrabajador OFF;

SET IDENTITY_INSERT dbo.Cargo ON;
INSERT dbo.Cargo (IdCargo, Cargo, Abreviatura) SELECT IdCargo, Cargo, Abreviatura FROM RRHH_Nuevo.dbo.Cargo;
SET IDENTITY_INSERT dbo.Cargo OFF;

SET IDENTITY_INSERT dbo.Local ON;
INSERT dbo.Local (idLocal, NombreLocal, Estado) SELECT idLocal, NombreLocal, Estado FROM RRHH_Nuevo.dbo.Local;
SET IDENTITY_INSERT dbo.Local OFF;

SET IDENTITY_INSERT dbo.ViveCon ON;
INSERT dbo.ViveCon (IdViveCon, DescripViveCon) SELECT IdViveCon, DescripViveCon FROM RRHH_Nuevo.dbo.ViveCon;
SET IDENTITY_INSERT dbo.ViveCon OFF;

SET IDENTITY_INSERT dbo.TipoFamiliar ON;
INSERT dbo.TipoFamiliar (IdTipoFam, DescripTipoFam) SELECT IdTipoFam, DescripTipoFam FROM RRHH_Nuevo.dbo.TipoFamiliar;
SET IDENTITY_INSERT dbo.TipoFamiliar OFF;

SET IDENTITY_INSERT dbo.MotivoBaja ON;
INSERT dbo.MotivoBaja (idMotivoBaja, Descripcion) SELECT idMotivoBaja, Descripcion FROM RRHH_Nuevo.dbo.MotivoBaja;
SET IDENTITY_INSERT dbo.MotivoBaja OFF;

SET IDENTITY_INSERT dbo.TipoEstudio ON;
INSERT dbo.TipoEstudio (IdTipoEstudios, DescripTipoEst) SELECT IdTipoEstudios, DescripTipoEst FROM RRHH_Nuevo.dbo.TipoEstudio;
SET IDENTITY_INSERT dbo.TipoEstudio OFF;

SET IDENTITY_INSERT dbo.EstadoEstudio ON;
INSERT dbo.EstadoEstudio (IdEstadoEstudio, DescripEstadoEstudio) SELECT IdEstadoEstudio, DescripEstadoEstudio FROM RRHH_Nuevo.dbo.EstadoEstudio;
SET IDENTITY_INSERT dbo.EstadoEstudio OFF;

SET IDENTITY_INSERT dbo.TipoInstitucion ON;
INSERT dbo.TipoInstitucion (IdTipoInstitucion, DescripTipoInstitucion) SELECT IdTipoInstitucion, DescripTipoInstitucion FROM RRHH_Nuevo.dbo.TipoInstitucion;
SET IDENTITY_INSERT dbo.TipoInstitucion OFF;

SET IDENTITY_INSERT dbo.TipoComportamiento ON;
INSERT dbo.TipoComportamiento (idTipoComportamiento, TipoCompDescrip) SELECT idTipoComportamiento, TipoCompDescrip FROM RRHH_Nuevo.dbo.TipoComportamiento;
SET IDENTITY_INSERT dbo.TipoComportamiento OFF;

-- TipoResolucion: se agregan 2 tipos para las resoluciones que estaban como columnas en Empleado
DECLARE @TipoResIngresoEstado int, @TipoResIngresoInstitucion int;
SET IDENTITY_INSERT dbo.TipoResolucion ON;
INSERT dbo.TipoResolucion (idTipoResolucion, DescripTipoResolucion) SELECT idTipoResolucion, DescripTipoResolucion FROM RRHH_Nuevo.dbo.TipoResolucion;
SELECT @TipoResIngresoEstado = MAX(idTipoResolucion) + 1 FROM dbo.TipoResolucion;
SET @TipoResIngresoInstitucion = @TipoResIngresoEstado + 1;
INSERT dbo.TipoResolucion (idTipoResolucion, DescripTipoResolucion) VALUES
	(@TipoResIngresoEstado,      N'Ingreso al Estado'),
	(@TipoResIngresoInstitucion, N'Ingreso a la Institución');
SET IDENTITY_INSERT dbo.TipoResolucion OFF;

SET IDENTITY_INSERT dbo.TipoPermiso ON;
INSERT dbo.TipoPermiso (idTipoPermiso, DescripTipoPermiso) SELECT idTipoPermiso, DescripTipoPermiso FROM RRHH_Nuevo.dbo.TipoPermiso;
SET IDENTITY_INSERT dbo.TipoPermiso OFF;

SET IDENTITY_INSERT dbo.Motivo_Perm ON;
INSERT dbo.Motivo_Perm (idMotivo, Descrip_Motivo, Salario, idTipoPermiso, Abrev, TipoModalidad, Activo)
SELECT idMotivo, Descrip_Motivo, Salario, idTipoPermiso, Abrev, NULLIF(LTRIM(RTRIM(TipoModalidad)), ''), Activo FROM RRHH_Nuevo.dbo.Motivo_Perm;
SET IDENTITY_INSERT dbo.Motivo_Perm OFF;

SET IDENTITY_INSERT dbo.RRHH_Feriado ON;
INSERT dbo.RRHH_Feriado (idFeriado, Fecha, Motivo) SELECT idFeriado, CAST(Fecha AS date), Motivo FROM RRHH_Nuevo.dbo.RRHH_Feriado;
SET IDENTITY_INSERT dbo.RRHH_Feriado OFF;

/* -------- vivienda -------- */
SET IDENTITY_INSERT dbo.TenenciaViv ON;     INSERT dbo.TenenciaViv (IdTenencia, DescripTenencia)         SELECT IdTenencia, DescripTenencia FROM RRHH_Nuevo.dbo.TenenciaViv;         SET IDENTITY_INSERT dbo.TenenciaViv OFF;
SET IDENTITY_INSERT dbo.TipoViv ON;         INSERT dbo.TipoViv (IdTipo, DescripTipo)                     SELECT IdTipo, DescripTipo FROM RRHH_Nuevo.dbo.TipoViv;                     SET IDENTITY_INSERT dbo.TipoViv OFF;
SET IDENTITY_INSERT dbo.MaterialViv ON;     INSERT dbo.MaterialViv (IdMaterial, DescripMaterial)         SELECT IdMaterial, DescripMaterial FROM RRHH_Nuevo.dbo.MaterialViv;         SET IDENTITY_INSERT dbo.MaterialViv OFF;
SET IDENTITY_INSERT dbo.ConservacionViv ON; INSERT dbo.ConservacionViv (IdConservacion, DescripConserva) SELECT IdConservacion, DescripConserva FROM RRHH_Nuevo.dbo.ConservacionViv; SET IDENTITY_INSERT dbo.ConservacionViv OFF;
SET IDENTITY_INSERT dbo.UbicacionViv ON;    INSERT dbo.UbicacionViv (IdUbicacion, DescripUbicacion)      SELECT IdUbicacion, DescripUbicacion FROM RRHH_Nuevo.dbo.UbicacionViv;      SET IDENTITY_INSERT dbo.UbicacionViv OFF;
SET IDENTITY_INSERT dbo.AlumbradoViv ON;    INSERT dbo.AlumbradoViv (IdAlumbrado, DescripAlumbrado)      SELECT IdAlumbrado, DescripAlumbrado FROM RRHH_Nuevo.dbo.AlumbradoViv;      SET IDENTITY_INSERT dbo.AlumbradoViv OFF;
SET IDENTITY_INSERT dbo.AguaViv ON;         INSERT dbo.AguaViv (IdAgua, DescripAgua)                     SELECT IdAgua, DescripAgua FROM RRHH_Nuevo.dbo.AguaViv;                     SET IDENTITY_INSERT dbo.AguaViv OFF;
SET IDENTITY_INSERT dbo.ExcretasViv ON;     INSERT dbo.ExcretasViv (IdExcretas, DescripExcretas)         SELECT IdExcretas, DescripExcretas FROM RRHH_Nuevo.dbo.ExcretasViv;         SET IDENTITY_INSERT dbo.ExcretasViv OFF;
SET IDENTITY_INSERT dbo.ServicioViv ON;
INSERT dbo.ServicioViv (IdServicio, Descripcion) VALUES (1, 'TELEFONO'), (2, 'CABLE'), (3, 'INTERNET'), (4, 'OTROS');
SET IDENTITY_INSERT dbo.ServicioViv OFF;

/* -------- ficha social -------- */
SET IDENTITY_INSERT dbo.RRHH_AcudeEnferm ON;  INSERT dbo.RRHH_AcudeEnferm (idAcudeEnferm, AcudeEnfermDescrip)   SELECT idAcudeEnferm, AcudeEnfermDescrip FROM RRHH_Nuevo.dbo.RRHH_AcudeEnferm;   SET IDENTITY_INSERT dbo.RRHH_AcudeEnferm OFF;
SET IDENTITY_INSERT dbo.RRHH_TipoFamilia ON;  INSERT dbo.RRHH_TipoFamilia (idTipoFam, TipoFamDescrip)           SELECT idTipoFam, TipoFamDescrip FROM RRHH_Nuevo.dbo.RRHH_TipoFamilia;           SET IDENTITY_INSERT dbo.RRHH_TipoFamilia OFF;
SET IDENTITY_INSERT dbo.RRHH_RelacPareja ON;  INSERT dbo.RRHH_RelacPareja (idRelacPareja, RelacParejaDescrip)   SELECT idRelacPareja, RelacParejaDescrip FROM RRHH_Nuevo.dbo.RRHH_RelacPareja;   SET IDENTITY_INSERT dbo.RRHH_RelacPareja OFF;
SET IDENTITY_INSERT dbo.RRHH_RelacPH ON;      INSERT dbo.RRHH_RelacPH (idRelacPH, RelacPHDescrip)               SELECT idRelacPH, RelacPHDescrip FROM RRHH_Nuevo.dbo.RRHH_RelacPH;               SET IDENTITY_INSERT dbo.RRHH_RelacPH OFF;
SET IDENTITY_INSERT dbo.RRHH_RelacHermano ON; INSERT dbo.RRHH_RelacHermano (idRelacHermano, RelacHermanoDescrip) SELECT idRelacHermano, RelacHermanoDescrip FROM RRHH_Nuevo.dbo.RRHH_RelacHermano; SET IDENTITY_INSERT dbo.RRHH_RelacHermano OFF;

INSERT dbo.RRHH_FuncFamPregunta (IdPregunta, Codigo, Dimension, Orden) VALUES
	(1, 'Com1',   'Comunicacion',      1), (2, 'Com2',   'Comunicacion',      2),
	(3, 'Afec1',  'Afectividad',       3), (4, 'Afec2',  'Afectividad',       4),
	(5, 'Decis1', 'Toma de decisiones',5), (6, 'Decis2', 'Toma de decisiones',6),
	(7, 'Soc1',   'Socializacion',     7), (8, 'Soc2',   'Socializacion',     8),
	(9, 'Soc3',   'Socializacion',     9);

INSERT dbo.VinculoAsegurado (Descripcion)
SELECT DISTINCT LTRIM(RTRIM(VinculoFamiliar)) FROM RRHH_Nuevo.dbo.RRHH_Asegurado
WHERE NULLIF(LTRIM(RTRIM(VinculoFamiliar)), '') IS NOT NULL;

/* -------- asistencia: catalogos nuevos sacados de los codigos usados -------- */
INSERT dbo.EstadoAsistencia (Codigo)
SELECT DISTINCT LTRIM(RTRIM(Estado)) FROM RRHH_Nuevo.dbo.RegAsisDiario WHERE NULLIF(LTRIM(RTRIM(Estado)), '') IS NOT NULL;

INSERT dbo.TipoMarcacion (Codigo)
SELECT DISTINCT SUBSTRING(Estado, 2, 1) FROM RRHH_Nuevo.dbo.Marcacion;

INSERT dbo.DispositivoMarcacion (Codigo, Descripcion)
SELECT DISTINCT LTRIM(RTRIM(Lugar)), CASE WHEN LTRIM(RTRIM(Lugar)) = '*' THEN 'NO ESPECIFICADO (*)' END
FROM RRHH_Nuevo.dbo.Marcacion WHERE NULLIF(LTRIM(RTRIM(Lugar)), '') IS NOT NULL;

SET IDENTITY_INSERT dbo.Horario ON;
INSERT dbo.Horario (idHorario, Descrip_Horario, Ingreso1, Salida1, Ingreso2, Salida2, SgtDia, Estado)
SELECT idHorario, Descrip_Horario,
       TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(Ingreso1)), '')), TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(Salida1)), '')),
       TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(Ingreso2)), '')), TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(Salida2)), '')),
       SgtDia, Estado
FROM RRHH_Nuevo.dbo.Horario;
SET IDENTITY_INSERT dbo.Horario OFF;

/* ============================================================
   2. UBIGEO: se separan los 3 niveles y se descartan los codigos
      mal formados (sin cero inicial, p.ej. '10101' en vez de '010101')
   ============================================================ */
INSERT mig.Incidencia (Tabla, Clave, Campo, ValorOriginal, Accion, Detalle)
SELECT 'Ubigeo', RTRIM(u.IdUbigeo), 'IdUbigeo', u.Descripcion, 'DESCARTADO',
       CASE WHEN g.IdUbigeo IS NOT NULL THEN 'Codigo sin cero inicial; duplicado de ' + g.IdUbigeo
            ELSE 'Codigo mal formado sin equivalente valido' END
FROM RRHH_Nuevo.dbo.Ubigeo u
LEFT JOIN RRHH_Nuevo.dbo.Ubigeo g ON g.IdUbigeo = RIGHT('000000' + RTRIM(u.IdUbigeo), 6) AND LEN(RTRIM(g.IdUbigeo)) = 6
WHERE LEN(RTRIM(u.IdUbigeo)) < 6;

INSERT dbo.Departamento (CodDepartamento, Nombre)
SELECT Departamento, LTRIM(RTRIM(Descripcion)) FROM RRHH_Nuevo.dbo.Ubigeo
WHERE LEN(RTRIM(IdUbigeo)) = 6 AND Provincia = '00' AND Distrito = '00';

INSERT dbo.Provincia (CodDepartamento, CodProvincia, Nombre)
SELECT Departamento, Provincia, LTRIM(RTRIM(Descripcion)) FROM RRHH_Nuevo.dbo.Ubigeo
WHERE LEN(RTRIM(IdUbigeo)) = 6 AND Provincia <> '00' AND Distrito = '00';

INSERT dbo.Distrito (CodDepartamento, CodProvincia, CodDistrito, Nombre)
SELECT Departamento, Provincia, Distrito, LTRIM(RTRIM(Descripcion)) FROM RRHH_Nuevo.dbo.Ubigeo
WHERE LEN(RTRIM(IdUbigeo)) = 6 AND Distrito <> '00';

/* ============================================================
   3. PERSONA
      - Solo personas de RRHH (referenciadas por Empleado, Familiar o
        RRHH_Asegurado). Las demas eran proveedores de logistica.
      - Los atributos personales que estaban repetidos en Empleado,
        Familiar y RRHH_Asegurado se consolidan aqui con esta prioridad:
          1) vinculo laboral mas reciente  2) registro como familiar
          3) registro como asegurado
        Si las fuentes discrepan se registra en mig.Incidencia.
   ============================================================ */
SELECT p.IdPersona
INTO #PersonaRRHH
FROM RRHH_Nuevo.dbo.Persona p
WHERE EXISTS (SELECT 1 FROM RRHH_Nuevo.dbo.Empleado e WHERE e.IdPersona = p.IdPersona)
   OR EXISTS (SELECT 1 FROM RRHH_Nuevo.dbo.Familiar f WHERE f.IdPersona = p.IdPersona)
   OR EXISTS (SELECT 1 FROM RRHH_Nuevo.dbo.RRHH_Asegurado a WHERE a.idPersona = p.IdPersona);

INSERT mig.Incidencia (Tabla, Clave, Accion, Detalle)
SELECT 'Persona', NULL, 'NO MIGRADO',
       CAST(COUNT(*) AS varchar(10)) + ' personas no referenciadas por RRHH (proveedores/personas juridicas de logistica). Siguen en RRHH_Nuevo.'
FROM RRHH_Nuevo.dbo.Persona p WHERE NOT EXISTS (SELECT 1 FROM #PersonaRRHH r WHERE r.IdPersona = p.IdPersona);

-- todas las fuentes de atributos personales, en formato largo (persona, atributo, valor)
WITH Fuentes AS (
	SELECT e.IdPersona, 1 AS Prioridad, e.IdEmpleado AS Orden,
	       CAST(NULLIF(LTRIM(RTRIM(e.Sexo)), '') AS nvarchar(200))              AS Sexo,
	       CAST(e.IdEstadoCivil AS nvarchar(200))                               AS IdEstadoCivil,
	       CAST(e.IdNacionalidad AS nvarchar(200))                              AS IdNacionalidad,
	       CAST(e.IdTipoSangre AS nvarchar(200))                                AS IdTipoSangre,
	       CAST(e.IdDiscapacidad AS nvarchar(200))                              AS IdDiscapacidad,
	       CAST(NULLIF(LTRIM(RTRIM(e.NumAutogenSalud)), '') AS nvarchar(200))   AS NumAutogenSalud,
	       CAST(NULLIF(LTRIM(RTRIM(e.NumRUC)), '') AS nvarchar(200))            AS NumRUC,
	       CAST(NULLIF(LTRIM(RTRIM(e.Brevete)), '') AS nvarchar(200))           AS Brevete,
	       CAST(NULLIF(LTRIM(RTRIM(e.NumLibretaMilitar)), '') AS nvarchar(200)) AS NumLibretaMilitar,
	       CAST(NULLIF(LTRIM(RTRIM(e.GradoInstruccion)), '') AS nvarchar(200))  AS GradoInstruccion,
	       CAST(NULLIF(LTRIM(RTRIM(e.Profesion)), '') AS nvarchar(200))         AS Profesion,
	       CAST(NULL AS nvarchar(200)) AS Ocupacion, CAST(NULL AS nvarchar(200)) AS LugarTrabajo,
	       CAST(NULL AS nvarchar(200)) AS Procedencia, CAST(NULL AS nvarchar(200)) AS Enfermedad,
	       CAST(NULL AS nvarchar(200)) AS TiempoEnfermedad
	FROM RRHH_Nuevo.dbo.Empleado e
	UNION ALL
	SELECT f.IdPersona, 2, f.IdEmpleado,
	       CAST(NULLIF(LTRIM(RTRIM(f.FamSexo)), '') AS nvarchar(200)),
	       CAST(f.IdEstadoCivil AS nvarchar(200)), NULL,
	       CAST(f.IdTipoSangre AS nvarchar(200)),
	       CAST(f.IdDiscapacidad AS nvarchar(200)),
	       CAST(NULLIF(LTRIM(RTRIM(f.NumAutogenSalud)), '') AS nvarchar(200)),
	       NULL, NULL, NULL,
	       CAST(NULLIF(LTRIM(RTRIM(f.FamGradoInstruc)), '') AS nvarchar(200)),
	       NULL,
	       CAST(NULLIF(LTRIM(RTRIM(f.FamOcupacion)), '') AS nvarchar(200)),
	       CAST(NULLIF(LTRIM(RTRIM(f.FamLugTrab)), '') AS nvarchar(200)),
	       CAST(NULLIF(LTRIM(RTRIM(f.Procedencia)), '') AS nvarchar(200)),
	       CAST(NULLIF(LTRIM(RTRIM(f.Enfermedad)), '') AS nvarchar(200)),
	       CAST(NULLIF(LTRIM(RTRIM(f.TiempoEnfermedad)), '') AS nvarchar(200))
	FROM RRHH_Nuevo.dbo.Familiar f
	UNION ALL
	SELECT a.idPersona, 3, a.idEmpleado,
	       CAST(NULLIF(LTRIM(RTRIM(a.Sexo)), '') AS nvarchar(200)),
	       CAST(a.idEstadoCivil AS nvarchar(200)),
	       NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
	FROM RRHH_Nuevo.dbo.RRHH_Asegurado a
	WHERE a.idPersona IS NOT NULL
)
SELECT u.IdPersona, u.Prioridad, u.Orden, u.Atributo, u.Valor
INTO #AtributoPersona
FROM Fuentes
UNPIVOT (Valor FOR Atributo IN (Sexo, IdEstadoCivil, IdNacionalidad, IdTipoSangre, IdDiscapacidad, NumAutogenSalud,
                                NumRUC, Brevete, NumLibretaMilitar, GradoInstruccion, Profesion, Ocupacion,
                                LugarTrabajo, Procedencia, Enfermedad, TiempoEnfermedad)) u;

SELECT IdPersona, Atributo, Valor
INTO #AtributoElegido
FROM (SELECT IdPersona, Atributo, Valor,
             ROW_NUMBER() OVER (PARTITION BY IdPersona, Atributo ORDER BY Prioridad, Orden DESC) AS rn
      FROM #AtributoPersona) x
WHERE rn = 1;

INSERT mig.Incidencia (Tabla, Clave, Campo, ValorOriginal, Accion, Detalle)
SELECT 'Persona', CAST(c.IdPersona AS varchar(20)), c.Atributo, c.Valores, 'CONSOLIDADO',
       'Valores distintos entre fuentes; se conservo: ' + el.Valor
FROM (SELECT d.IdPersona, d.Atributo, STRING_AGG(CAST(d.Valor AS nvarchar(max)), N' | ') AS Valores
      FROM (SELECT DISTINCT IdPersona, Atributo, Valor FROM #AtributoPersona) d
      GROUP BY d.IdPersona, d.Atributo
      HAVING COUNT(*) > 1) c
JOIN #AtributoElegido el ON el.IdPersona = c.IdPersona AND el.Atributo = c.Atributo;

-- ubigeos que no apuntan a un distrito valido (vacios o de nivel provincia/departamento)
INSERT mig.Incidencia (Tabla, Clave, Campo, ValorOriginal, Accion, Detalle)
SELECT 'Persona', CAST(p.IdPersona AS varchar(20)), v.Campo, v.Valor, 'ANULADO',
       CASE WHEN NULLIF(LTRIM(RTRIM(v.Valor)), '') IS NULL THEN 'Codigo vacio'
            ELSE 'No es un distrito valido (nivel provincia/departamento o inexistente)' END
FROM RRHH_Nuevo.dbo.Persona p
JOIN #PersonaRRHH r ON r.IdPersona = p.IdPersona
CROSS APPLY (VALUES ('IdUbigeo', p.IdUbigeo), ('UbigeoDireccion', p.UbigeoDireccion)) v(Campo, Valor)
WHERE v.Valor IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM dbo.Distrito d WHERE d.IdUbigeo = RTRIM(v.Valor));

SET IDENTITY_INSERT dbo.Persona ON;
INSERT dbo.Persona (IdPersona, TipoPersona, Nombres, Apellido_Paterno, Apellido_Materno, TipoDocID, NumDocID,
                    Sexo, FechaNacimiento, IdEstadoCivil, IdNacionalidad, IdTipoSangre, IdDiscapacidad,
                    NumAutogenSalud, NumRUC, Brevete, NumLibretaMilitar, GradoInstruccion, Profesion,
                    Ocupacion, LugarTrabajo, Procedencia, Enfermedad, TiempoEnfermedad,
                    Direccion, IdUbigeo, UbigeoDireccion, Telefono, NumCelular, Email, Foto, FechaRegistro)
SELECT p.IdPersona,
       CASE WHEN p.TipoPersona = 'J' THEN 'J' ELSE 'N' END,
       p.Nombres, p.Apellido_Paterno, p.Apellido_Materno, p.TipoDocID, NULLIF(LTRIM(RTRIM(p.NumDocID)), ''),
       a.Sexo, CAST(p.FechaNacimiento AS date),
       CAST(a.IdEstadoCivil AS int), CAST(a.IdNacionalidad AS int), CAST(a.IdTipoSangre AS int), CAST(a.IdDiscapacidad AS int),
       a.NumAutogenSalud, a.NumRUC, a.Brevete, a.NumLibretaMilitar, a.GradoInstruccion, a.Profesion,
       a.Ocupacion, a.LugarTrabajo, a.Procedencia, a.Enfermedad, a.TiempoEnfermedad,
       p.Direccion,
       (SELECT d.IdUbigeo FROM dbo.Distrito d WHERE d.IdUbigeo = RTRIM(p.IdUbigeo)),
       (SELECT d.IdUbigeo FROM dbo.Distrito d WHERE d.IdUbigeo = RTRIM(p.UbigeoDireccion)),
       p.Telefono, p.NumCelular, p.Email,
       (SELECT TOP 1 CAST(e.Foto AS varbinary(max)) FROM RRHH_Nuevo.dbo.Empleado e
         WHERE e.IdPersona = p.IdPersona AND e.Foto IS NOT NULL ORDER BY e.IdEmpleado DESC),
       p.FechaRegistro
FROM RRHH_Nuevo.dbo.Persona p
JOIN #PersonaRRHH r ON r.IdPersona = p.IdPersona
LEFT JOIN (
	SELECT IdPersona,
	       MAX(CASE WHEN Atributo = 'Sexo'              THEN Valor END) AS Sexo,
	       MAX(CASE WHEN Atributo = 'IdEstadoCivil'     THEN Valor END) AS IdEstadoCivil,
	       MAX(CASE WHEN Atributo = 'IdNacionalidad'    THEN Valor END) AS IdNacionalidad,
	       MAX(CASE WHEN Atributo = 'IdTipoSangre'      THEN Valor END) AS IdTipoSangre,
	       MAX(CASE WHEN Atributo = 'IdDiscapacidad'    THEN Valor END) AS IdDiscapacidad,
	       MAX(CASE WHEN Atributo = 'NumAutogenSalud'   THEN Valor END) AS NumAutogenSalud,
	       MAX(CASE WHEN Atributo = 'NumRUC'            THEN Valor END) AS NumRUC,
	       MAX(CASE WHEN Atributo = 'Brevete'           THEN Valor END) AS Brevete,
	       MAX(CASE WHEN Atributo = 'NumLibretaMilitar' THEN Valor END) AS NumLibretaMilitar,
	       MAX(CASE WHEN Atributo = 'GradoInstruccion'  THEN Valor END) AS GradoInstruccion,
	       MAX(CASE WHEN Atributo = 'Profesion'         THEN Valor END) AS Profesion,
	       MAX(CASE WHEN Atributo = 'Ocupacion'         THEN Valor END) AS Ocupacion,
	       MAX(CASE WHEN Atributo = 'LugarTrabajo'      THEN Valor END) AS LugarTrabajo,
	       MAX(CASE WHEN Atributo = 'Procedencia'       THEN Valor END) AS Procedencia,
	       MAX(CASE WHEN Atributo = 'Enfermedad'        THEN Valor END) AS Enfermedad,
	       MAX(CASE WHEN Atributo = 'TiempoEnfermedad'  THEN Valor END) AS TiempoEnfermedad
	FROM #AtributoElegido GROUP BY IdPersona
) a ON a.IdPersona = p.IdPersona;
SET IDENTITY_INSERT dbo.Persona OFF;

/* ============================================================
   4. EMPLEADO (vinculo laboral)
   ============================================================ */
SET IDENTITY_INSERT dbo.Empleado ON;
INSERT dbo.Empleado (IdEmpleado, IdPersona, FechaIngreso, Estado, IdTipoTrabajador, IdCargo, idHorario, idLocal,
                     IdRegimenPen, IdAFP, NroAFP, FechaAFP, CtaCorriente, DecLey, ExpSocial, CargaFam, JefeHogar, IdViveCon)
SELECT e.IdEmpleado, e.IdPersona, CAST(e.FechaIngreso AS date),
       CASE e.Estado WHEN '1' THEN 1 WHEN '0' THEN 0 END,
       e.IdTipoTrabajador, e.IdCargo, e.idHorario, e.idLocal,
       e.IdRegimenPen, e.IdAFP, NULLIF(LTRIM(RTRIM(e.NroAFP)), ''), CAST(e.FechaAFP AS date),
       NULLIF(LTRIM(RTRIM(e.CtaCorriente)), ''), NULLIF(LTRIM(RTRIM(e.DecLey)), ''), NULLIF(LTRIM(RTRIM(e.ExpSocial)), ''),
       TRY_CONVERT(tinyint, NULLIF(LTRIM(RTRIM(e.CargaFam)), '')), e.JefeHogar, e.IdViveCon
FROM RRHH_Nuevo.dbo.Empleado e;
SET IDENTITY_INSERT dbo.Empleado OFF;

-- columnas eliminadas de Empleado que tenian datos
INSERT mig.Incidencia (Tabla, Clave, Campo, ValorOriginal, Accion, Detalle)
SELECT 'Empleado', CAST(IdEmpleado AS varchar(20)), 'Userid', CAST(Userid AS nvarchar(50)), 'DESCARTADO',
       'Referencia a aspnet_Users (tabla eliminada). El vinculo usuario-empleado vive en Usuario.IdEmpleado'
FROM RRHH_Nuevo.dbo.Empleado WHERE Userid IS NOT NULL;

INSERT mig.Incidencia (Tabla, Clave, Campo, ValorOriginal, Accion, Detalle)
SELECT 'Empleado', CAST(e.IdEmpleado AS varchar(20)), 'NumHijos', e.NumHijos, 'DERIVADO',
       'No coincide con los hijos registrados en Familiar (' + CAST(h.n AS varchar(10)) + '). Ahora se calcula en vw_EmpleadoResumenFamiliar'
FROM RRHH_Nuevo.dbo.Empleado e
CROSS APPLY (SELECT COUNT(*) AS n FROM RRHH_Nuevo.dbo.Familiar f WHERE f.IdEmpleado = e.IdEmpleado AND f.IdTipoFam = 4) h
WHERE NULLIF(LTRIM(RTRIM(e.NumHijos)), '') IS NOT NULL AND TRY_CONVERT(int, e.NumHijos) <> h.n;

/* ============================================================
   5. ESTRUCTURA ORGANIZACIONAL Y AREAS
   ============================================================ */
INSERT dbo.EstructOrganiz ([Year], idAreaOrganiz, AreaOrganizacional, Abrev, Sigla, Mision, Vision, YearDe, idDependeDe, IdEmpleado)
SELECT o.[Year], o.idAreaOrganiz, o.AreaOrganizacional, o.Abrev, o.Sigla, o.Mision, o.Vision, o.YearDe, o.idDependeDe,
       CASE WHEN EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.IdEmpleado = o.IdEmpleado) THEN o.IdEmpleado END
FROM RRHH_Nuevo.dbo.EstructOrganiz o;

INSERT mig.Incidencia (Tabla, Clave, Campo, ValorOriginal, Accion, Detalle)
SELECT 'EstructOrganiz', CAST(o.[Year] AS varchar(4)) + '-' + CAST(o.idAreaOrganiz AS varchar(10)), 'IdEmpleado',
       CAST(o.IdEmpleado AS nvarchar(20)), 'ANULADO', 'Jefe de area apunta a un empleado inexistente'
FROM RRHH_Nuevo.dbo.EstructOrganiz o
WHERE o.IdEmpleado IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Empleado e WHERE e.IdEmpleado = o.IdEmpleado);

INSERT mig.Incidencia (Tabla, Accion, Detalle) VALUES
	('EstructOrganiz', 'NO MIGRADO', 'Columnas de presupuesto/logistica (PresupAnual, IdPresupuesto, InversPublic, ApruebaPedido, MontoAprobacion, PresupCompra, PresupUtilizado): fuera del dominio RRHH. Siguen en RRHH_Nuevo.'),
	('EstructOrganiz', 'DERIVADO',   'NroTrabajaUO se calcula en vw_EstructOrganizDotacion; PromedMensSueld depende de planillas (no existen en esta BD).');

-- historial de areas + el area que estaba guardada en Empleado (si no estaba ya en el historial)
INSERT dbo.Empleado_Area ([Year], idAreaOrganiz, IdEmpleado)
SELECT [Year], idAreaOrganiz, IdEmpleado FROM RRHH_Nuevo.dbo.Empleado_Area
UNION
SELECT [Year], idAreaOrganiz, IdEmpleado FROM RRHH_Nuevo.dbo.Empleado;

INSERT mig.Incidencia (Tabla, Clave, Campo, ValorOriginal, Accion, Detalle)
SELECT 'Empleado', CAST(e.IdEmpleado AS varchar(20)), 'Year/idAreaOrganiz',
       CAST(e.[Year] AS varchar(4)) + '-' + CAST(e.idAreaOrganiz AS varchar(10)), 'MOVIDO',
       'Area guardada en Empleado que no estaba en el historial; se agrego a Empleado_Area'
FROM RRHH_Nuevo.dbo.Empleado e
WHERE NOT EXISTS (SELECT 1 FROM RRHH_Nuevo.dbo.Empleado_Area a
                  WHERE a.IdEmpleado = e.IdEmpleado AND a.[Year] = e.[Year] AND a.idAreaOrganiz = e.idAreaOrganiz);

INSERT dbo.Rotacion (idEmpleado, idRotacion, FechaMemo, NroMemo, [Year], idAreaOrganiz)
SELECT idEmpleado, idRotacion, FechaMemo, NroMemo, [Year], idAreaOrganiz FROM RRHH_Nuevo.dbo.Rotacion;

SET IDENTITY_INSERT dbo.ResponsableXUO ON;
INSERT dbo.ResponsableXUO (IdResponsable, [Year], idAreaOrganiz, IdEmpleado, F_Inicio, F_Termino)
SELECT IdResponsable, [Year], idAreaOrganiz, IdEmpleado, CAST(F_Inicio AS date), CAST(F_Termino AS date)
FROM RRHH_Nuevo.dbo.ResponsableXUO;
SET IDENTITY_INSERT dbo.ResponsableXUO OFF;

INSERT mig.Incidencia (Tabla, Accion, Detalle)
SELECT 'ResponsableXUO', 'DESCARTADO',
       CAST(COUNT(*) AS varchar(10)) + ' filas: AreaOrganizacional e idDependeDe eran copia de EstructOrganiz (dependencia transitiva). '
       + CAST(SUM(CASE WHEN ISNULL(r.AreaOrganizacional, '') <> o.AreaOrganizacional THEN 1 ELSE 0 END) AS varchar(10))
       + ' de ellas ya tenian un nombre de area distinto al maestro.'
FROM RRHH_Nuevo.dbo.ResponsableXUO r
JOIN RRHH_Nuevo.dbo.EstructOrganiz o ON o.[Year] = r.[Year] AND o.idAreaOrganiz = r.idAreaOrganiz;

/* ============================================================
   6. VIDA LABORAL Y RESOLUCIONES
   ============================================================ */
INSERT dbo.PeriodoLaboral (IdEmpleado, NroPeriodo, FechaIngreso, FechaSalida, NumResoIngreInstitu, FechaResoIngreInstitu, Judicial, Obs)
SELECT IdEmpleado, NroPeriodo, CAST(FechaIngreso AS date), CAST(FechaSalida AS date),
       NULLIF(LTRIM(RTRIM(NumResoIngreInstitu)), ''), CAST(FechaResoIngreInstitu AS date), Judicial, Obs
FROM RRHH_Nuevo.dbo.PeriodoLaboral;

INSERT dbo.Resolucion (idEmpleado, idResolucion, idTipoResolucion, NumeroResolucion, FechaResolucion, FechaVigencia)
SELECT idEmpleado, idResolucion, idTipoResolucion, NumeroResolucion, FechaResolucion, NULL
FROM RRHH_Nuevo.dbo.Resolucion;

-- columnas-resolucion de Empleado (grupo repetido) -> filas de Resolucion
WITH Nuevas AS (
	SELECT e.IdEmpleado, @TipoResIngresoEstado AS idTipoResolucion,
	       NULLIF(LTRIM(RTRIM(e.NumResoIngreEsta)), '') AS Numero,
	       CAST(e.FechaResolIngreEsta AS date) AS FechaResolucion, CAST(e.FechaIngreEstado AS date) AS FechaVigencia
	FROM RRHH_Nuevo.dbo.Empleado e
	WHERE NULLIF(LTRIM(RTRIM(e.NumResoIngreEsta)), '') IS NOT NULL OR e.FechaResolIngreEsta IS NOT NULL OR e.FechaIngreEstado IS NOT NULL
	UNION ALL
	SELECT e.IdEmpleado, @TipoResIngresoInstitucion,
	       NULLIF(LTRIM(RTRIM(e.NumResoIngreInstitu)), ''),
	       CAST(e.FechaResoIngreInstitu AS date), CAST(e.FechaIngreso AS date)
	FROM RRHH_Nuevo.dbo.Empleado e
	WHERE NULLIF(LTRIM(RTRIM(e.NumResoIngreInstitu)), '') IS NOT NULL OR e.FechaResoIngreInstitu IS NOT NULL
	-- NumResolCese / NumResol20530 / NumResolPenMen (y sus fechas) estaban vacias en todas las filas
)
INSERT dbo.Resolucion (idEmpleado, idResolucion, idTipoResolucion, NumeroResolucion, FechaResolucion, FechaVigencia)
SELECT n.IdEmpleado,
       ISNULL((SELECT MAX(r.idResolucion) FROM dbo.Resolucion r WHERE r.idEmpleado = n.IdEmpleado), 0)
         + ROW_NUMBER() OVER (PARTITION BY n.IdEmpleado ORDER BY n.idTipoResolucion),
       n.idTipoResolucion, n.Numero, n.FechaResolucion, n.FechaVigencia
FROM Nuevas n;

INSERT mig.Incidencia (Tabla, Accion, Detalle)
SELECT 'Empleado', 'MOVIDO',
       CAST(COUNT(*) AS varchar(10)) + ' resoluciones (ingreso al Estado / ingreso a la institucion) pasaron de columnas de Empleado a filas de Resolucion'
FROM dbo.Resolucion WHERE idTipoResolucion IN (@TipoResIngresoEstado, @TipoResIngresoInstitucion);

SET IDENTITY_INSERT dbo.Comportamiento ON;
INSERT dbo.Comportamiento (idComportamiento, IdEmpleado, idTipoComportamiento, Fecha, NumResol, Titulo, Descripcion)
SELECT idComportamiento, IdEmpleado, idTipoComportamiento, CAST(Fecha AS date), NumResol, Titulo, Descripcion
FROM RRHH_Nuevo.dbo.Comportamiento;
SET IDENTITY_INSERT dbo.Comportamiento OFF;

/* ============================================================
   7. FORMACION Y EXPERIENCIA
   ============================================================ */
INSERT dbo.EstudiosRealizado (IdEmpleado, IdTipoEstudios, Correlativo, CentroEstudios, Especialidad, FechaInicio, FechaTermino, IdEstadoEstudio)
SELECT IdEmpleado, IdTipoEstudios, Correlativo, CentroEstudios, Especialidad,
       CAST([AñoInicio] AS date), CAST([AñoTermino] AS date), IdEstadoEstudio
FROM RRHH_Nuevo.dbo.EstudiosRealizado;

INSERT dbo.TitulosEmpleado (IdEmpleado, NumColegiatura, DenominacionGrado, Institucion, Fecha)
SELECT IdEmpleado, NumColegiatura, DenominacionGrado, Institucion, CAST(Fecha AS date)
FROM RRHH_Nuevo.dbo.TitulosEmpleado
ORDER BY IdEmpleado, NumColegiatura;

SET IDENTITY_INSERT dbo.Capacitacion ON;
INSERT dbo.Capacitacion (IdCapacitacion, IdEmpleado, NombreCurso, InstitucionOrganizadora, FechaInicio, FechaFin, NumHoras)
SELECT IdCapacitacion, IdEmpleado, NombreCurso, InstitucionOrganizadora, CAST(FechaInicio AS date), CAST(FechaFin AS date),
       TRY_CONVERT(smallint, NULLIF(LTRIM(RTRIM(NumHoras)), ''))
FROM RRHH_Nuevo.dbo.Capacitacion;
SET IDENTITY_INSERT dbo.Capacitacion OFF;

INSERT dbo.ExpLaboral (IdEmpleado, IdExpLab, NomInstitucion, IdTipoInstitucion, UnidadOrganica, Cargo, FechaIngreso, FechaEgreso)
SELECT IdEmpleado, IdExpLab, NomInstitucion, IdTipoInstitucion, UnidadOrganica, Cargo, CAST(FechaIngreso AS date), CAST(FechaEgreso AS date)
FROM RRHH_Nuevo.dbo.ExpLaboral;

/* ============================================================
   8. FAMILIA
   ============================================================ */
INSERT dbo.Familiar (IdEmpleado, IdPersona, IdTipoFam, ViveCasa)
SELECT IdEmpleado, IdPersona, IdTipoFam, ViveCasa FROM RRHH_Nuevo.dbo.Familiar;

INSERT mig.Incidencia (Tabla, Clave, Campo, ValorOriginal, Accion, Detalle)
SELECT 'Familiar', CAST(f.IdEmpleado AS varchar(10)) + '-' + CAST(f.IdPersona AS varchar(10)), 'LaboraInstitucion',
       CAST(f.LaboraInstitucion AS nvarchar(5)), 'DERIVADO',
       'El valor guardado no coincide con el calculado (vinculo laboral activo). Ahora se calcula en vw_FamiliarDetalle'
FROM RRHH_Nuevo.dbo.Familiar f
JOIN dbo.vw_FamiliarDetalle v ON v.IdEmpleado = f.IdEmpleado AND v.IdPersona = f.IdPersona
WHERE ISNULL(f.LaboraInstitucion, 0) <> v.LaboraInstitucion;

INSERT dbo.RRHH_Asegurado (idEmpleado, NroBeneficiario, idPersona, IdVinculoAsegurado, NroPartida, NroResolMayorIndiscap,
                           FechaInicioVinculo, FechaFinVinculo, idMotivoBaja, NroPartidaDef, OtrosMotivos, Reevaluar)
SELECT a.idEmpleado, a.NroBeneficiario, a.idPersona, v.IdVinculoAsegurado, a.NroPartida, a.NroResolMayorIndiscap,
       CAST(a.FechaInicioVinculo AS date), CAST(a.FechaFinVinculo AS date), a.idMotivoBaja, a.NroPartidaDef, a.OtrosMotivos,
       NULLIF(LTRIM(RTRIM(a.Reevaluar)), '')
FROM RRHH_Nuevo.dbo.RRHH_Asegurado a
LEFT JOIN dbo.VinculoAsegurado v ON v.Descripcion = LTRIM(RTRIM(a.VinculoFamiliar));

/* ============================================================
   9. FICHA SOCIAL
   ============================================================ */
SET IDENTITY_INSERT dbo.Vivienda ON;
INSERT dbo.Vivienda (IdVivienda, IdEmpleado, IdTenencia, IdTipo, IdMaterial, IdConservacion, IdUbicacion,
                     IdAlumbrado, OtroAlumbrado, IdAgua, OtroAgua, IdExcretas, OtroExcreta,
                     NumDormitorio, NumServHig, NumCompart, NumPerDorm, Observacion)
SELECT IdVivienda, IdEmpleado, IdTenencia, IdTipo, IdMaterial, IdConservacion, IdUbicacion,
       IdAlumbrado, OtroAlumbrado, IdAgua, OtroAgua, IdExcretas, OtroExcreta,
       TRY_CONVERT(tinyint, NULLIF(LTRIM(RTRIM(NumDormitorio)), '')), TRY_CONVERT(tinyint, NULLIF(LTRIM(RTRIM(NumServHig)), '')),
       TRY_CONVERT(tinyint, NULLIF(LTRIM(RTRIM(NumCompart)), '')),    TRY_CONVERT(tinyint, NULLIF(LTRIM(RTRIM(NumPerDorm)), '')),
       Observacion
FROM RRHH_Nuevo.dbo.Vivienda;
SET IDENTITY_INSERT dbo.Vivienda OFF;

INSERT dbo.ViviendaServicio (IdVivienda, IdServicio)
SELECT v.IdVivienda, s.IdServicio
FROM RRHH_Nuevo.dbo.Vivienda v
CROSS APPLY (VALUES (1, v.Telefono), (2, v.Cable), (3, v.Internet), (4, v.Otros)) s(IdServicio, Tiene)
WHERE s.Tiene = 1;

SET IDENTITY_INSERT dbo.RRHH_Salud ON;
INSERT dbo.RRHH_Salud (idSalud, IdEmpleado, EnferCronica, EnferCronicaDescrip, FamDiscapacidad, FamDiscapDescrip, Alergico, AlergicoDescrip, idAcudeEnferm)
SELECT idSalud, IdEmpleado, EnferCronica, EnferCronicaDescrip, FamDiscapacidad, FamDiscapDescrip, Alergico, AlergicoDescrip, idAcudeEnferm
FROM RRHH_Nuevo.dbo.RRHH_Salud;
SET IDENTITY_INSERT dbo.RRHH_Salud OFF;

SET IDENTITY_INSERT dbo.RRHH_AspSocio ON;
INSERT dbo.RRHH_AspSocio (idAspSocio, IdEmpleado, PrincipProblemas, PropMejoraInstitu, hobby, ActRecreaInstitu,
                          ObsFam, AspecSalud, AspecEducacion, AspecRecreacion, AspecPsico, AspecSocial, DiagnosSocial)
SELECT s.idAspSocio, s.IdEmpleado, s.PrincipProblemas, s.PropMejoraInstitu, s.hobby, s.ActRecreaInstitu,
       NULLIF(LTRIM(RTRIM(e.ObsFam)), ''),         NULLIF(LTRIM(RTRIM(e.AspecSalud)), ''),
       NULLIF(LTRIM(RTRIM(e.AspecEducacion)), ''), NULLIF(LTRIM(RTRIM(e.AspecRecreacion)), ''),
       NULLIF(LTRIM(RTRIM(e.AspecPsico)), ''),     NULLIF(LTRIM(RTRIM(e.AspecSocial)), ''),
       NULLIF(LTRIM(RTRIM(e.DiagnosSocial)), '')
FROM RRHH_Nuevo.dbo.RRHH_AspSocio s
JOIN RRHH_Nuevo.dbo.Empleado e ON e.IdEmpleado = s.IdEmpleado;
SET IDENTITY_INSERT dbo.RRHH_AspSocio OFF;

-- empleados con diagnostico social en Empleado pero sin ficha RRHH_AspSocio: se les crea la ficha
INSERT dbo.RRHH_AspSocio (IdEmpleado, ObsFam, AspecSalud, AspecEducacion, AspecRecreacion, AspecPsico, AspecSocial, DiagnosSocial)
SELECT e.IdEmpleado,
       NULLIF(LTRIM(RTRIM(e.ObsFam)), ''),         NULLIF(LTRIM(RTRIM(e.AspecSalud)), ''),
       NULLIF(LTRIM(RTRIM(e.AspecEducacion)), ''), NULLIF(LTRIM(RTRIM(e.AspecRecreacion)), ''),
       NULLIF(LTRIM(RTRIM(e.AspecPsico)), ''),     NULLIF(LTRIM(RTRIM(e.AspecSocial)), ''),
       NULLIF(LTRIM(RTRIM(e.DiagnosSocial)), '')
FROM RRHH_Nuevo.dbo.Empleado e
WHERE NOT EXISTS (SELECT 1 FROM dbo.RRHH_AspSocio s WHERE s.IdEmpleado = e.IdEmpleado)
  AND COALESCE(NULLIF(LTRIM(RTRIM(e.ObsFam)), ''), NULLIF(LTRIM(RTRIM(e.AspecSalud)), ''), NULLIF(LTRIM(RTRIM(e.AspecEducacion)), ''),
               NULLIF(LTRIM(RTRIM(e.AspecRecreacion)), ''), NULLIF(LTRIM(RTRIM(e.AspecPsico)), ''),
               NULLIF(LTRIM(RTRIM(e.AspecSocial)), ''), NULLIF(LTRIM(RTRIM(e.DiagnosSocial)), '')) IS NOT NULL;

INSERT mig.Incidencia (Tabla, Accion, Detalle)
SELECT 'RRHH_AspSocio', 'DESCARTADO',
       'idPregunta1/2/3: grupo repetido sin catalogo de preguntas; ' + CAST(COUNT(*) AS varchar(10)) + ' filas con algun valor (se perdio el dato)'
FROM RRHH_Nuevo.dbo.RRHH_AspSocio WHERE COALESCE(idPregunta1, idPregunta2, idPregunta3) IS NOT NULL;

SET IDENTITY_INSERT dbo.RRHH_DinamicaFamiliar ON;
INSERT dbo.RRHH_DinamicaFamiliar (idDinamica, IdEmpleado, idTipoFam, idRelaPareja, idRelaPH, idRelacHermano, Observacion)
SELECT idDinamica, IdEmpleado, idTipoFam, idRelaPareja, idRelaPH, idRelacHermano, Observacion
FROM RRHH_Nuevo.dbo.RRHH_DinamicaFamiliar;
SET IDENTITY_INSERT dbo.RRHH_DinamicaFamiliar OFF;

INSERT dbo.RRHH_FuncFamRespuesta (IdEmpleado, IdPregunta, Respuesta)
SELECT u.IdEmpleado, p.IdPregunta, LTRIM(RTRIM(u.Respuesta))
FROM RRHH_Nuevo.dbo.RRHH_FuncFam f
UNPIVOT (Respuesta FOR Codigo IN (Com1, Com2, Afec1, Afec2, Decis1, Decis2, Soc1, Soc2, Soc3)) u
JOIN dbo.RRHH_FuncFamPregunta p ON p.Codigo = u.Codigo
WHERE NULLIF(LTRIM(RTRIM(u.Respuesta)), '') IS NOT NULL;

/* ============================================================
   10. ASISTENCIA  (~650 mil filas)
   ============================================================ */
INSERT dbo.HorarioTemporal (idEmpleado, N, FechaInicio, FechaFin, HorarioAsignado, NroDocumento, Sisgedo, FechaRegistro)
SELECT idEmpleado, N, FechaInicio, FechaFin, HorarioAsignado, NroDocumento, Sisgedo, FechaRegistro
FROM RRHH_Nuevo.dbo.HorarioTemporal;

INSERT dbo.Asistencia (Fecha, IdEmpleado, idHorario, Hor_Ent, Alm_Sal, Alm_Ent, Hor_Sal, Flag_Ent, Flag_Sal, Estado, Usuario, PC)
SELECT CAST(Fecha AS date), IdEmpleado, idHorario,
       TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(Hor_Ent)), '')), TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(Alm_Sal)), '')),
       TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(Alm_Ent)), '')), TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(Hor_Sal)), '')),
       Flag_Ent, Flag_Sal, Estado, Usuario, PC
FROM RRHH_Nuevo.dbo.Asistencia;

-- Estado '1N' (valor compuesto, viola 1FN) -> NroMarcacion = 1, TipoMarcacion = 'N'
INSERT dbo.Marcacion (Fecha, IdEmpleado, NroMarcacion, TipoMarcacion, CodDispositivo, idHorario)
SELECT Fecha, IdEmpleado, CAST(LEFT(Estado, 1) AS tinyint), SUBSTRING(Estado, 2, 1),
       NULLIF(LTRIM(RTRIM(Lugar)), ''), idHorario
FROM RRHH_Nuevo.dbo.Marcacion;

-- MinTarde venia como texto 'hh:mm:ss' -> segundos enteros
INSERT dbo.RegAsisDiario (Fecha, IdEmpleado, MinNormales, SegundosTarde, Estado)
SELECT CAST(Fecha AS date), IdEmpleado,
       TRY_CONVERT(smallint, NULLIF(LTRIM(RTRIM(MinNormales)), '')),
       CASE WHEN MinTarde LIKE '%:%' THEN DATEDIFF(SECOND, CAST('00:00:00' AS time(0)), TRY_CONVERT(time(0), LTRIM(RTRIM(MinTarde))))
            ELSE TRY_CONVERT(int, NULLIF(LTRIM(RTRIM(MinTarde)), '')) * 60 END,
       NULLIF(LTRIM(RTRIM(Estado)), '')
FROM RRHH_Nuevo.dbo.RegAsisDiario;

INSERT dbo.Permiso (IdEmpleado, NPermiso, idMotivo, FechaInicio, FechaFin, Hora_Sal, Hora_Ret, Dia, Retorno,
                    Lugar, Referencia, Autorizacion, AutorizacionRRHH, Autorizado, Obs)
SELECT IdEmpleado, NPermiso, idMotivo, CAST(FechaInicio AS date), CAST(FechaFin AS date),
       TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(Hora_Sal)), '')), TRY_CONVERT(time(0), NULLIF(LTRIM(RTRIM(Hora_Ret)), '')),
       Dia, Retorno, Lugar, Referencia, Autorizacion, AutorizacionRRHH, Autorizado, Obs
FROM RRHH_Nuevo.dbo.Permiso;

/* ============================================================
   11. SEGURIDAD
   ============================================================ */
INSERT dbo.SistemaOpcion (IdSistemaOpcion, Descripcion)
SELECT IdSistemaOpcion, Descripcion FROM RRHH_Nuevo.dbo.SistemaOpcion;

INSERT mig.Incidencia (Tabla, Accion, Detalle)
SELECT 'SistemaOpcion', 'DESCARTADO',
       'Columnas ConClave/Clave eliminadas (claves en texto plano). Filas con clave: ' + CAST(COUNT(*) AS varchar(10))
FROM RRHH_Nuevo.dbo.SistemaOpcion WHERE NULLIF(Clave, '') IS NOT NULL;

SET IDENTITY_INSERT dbo.Rol ON;
INSERT dbo.Rol (IdRol, Descripcion, IdSistema) SELECT IdRol, Descripcion, IdSistema FROM RRHH_Nuevo.dbo.Rol;
SET IDENTITY_INSERT dbo.Rol OFF;

INSERT dbo.Rol_Acceso (IdRol, IdSistemaOpcion)
SELECT DISTINCT IdRol, IdSistemaOpcion FROM RRHH_Nuevo.dbo.Rol_Acceso;

SET IDENTITY_INSERT dbo.Usuario ON;
INSERT dbo.Usuario (IdUsuario, Login, Descripcion, PasswordHash, Estado, Fecha, IdEmpleado)
SELECT IdUsuario, LTRIM(RTRIM(Login)), Descripcion, PasswordHash, Estado, Fecha, IdEmpleado FROM RRHH_Nuevo.dbo.Usuario;
SET IDENTITY_INSERT dbo.Usuario OFF;

INSERT dbo.Usuario_Rol (IdUsuario, IdRol) SELECT IdUsuario, IdRol FROM RRHH_Nuevo.dbo.Usuario_Rol;

/* ============================================================
   12. TABLAS NO MIGRADAS (fuera del dominio RRHH)
   ============================================================ */
INSERT mig.Incidencia (Tabla, Accion, Detalle)
SELECT t.Tabla, 'NO MIGRADO', CAST(t.Filas AS varchar(10)) + ' filas. Tabla de logistica/planeamiento heredada de CMI; sigue en RRHH_Nuevo.'
FROM (VALUES
	('Familia',         (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Familia)),
	('UnidadMedida',    (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.UnidadMedida)),
	('TipoRecurso',     (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.TipoRecurso)),
	('TipoDoc',         (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.TipoDoc)),
	('TipoMovimiento',  (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.TipoMovimiento)),
	('TipoTransaccion', (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.TipoTransaccion)),
	('TipoMoneda',      (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.TipoMoneda)),
	('Inconveniente',   (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Inconveniente)),
	('Limitacion',      (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Limitacion)),
	('Institucion',     (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Institucion))
) t(Tabla, Filas);

COMMIT TRANSACTION;
PRINT 'Migracion completada.';
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
	PRINT 'ERROR en la migracion. No se guardo nada.';
	THROW;
END CATCH
GO

/* ============================================================
   13. VERIFICACION
   ============================================================ */
PRINT '--- Filas origen vs destino ---';
SELECT x.Tabla, x.Origen, x.Destino, CASE WHEN x.Origen = x.Destino THEN 'OK' ELSE 'REVISAR (ver NORMALIZACION.md)' END AS Resultado
FROM (VALUES
	('Persona (solo RRHH)', (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Persona p WHERE EXISTS (SELECT 1 FROM RRHH_Nuevo.dbo.Empleado e WHERE e.IdPersona = p.IdPersona) OR EXISTS (SELECT 1 FROM RRHH_Nuevo.dbo.Familiar f WHERE f.IdPersona = p.IdPersona) OR EXISTS (SELECT 1 FROM RRHH_Nuevo.dbo.RRHH_Asegurado a WHERE a.idPersona = p.IdPersona)), (SELECT COUNT(*) FROM dbo.Persona)),
	('Empleado',            (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Empleado),        (SELECT COUNT(*) FROM dbo.Empleado)),
	('Familiar',            (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Familiar),        (SELECT COUNT(*) FROM dbo.Familiar)),
	('EstructOrganiz',      (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.EstructOrganiz),  (SELECT COUNT(*) FROM dbo.EstructOrganiz)),
	('PeriodoLaboral',      (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.PeriodoLaboral),  (SELECT COUNT(*) FROM dbo.PeriodoLaboral)),
	('Asistencia',          (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Asistencia),      (SELECT COUNT(*) FROM dbo.Asistencia)),
	('Marcacion',           (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Marcacion),       (SELECT COUNT(*) FROM dbo.Marcacion)),
	('RegAsisDiario',       (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.RegAsisDiario),   (SELECT COUNT(*) FROM dbo.RegAsisDiario)),
	('Permiso',             (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Permiso),         (SELECT COUNT(*) FROM dbo.Permiso)),
	('HorarioTemporal',     (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.HorarioTemporal), (SELECT COUNT(*) FROM dbo.HorarioTemporal)),
	('Capacitacion',        (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Capacitacion),    (SELECT COUNT(*) FROM dbo.Capacitacion)),
	('EstudiosRealizado',   (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.EstudiosRealizado), (SELECT COUNT(*) FROM dbo.EstudiosRealizado)),
	('TitulosEmpleado',     (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.TitulosEmpleado), (SELECT COUNT(*) FROM dbo.TitulosEmpleado)),
	('ExpLaboral',          (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.ExpLaboral),      (SELECT COUNT(*) FROM dbo.ExpLaboral)),
	('Comportamiento',      (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Comportamiento),  (SELECT COUNT(*) FROM dbo.Comportamiento)),
	('Rotacion',            (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Rotacion),        (SELECT COUNT(*) FROM dbo.Rotacion)),
	('ResponsableXUO',      (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.ResponsableXUO),  (SELECT COUNT(*) FROM dbo.ResponsableXUO)),
	('Vivienda',            (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Vivienda),        (SELECT COUNT(*) FROM dbo.Vivienda)),
	('RRHH_Salud',          (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.RRHH_Salud),      (SELECT COUNT(*) FROM dbo.RRHH_Salud)),
	('RRHH_DinamicaFamiliar', (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.RRHH_DinamicaFamiliar), (SELECT COUNT(*) FROM dbo.RRHH_DinamicaFamiliar)),
	('RRHH_Asegurado',      (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.RRHH_Asegurado),  (SELECT COUNT(*) FROM dbo.RRHH_Asegurado)),
	('RRHH_FuncFam (respuestas no nulas)',
		(SELECT COUNT(*) FROM RRHH_Nuevo.dbo.RRHH_FuncFam f
		 UNPIVOT (Respuesta FOR Codigo IN (Com1, Com2, Afec1, Afec2, Decis1, Decis2, Soc1, Soc2, Soc3)) u
		 WHERE NULLIF(LTRIM(RTRIM(u.Respuesta)), '') IS NOT NULL),
		(SELECT COUNT(*) FROM dbo.RRHH_FuncFamRespuesta)),
	('Usuario',             (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Usuario),         (SELECT COUNT(*) FROM dbo.Usuario)),
	('Usuario_Rol',         (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Usuario_Rol),     (SELECT COUNT(*) FROM dbo.Usuario_Rol)),
	('Ubigeo (distritos validos)', (SELECT COUNT(*) FROM RRHH_Nuevo.dbo.Ubigeo WHERE LEN(RTRIM(IdUbigeo)) = 6 AND Distrito <> '00'), (SELECT COUNT(*) FROM dbo.Distrito))
) x(Tabla, Origen, Destino);

PRINT '--- FKs no confiables (debe salir vacio) ---';
SELECT OBJECT_NAME(parent_object_id) AS Tabla, name AS FK FROM sys.foreign_keys WHERE is_not_trusted = 1 OR is_disabled = 1;

PRINT '--- Resumen de incidencias de datos ---';
SELECT Tabla, Accion, COUNT(*) AS Registros FROM mig.Incidencia GROUP BY Tabla, Accion ORDER BY Tabla, Accion;
GO
SET NOEXEC OFF
GO
