# EVIDENCE_BLOCKED — Gates 1 y 2

## Decisión

Se detuvo la recuperación sin crear resultados sintéticos. La evidencia no puede reproducirse con las restricciones simultáneas actuales y sin editar/agregar artefactos de ejecución en producción.

## Restricciones en conflicto

| Restricción | Evidencia |
|---|---|
| Harnesses/artefactos bajo `04-evidencia/` | Mandato del Phase 4 Evidence Engineer |
| No editar OLD ni NEW production | Solicitud de recuperación y mandato del rol |
| Build/run solo con wrapper seguro | Solicitud de recuperación |
| Wrapper acepta únicamente targets bajo `03-src/` | `scripts/phase4-safe-dotnet.sh:12-18` |
| Sandbox copia únicamente `03-src` | `scripts/phase4-safe-dotnet.sh:44-46` |

## Impacto

- Gate 1: FAIL; OLD no tiene ocho escenarios ejecutados.
- Gate 2: FAIL; sin baseline OLD no se permite afirmar equivalencia NEW.
- No hay MISMATCH clasificable porque no existe par OLD/NEW ejecutado.
- No hay evidencia para atribuir una regresión a Phase 3.

## Cambio de capacidad requerido (no ejecutado)

Se necesita autorización humana para una vía segura que ejecute proyectos de evidencia bajo `04-evidencia/` (por ejemplo, una acción dedicada del wrapper que copie `03-src` de solo lectura junto con el harness de evidencia). Esto es `PENDING_HUMAN_REVIEW`; no se atribuye como decisión del equipo.

No se solicita ni autoriza modificar comportamiento de producción.
