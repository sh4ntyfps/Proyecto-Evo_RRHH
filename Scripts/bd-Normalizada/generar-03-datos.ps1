param(
  [string]$Server = 'localhost',
  [string]$Db     = 'RRHH_Normalizado',
  [string]$Out    = 'D:\evo_shanty\Scripts\bd-Normalizada\03-datos-RRHH_Normalizado.sql',
  [int]$BatchSize = 1000
)
$ErrorActionPreference = 'Stop'
$inv = [System.Globalization.CultureInfo]::InvariantCulture
$cn = New-Object System.Data.SqlClient.SqlConnection "Server=$Server;Database=$Db;Integrated Security=True;TrustServerCertificate=True"
$cn.Open()

function Query($sql) {
  $cmd = $cn.CreateCommand(); $cmd.CommandText = $sql; $cmd.CommandTimeout = 0
  $dt = New-Object System.Data.DataTable
  $dt.Load($cmd.ExecuteReader()); return ,$dt
}

# --- Tablas y dependencias (FK) para ordenar padres antes que hijos ---
$tables = Query "SELECT t.object_id, s.name sch, t.name tbl, OBJECTPROPERTY(t.object_id,'TableHasIdentity') hasId
                 FROM sys.tables t JOIN sys.schemas s ON s.schema_id=t.schema_id ORDER BY s.name, t.name"
$deps   = Query "SELECT DISTINCT parent_object_id child, referenced_object_id parent FROM sys.foreign_keys WHERE parent_object_id<>referenced_object_id"

$pending = [System.Collections.Generic.List[object]]::new(); $tables.Rows | ForEach-Object { $pending.Add($_) }
$done = @{}; $ordered = @()
while ($pending.Count -gt 0) {
  $ready = @($pending | Where-Object { $id = $_.object_id; -not ($deps.Rows | Where-Object { $_.child -eq $id -and -not $done.ContainsKey($_.parent) }) })
  if ($ready.Count -eq 0) { throw 'Ciclo de FKs entre tablas' }
  foreach ($r in $ready) { $ordered += $r; $done[$r.object_id] = $true; [void]$pending.Remove($r) }
}

function Lit($v, [string]$type) {
  if ($v -is [System.DBNull]) { return 'NULL' }
  switch ($type) {
    { $_ -in 'char','varchar','nchar','nvarchar','text','ntext' } { return "N'" + $v.Replace("'", "''") + "'" }
    'bit'       { return $(if ($v) { '1' } else { '0' }) }
    'date'      { return "CAST(N'" + $v.ToString('yyyy-MM-dd', $inv) + "' AS Date)" }
    'datetime'  { return "CAST(N'" + $v.ToString('yyyy-MM-ddTHH:mm:ss.fff', $inv) + "' AS DateTime)" }
    'datetime2' { return "CAST(N'" + $v.ToString('yyyy-MM-ddTHH:mm:ss.fffffff', $inv) + "' AS DateTime2)" }
    'time'      { return "CAST(N'" + $v.ToString('hh\:mm\:ss\.fffffff', $inv) + "' AS Time)" }
    'varbinary' { return '0x' + [System.BitConverter]::ToString($v).Replace('-', '') }
    default     { return [System.Convert]::ToString($v, $inv) }
  }
}

$enc = New-Object System.Text.UTF8Encoding $true
$w = New-Object System.IO.StreamWriter($Out, $false, $enc)
$w.NewLine = "`r`n"
$counts = @()

