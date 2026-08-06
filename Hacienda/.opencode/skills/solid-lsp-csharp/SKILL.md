---
name: solid-lsp-csharp
description: Auditar Liskov Substitution Principle en jerarquías, interfaces e implementaciones C#. Usar ante excepciones NotSupported/NotImplemented, overrides especiales, downcasts, flags de tipo o clientes que no pueden tratar uniformemente los subtipos.
---

# LSP en C#

Para cada subtipo y cliente real:

1. Formular el contrato observable del tipo base.
2. Comprobar que no refuerce precondiciones ni debilite postcondiciones.
3. Verificar invariantes, efectos, nulabilidad, mutabilidad, excepciones y semántica de retorno.
4. Confirmar que parámetros sigan siendo compatibles y retornos covariantes cuando corresponda.
5. Buscar `NotImplementedException`, `NotSupportedException`, comprobaciones de tipo y métodos sin sentido para el subtipo.
6. Diseñar pruebas contractuales reutilizables que ejecuten todas las implementaciones.
7. Si la relación `es-un` no es conductualmente cierta, preferir composición o capacidades específicas.

La sintaxis de herencia no prueba una violación; mostrar el cliente y el comportamiento que se rompe.
