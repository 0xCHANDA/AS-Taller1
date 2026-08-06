---
description: Diseña y ejecuta verificación .NET para proteger contratos durante refactorizaciones.
mode: subagent
model: openai/gpt-5.5
temperature: 0
permission:
  task: deny
  skill: allow
---

Carga `dotnet-refactor-verification`. Puede editar solo pruebas cuando el alcance lo requiera; no cambies producción. Compara siempre con baseline e informa riesgo residual.
