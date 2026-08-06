---
name: solid-analysis-protocol
description: Ejecutar auditorías SOLID rigurosas sobre código C# existente. Usar antes de evaluar SRP, OCP, LSP, ISP o DIP, consolidar hallazgos o proponer refactorizaciones, especialmente cuando se requiere distinguir violaciones confirmadas de smells y preferencias.
---

# Protocolo de análisis SOLID C#

1. Delimitar solución, proyectos, símbolos y comportamiento observable.
2. Establecer baseline con compilación/pruebas existentes antes de atribuir fallos.
3. Separar evidencia, inferencia y desconocidos. No confirmar sin ubicación y contrato o presión de cambio.
4. Auditar cada principio independientemente y registrar contraevidencia.
5. Clasificar cada candidato como `confirmed`, `suspected`, `false-positive` o `accepted-trade-off`.
6. Proponer el cambio mínimo que reduzca el riesgo demostrado; no maximizar patrones ni cantidad de interfaces.
7. Exigir pruebas de caracterización para conducta sensible y slices reversibles.
8. Consolidar causas raíz compartidas para evitar cinco reportes del mismo problema.

Usar el esquema de `../../../docs/solid/finding-schema.md`, las barreras de `../../../docs/solid/anti-overengineering.md` y el marco académico sintetizado en `references/course-foundations.md`.

No editar código durante la auditoría.
