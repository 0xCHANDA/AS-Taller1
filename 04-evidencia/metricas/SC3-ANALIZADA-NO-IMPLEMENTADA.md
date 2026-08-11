# SC-3 — Historia clínica por res (ANALIZADA, NO IMPLEMENTADA)

**Estado:** ANALIZADA — NO IMPLEMENTADA
**Fecha de análisis:** 2026-08-09 (baseline Fase 2)
**Evidencia base:** `01-diagnostico/Fase 2 — Los cambios que vienen.docx`, `04-evidencia/historico/ESTADO_ACTUAL_Y_PLAN_CIERRE.md:132`

## Resumen del cambio solicitado

Cada res debe tener una historia clínica con eventos sanitarios (vacunaciones, enfermedades, tratamientos). Implica:
- Nuevas reglas de vacunación con calendario, dosis y refuerzos.
- Validación de esquema completo antes de ciertas operaciones.
- Persistencia de eventos clínicos con formato nuevo.

## Alcance estimado (Fase 2)

| Métrica | Cantidad |
|---|---|
| Clases existentes a modificar | 5 |
| Archivos existentes a modificar | 8 |
| Clases nuevas | 1 |
| Archivos nuevos | 2 |

Clases afectadas: `Res`, `Vacuna`, `Hacienda`, `PersistenciaService`, `VacunaService` (existente). Nueva: `HistoriaClinica` o `EventoClinico`. Archivos nuevos: clase + archivo de datos `Clinica.txt`.

## Riesgos identificados

1. **Reglas vacunales (Alto).** El dominio actual tiene un modelo simple de vacunas (alta, aplicación, vencimiento). SC-3 agrega calendarios, refuerzos, dosis y validación de esquema completo. Sin especificación formal de reglas, el riesgo de implementar reglas incorrectas es alto.
2. **Formato de persistencia (Alto).** Nuevo archivo `Clinica.txt` con formato posicional; `PersistenciaService` ya concentra cinco formatos. Agregar un sexto sin separar responsabilidades agrava la deuda de SRP.
3. **Acoplamiento sanitario (Medio).** `Res` y `Hacienda` ya contienen lógica de vacunación. Agregar historia clínica sin extraer un módulo `SaludAnimal` aumenta la superficie de cambio de la clase central.

## Razón de no implementación

SC-1 fue seleccionada formalmente (`decisiones/SC1-SELECCION.md`) porque:
- El eje "producto vendible" es más simple de aislar y demostrar OCP.
- SC-3 introduce un eje de variación diferente (sanitario) con reglas de dominio complejas.
- La línea base de Fase 2 estimó 5 clases existentes a modificar; combinado con SC-1, el diff de OCP sería difícil de atribuir a un solo eje.
- `PersistenciaService` (deuda consciente) habría recibido presión de cambio en dos ejes simultáneos.

## Impacto en TO-BE

El TO-BE actual no incorpora historia clínica. Si se implementara SC-3, requeriría:
- `HistoriaClinica` o `EventoClinico` como clase de dominio.
- `IRegistroClinico` o `IPersistenciaClinica` como puerto nuevo.
- Validación de esquema de vacunación completo.
- Adaptador de persistencia con nuevo archivo `Clinica.txt`.
- Posible extracción de `SaludAnimalService` desde `Hacienda`/`VacunaService`.

## Nota para el evaluador

SC-3 fue analizada en Fase 2 con baseline congelada. La no implementación es una decisión de alcance, no una omisión. El análisis de Fase 2 cubre el "análisis de las otras SC en TO-BE" requerido por la rúbrica. Ver `04-evidencia/historico/ESTADO_ACTUAL_Y_PLAN_CIERRE.md:132` y `decisiones/SC1-SELECCION.md`.
