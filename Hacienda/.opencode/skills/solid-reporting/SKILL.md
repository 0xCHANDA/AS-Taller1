---
name: solid-reporting
description: Consolidar auditorías SOLID C# en un reporte accionable. Usar para deduplicar hallazgos de varios auditores, priorizar causas raíz, registrar falsos positivos y producir una secuencia de refactorización verificable.
---

# Reporte SOLID

1. Normalizar cada hallazgo con `../../../docs/solid/finding-schema.md`.
2. Fusionar duplicados por causa raíz; elegir principio primario y efectos secundarios.
3. Mantener contraevidencia y falsos positivos para evitar reabrirlos sin datos nuevos.
4. Priorizar por impacto, probabilidad, alcance y costo de cambio; no por conteo de principios.
5. Convertir recomendaciones aprobadas en slices con archivos, conducta protegida, verificación y rollback.
6. Completar `../../../docs/solid/report-template.md` con baseline, incertidumbre y revisión adversarial.

No ocultar desacuerdos entre auditores: resolverlos con evidencia o marcarlos como sospechosos.
