---
name: solid-ocp-csharp
description: Auditar Open/Closed Principle en C#. Usar cuando nuevas variantes obligan a editar repetidamente switches, condicionales, fábricas o servicios centrales; exigir una presión de extensión real antes de introducir polimorfismo, estrategia o composición.
---

# OCP en C#

1. Identificar el eje de variación y ejemplos actuales o previstos con evidencia.
2. Localizar módulos estables modificados cada vez que aparece una variante.
3. Distinguir corrección de bugs de extensión: OCP no impide corregir la clase original.
4. Rechazar abstracciones para ramas cerradas, requisitos únicos o variación especulativa.
5. Elegir el límite mínimo: función, composición, estrategia, registro de handlers o polimorfismo.
6. Verificar que agregar una variante requiera código nuevo localizado y no editar múltiples clientes.

Evaluar costo de descubrimiento, orden, DI y debugging. No declarar que todo `switch` viola OCP.