$w.WriteLine(@"
/* ============================================================
   CARGA DE DATOS - BD RRHH_Normalizado
   Equivalente normalizado de Scripts\bd-Datos\bd-Datos.sql.
   Generado desde RRHH_Normalizado ya migrada (02-migracion), por
   lo que NO necesita RRHH_Nuevo.

   Orden:  01-esquema-RRHH_Normalizado.sql  ->  este script
   - Tablas en orden de dependencia (padres antes que hijos).
   - IDENTITY_INSERT por tabla: se conservan los Id originales
     y el IDENTITY queda posicionado en el maximo cargado.
   - FKs deshabilitadas durante la carga y re-habilitadas al
     final WITH CHECK (quedan confiables).
   - Al final se verifican los conteos esperados.
   ============================================================ */
USE [$Db]
GO
SET NOCOUNT ON
SET XACT_ABORT ON
SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO
IF EXISTS (SELECT 1 FROM sys.partitions p JOIN sys.tables t ON t.object_id = p.object_id
           WHERE p.index_id IN (0,1) AND p.rows > 0)
BEGIN
    RAISERROR('La BD $Db ya tiene datos. Ejecute primero 01-esquema-RRHH_Normalizado.sql para recrearla vacia.', 16, 1)
    SET NOEXEC ON
END
GO
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'
GO
"@)

foreach ($t in $ordered) {
  $full = "[$($t.sch)].[$($t.tbl)]"
  $cols = Query "SELECT c.name, TYPE_NAME(c.system_type_id) ty FROM sys.columns c
                 WHERE c.object_id = $($t.object_id) AND c.is_computed = 0 ORDER BY c.column_id"
  $colList = ($cols.Rows | ForEach-Object { "[$($_.name)]" }) -join ', '
  $types = @($cols.Rows | ForEach-Object { $_.ty })
  $pk = Query "SELECT c.name FROM sys.indexes i JOIN sys.index_columns ic ON ic.object_id=i.object_id AND ic.index_id=i.index_id
               JOIN sys.columns c ON c.object_id=ic.object_id AND c.column_id=ic.column_id
               WHERE i.object_id=$($t.object_id) AND i.is_primary_key=1 ORDER BY ic.key_ordinal"
  $orderBy = if ($pk.Rows.Count) { ' ORDER BY ' + (($pk.Rows | ForEach-Object { "[$($_.name)]" }) -join ', ') } else { '' }

  $cmd = $cn.CreateCommand(); $cmd.CommandTimeout = 0
  $cmd.CommandText = "SELECT $colList FROM $full$orderBy"
  $rd = $cmd.ExecuteReader()
  $n = 0; $prefix = "INSERT $full ($colList) VALUES ("
  $vals = New-Object string[] $types.Count
  while ($rd.Read()) {
    if ($n -eq 0) {
      $w.WriteLine("-- $full")
      if ($t.hasId) { $w.WriteLine("SET IDENTITY_INSERT $full ON") }
    }
    for ($i = 0; $i -lt $types.Count; $i++) { $vals[$i] = Lit $rd.GetValue($i) $types[$i] }
    $w.WriteLine($prefix + ($vals -join ', ') + ')')
    $n++
    if ($n % $BatchSize -eq 0) { $w.WriteLine('GO') }
  }
  $rd.Close()
  if ($n -gt 0) {
    if ($n % $BatchSize -ne 0) { $w.WriteLine('GO') }
    if ($t.hasId) { $w.WriteLine("SET IDENTITY_INSERT $full OFF"); $w.WriteLine('GO') }
  }
  $counts += [pscustomobject]@{ Tabla = $full; Filas = $n }
  Write-Host ("{0,-40} {1,8}" -f $full, $n)
}

$w.WriteLine(@"
/* ---------- Re-habilitar FKs/CHECKs validando los datos ---------- */
EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'
GO
/* ---------- Verificacion ---------- */
DECLARE @v TABLE (Tabla sysname, Esperado int)
"@)
$vals = $counts | ForEach-Object { "('$($_.Tabla)', $($_.Filas))" }
for ($i = 0; $i -lt $vals.Count; $i += 50) {
  $w.WriteLine('INSERT @v VALUES ' + (($vals[$i..([Math]::Min($i + 49, $vals.Count - 1))]) -join ",`r`n  "))
}
$w.WriteLine(@"
SELECT v.Tabla, v.Esperado, ISNULL(SUM(p.rows), 0) AS Cargado,
       CASE WHEN v.Esperado = ISNULL(SUM(p.rows), 0) THEN 'OK' ELSE 'DIFERENCIA' END AS Resultado
FROM @v v
LEFT JOIN sys.partitions p ON p.object_id = OBJECT_ID(v.Tabla) AND p.index_id IN (0,1)
GROUP BY v.Tabla, v.Esperado
ORDER BY CASE WHEN v.Esperado = ISNULL(SUM(p.rows), 0) THEN 1 ELSE 0 END, v.Tabla

SELECT COUNT(*) AS FKs_no_confiables FROM sys.foreign_keys WHERE is_not_trusted = 1
GO
SET NOEXEC OFF
GO
"@)
$w.Close(); $cn.Close()
"Tablas: $($counts.Count)  Filas: $(($counts | Measure-Object Filas -Sum).Sum)"
