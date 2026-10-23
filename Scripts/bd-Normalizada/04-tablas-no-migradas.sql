/*
  Copia a RRHH_Normalizado las 10 tablas que no se migraron (NORMALIZACION.md, seccion 6),
  para que sigan funcionando sus pantallas (Instituciones, Inconvenientes y los catalogos
  de logistica). Se copian tal cual desde RRHH_Nuevo, con su clave primaria.

  Requisitos: RRHH_Nuevo y RRHH_Normalizado en el mismo servidor.
  Se puede ejecutar varias veces: solo crea las tablas que faltan.
*/
USE [RRHH_Normalizado];
GO
SET NOCOUNT ON;

IF OBJECT_ID('dbo.Familia') IS NULL
BEGIN
    SELECT * INTO dbo.Familia FROM RRHH_Nuevo.dbo.Familia;
    ALTER TABLE dbo.Familia ADD CONSTRAINT PK_Familia PRIMARY KEY (IdFamilia, IdClase, IdGrupo);
END
IF OBJECT_ID('dbo.UnidadMedida') IS NULL
BEGIN
    SELECT * INTO dbo.UnidadMedida FROM RRHH_Nuevo.dbo.UnidadMedida;
    ALTER TABLE dbo.UnidadMedida ADD CONSTRAINT PK_UnidadMedida PRIMARY KEY (IdUnidadMedida);
END
IF OBJECT_ID('dbo.TipoRecurso') IS NULL
BEGIN
    SELECT * INTO dbo.TipoRecurso FROM RRHH_Nuevo.dbo.TipoRecurso;
    ALTER TABLE dbo.TipoRecurso ADD CONSTRAINT PK_TipoRecurso PRIMARY KEY (IdTipoRecurso);
END
IF OBJECT_ID('dbo.TipoDoc') IS NULL
BEGIN
    SELECT * INTO dbo.TipoDoc FROM RRHH_Nuevo.dbo.TipoDoc;
    ALTER TABLE dbo.TipoDoc ADD CONSTRAINT PK_TipoDoc PRIMARY KEY (IdTipodoc, Anio);
END
IF OBJECT_ID('dbo.TipoTransaccion') IS NULL
BEGIN
    SELECT * INTO dbo.TipoTransaccion FROM RRHH_Nuevo.dbo.TipoTransaccion;
    ALTER TABLE dbo.TipoTransaccion ADD CONSTRAINT PK_TipoTransaccion PRIMARY KEY (IdTipoTransaccion, Anio);
END
IF OBJECT_ID('dbo.TipoMovimiento') IS NULL
BEGIN
    SELECT * INTO dbo.TipoMovimiento FROM RRHH_Nuevo.dbo.TipoMovimiento;
    ALTER TABLE dbo.TipoMovimiento ADD CONSTRAINT PK_TipoMovimiento PRIMARY KEY (IdTipoMovimiento, IdTipodoc);
END
IF OBJECT_ID('dbo.TipoMoneda') IS NULL
BEGIN
    SELECT * INTO dbo.TipoMoneda FROM RRHH_Nuevo.dbo.TipoMoneda;
    ALTER TABLE dbo.TipoMoneda ADD CONSTRAINT PK_TipoMoneda PRIMARY KEY (IdTipoMoneda);
END
IF OBJECT_ID('dbo.Limitacion') IS NULL
BEGIN
    SELECT * INTO dbo.Limitacion FROM RRHH_Nuevo.dbo.Limitacion;
    ALTER TABLE dbo.Limitacion ADD CONSTRAINT PK_Limitacion PRIMARY KEY (IdLimitacion);
END
IF OBJECT_ID('dbo.Institucion') IS NULL
BEGIN
    SELECT * INTO dbo.Institucion FROM RRHH_Nuevo.dbo.Institucion;
    ALTER TABLE dbo.Institucion ADD CONSTRAINT PK_Institucion PRIMARY KEY (IdInstitucion);
END
IF OBJECT_ID('dbo.Inconveniente') IS NULL
BEGIN
    SELECT * INTO dbo.Inconveniente FROM RRHH_Nuevo.dbo.Inconveniente;
    ALTER TABLE dbo.Inconveniente ADD CONSTRAINT PK_Inconveniente PRIMARY KEY (IdObjetivo, Id_Actividad, IdInconveniente);
END
GO

SELECT t.name AS Tabla, SUM(p.rows) AS Filas
FROM sys.tables t JOIN sys.partitions p ON p.object_id = t.object_id AND p.index_id IN (0, 1)
WHERE t.name IN ('Familia','UnidadMedida','TipoRecurso','TipoDoc','TipoTransaccion',
                 'TipoMovimiento','TipoMoneda','Limitacion','Institucion','Inconveniente')
GROUP BY t.name ORDER BY t.name;
