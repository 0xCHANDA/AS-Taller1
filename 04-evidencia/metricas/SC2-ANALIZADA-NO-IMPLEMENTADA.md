# SC-2 — Chip de trazabilidad y geolocalización (ANALIZADA, NO IMPLEMENTADA)

**Estado:** ANALIZADA — NO IMPLEMENTADA
**Fecha de análisis:** 2026-08-09 (baseline Fase 2)
**Evidencia base:** `01-diagnostico/Fase 2 — Los cambios que vienen.docx`, `04-evidencia/historico/ESTADO_ACTUAL_Y_PLAN_CIERRE.md:131`

## Resumen del cambio solicitado

Cada res debe llevar un chip con trazabilidad geográfica. Implica:
- 226 registros de reses existentes deben incorporar identidad de chip.
- Compatibilidad con reses sin chip (históricas) y con chip (nuevas).
- La identidad del animal ya no depende solo del nombre; el chip es identificador único.

## Alcance estimado (Fase 2)

| Métrica | Cantidad |
|---|---|
| Clases existentes a modificar | 4 |
| Archivos existentes a modificar | 5 |
| Clases nuevas | 0 |
| Archivos nuevos | 0 |

Clases afectadas bajo el alcance mínimo declarado: `Res`, `Hacienda`, `PersistenciaService`, `VentaService`. El archivo de datos `Reses.txt` cambia de formato.

## Riesgos identificados

1. **Identidad y compatibilidad (Alto).** 226 registros OLD deben ser migrables o coexistir con el nuevo formato. Un cambio de identificador (nombre → chip) puede romper búsquedas, ventas, alimentación y vacunación.
2. **Persistencia (Alto).** `PersistenciaService` carga/escribe `Reses.txt` con formato posicional. Agregar un campo de chip requiere adaptar el parser y el writer sin romper la carga de datos existentes.
3. **API pública (Medio).** `vender_res`, `buscar_res` y métodos de `Potrero` usan `nombre` como clave de búsqueda. Agregar chip como clave alternativa implica sobrecargas o nuevo contrato.

## Razón de no implementación

SC-1 fue seleccionada formalmente (`decisiones/SC1-SELECCION.md`) por su relación directa con OCP en el eje "tipo de producto vendible". SC-2 no fue priorizada porque:
- Su costo de implementación (migración de 226 registros, doble formato de persistencia, cambio de identidad) supera el beneficio demostrativo para OCP.
- La variación de chip/geolocalización es un eje distinto (identidad + IoT) que no comparte abstracción con el eje SC-1.
- Implementar ambos SC habría requerido modificar `PersistenciaService` (ya en deuda consciente) en dos ejes simultáneos, diluyendo la métrica de OCP.

## Impacto en TO-BE

El TO-BE actual no incorpora chip ni geolocalización. Si se implementara SC-2, requeriría:
- Nueva clase `Chip` o propiedad en `Res`.
- Adaptador de persistencia con formato V3 (o doble formato V2/V3).
- Posible nuevo puerto `IPersistenciaChips` o extensión de `IPersistenciaReses`.
- Decisión de identidad: ¿`Chip` reemplaza `Nombre` como clave, o coexisten?

## Nota para el evaluador

SC-2 fue analizada en Fase 2 con baseline congelada. El hecho de que no esté implementada es una decisión consciente de alcance (SC-1), no una omisión accidental. Ver `04-evidencia/historico/ESTADO_ACTUAL_Y_PLAN_CIERRE.md:128-133` y `decisiones/SC1-SELECCION.md`.
