# Gestión del cambio (Semana 9)

## Flujo

```
Solicitud ─► Análisis ─► Evaluación de impacto ─► Aprobación / rechazo
   ─► Planificación ─► Implementación (rama + PR) ─► Verificación (dotnet test + prueba funcional) ─► Registro y cierre
```

| Rol | Función |
|---|---|
| Solicitante (usuario de RR.HH. o un integrante) | Describe el cambio y su motivo |
| Responsable técnico | Analiza el cambio y evalúa su impacto |
| Comité de cambios (el equipo, con el docente como cliente) | Aprueba o rechaza |
| Desarrollador | Implementa el cambio en una rama |
| Revisor | Revisa el *pull request* antes de integrarlo |

## Formato de solicitud de cambio

Plantilla en GitHub: `.github/ISSUE_TEMPLATE/solicitud-de-cambio.md`.

| Campo | Contenido |
|---|---|
| Código | SC-NN |
| Solicitante / fecha | |
| Tipo | Correctivo · Adaptativo · Perfectivo · Preventivo |
| Descripción | Qué se pide y por qué |
| Módulos y capas afectadas | |
| Impacto | Alto · Medio · Bajo, con justificación |
| Decisión | Aprobado · Rechazado · Postergado |
| Verificación | Pruebas que demuestran el cambio |

## Matriz de cambios del proyecto

| Código | Tipo | Descripción | Capas afectadas | Impacto | Estado |
|---|---|---|---|---|---|
| SC-01 | Preventivo | Reestructurar las capas con contratos para que un cambio de BD solo toque `Capa Datos` | Todas | Alto | Implementado |
| SC-02 | Adaptativo | Base normalizada `RRHH_Normalizado` (FNBC) con migración desde `RRHH_Nuevo` | Scripts de BD | Alto | Scripts listos; la app aún usa `RRHH_Nuevo` |
| SC-03 | Correctivo | Alta de beneficiario en Familia/Salud guardaba siempre N.º 0 (el segundo fallaba) | Capa Datos | Bajo | Implementado |
| SC-04 | Correctivo | Formulario de Permiso daba error 500 (checkbox `bool?`) | Vistas | Bajo | Implementado |
| SC-05 | Perfectivo | Pantallas que cargan tablas completas en memoria (Marcaciones, Registro diario) | Lógica y Datos | Medio | Pendiente |
| SC-06 | Adaptativo | Conectar la aplicación a `RRHH_Normalizado` | Capa Datos | Medio | Pendiente (depende de SC-01 y SC-02) |

## Análisis de impacto

- **SC-01:** toca las 4 capas, pero sin cambiar el comportamiento observable.
  - *Mitigación:* las pruebas existentes siguen en verde y se agregaron `ArquitecturaTests`. Además se probó la aplicación sobre una copia de la BD.
- **SC-02:** no afecta a la aplicación en producción porque se crea una BD nueva.
  - *Riesgo:* datos inconsistentes del sistema legado.
  - *Mitigación:* cada corrección queda auditada en `mig.Incidencia`.
- **SC-03 y SC-04:** impacto local en un formulario cada uno; no afectan otros módulos.
- **SC-05:** mejora el rendimiento. Requiere agregar consultas filtradas en la BD (en el contrato del repositorio que lo necesite) y paginación.
- **SC-06:** gracias a SC-01, solo cambian `Capa Datos` y la cadena de conexión. El detalle está en `NORMALIZACION.md`, sección 9.
  - *Decisión previa:* qué hacer con `Institucion` e `Inconveniente`, que no existen en la BD nueva.

## Buenas prácticas del equipo

- Ningún cambio se integra sin solicitud registrada y *pull request* revisado.
- Un cambio por rama; los cambios grandes se dividen.
- `dotnet test` debe pasar antes de integrar.
- Al cerrar, se actualiza el estado en esta matriz.
