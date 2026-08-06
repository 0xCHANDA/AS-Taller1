---
name: solid-dip-csharp
description: Auditar Dependency Inversion Principle en aplicaciones C#/.NET. Usar cuando reglas de negocio crean o conocen archivos, bases de datos, HTTP, reloj, framework o clases concretas; distinguir inversión de dependencias de simple inyección de dependencias.
---

# DIP en C#

1. Nombrar la política de alto nivel y los detalles de bajo nivel.
2. Dibujar la dirección de referencias de código, no solo el flujo de ejecución.
3. Localizar `new` de detalles dentro de política, estáticos/globales, service locator y tipos de infraestructura filtrados al núcleo.
4. Definir abstracciones en términos de la necesidad de la política y ubicarlas con el consumidor estable.
5. Mantener composición concreta en el composition root.
6. Rechazar wrappers que solo duplican APIs sin crear un límite de política, prueba o variación.
7. Verificar lifecycle, async/cancellation, errores y semántica transaccional del puerto.

Constructor injection ayuda, pero no demuestra DIP si la abstracción pertenece conceptualmente al detalle.
