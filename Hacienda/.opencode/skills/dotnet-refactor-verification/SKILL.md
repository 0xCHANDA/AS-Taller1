---
name: dotnet-refactor-verification
description: Verificar refactorizaciones y mejoras de código .NET/C#. Usar para establecer baseline, seleccionar comandos de restore/build/test/format, crear pruebas contractuales o de caracterización y detectar regresiones frente a fallos preexistentes.
---

# Verificación .NET

1. Detectar SDK, target frameworks, solution, proyectos de prueba y convenciones existentes.
2. Registrar baseline antes de editar; no culpar al refactor por fallos preexistentes.
3. Priorizar pruebas enfocadas del contrato cambiado y luego ampliar alcance.
4. Usar, cuando sean apropiados: `dotnet restore`, `dotnet build`, `dotnet test`, `dotnet format --verify-no-changes`.
5. Añadir pruebas de caracterización para efectos y formatos; pruebas contractuales para implementaciones intercambiables.
6. Comparar errores, warnings, resultados y conducta observable con baseline.
7. Informar comandos, resultados, checks omitidos, razón y riesgo residual.

No cambiar producción para hacer pasar una prueba que describe incorrectamente el contrato.
