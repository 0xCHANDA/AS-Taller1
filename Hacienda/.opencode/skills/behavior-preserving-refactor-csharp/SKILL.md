---
name: behavior-preserving-refactor-csharp
description: Diseñar y ejecutar refactorizaciones C# que preservan comportamiento. Usar después de aprobar hallazgos SOLID para dividir el cambio en slices reversibles, proteger contratos y evitar reescrituras o abstracciones especulativas.
---

# Refactorización C# que preserva conducta

1. Vincular cada cambio a un hallazgo confirmado y criterio de aceptación.
2. Capturar baseline y agregar caracterización del comportamiento sensible.
3. Ordenar slices: seam de pruebas → movimiento mecánico → inversión/extracción → limpieza.
4. Mantener cada slice compilable, revisable y reversible.
5. Preservar APIs, serialización, rutas, formatos, efectos, orden, excepciones y nulabilidad salvo autorización explícita.
6. Compilar/probar después de cada slice; comparar con baseline.
7. Detenerse si aparece drift semántico o la abstracción cuesta más que el cambio demostrado.

Solo un agente implementador debe escribir código de producción. Revisores y auditores permanecen en solo lectura.
