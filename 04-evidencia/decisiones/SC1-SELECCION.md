# Selección de la solicitud de cambio - Fase 4

## Solicitud seleccionada

SC-1 — Productos derivados del ganado.

La hacienda comenzará a vender productos derivados del ganado, incluyendo lácteos, carne y piel.

## Decisión del equipo

**SC-1 aceptada.**

SC-1 será la solicitud implementada para demostrar extensibilidad y OCP con una comparación antes/después.

## Justificación

| Solicitud | Costo observado en AS-IS | Razón de la decisión |
|---|---|---|
| SC-1 - Productos derivados | 4 clases y 6 archivos existentes | Tiene un eje de variación claro: agregar tipos de producto vendible. Permite comprobar si el rediseño crece agregando código nuevo sin modificar la política de venta. |
| SC-2 - Chips y geolocalización | 4 clases y 5 archivos existentes | El cambio mezcla identidad, persistencia y una posible integración tecnológica. Es válido, pero demuestra OCP de forma menos directa. |
| SC-3 - Historia clínica | 5 clases y 8 archivos existentes, más elementos nuevos | Tiene el mayor alcance y combina dominio, persistencia, presentación y arranque. Implementarla habría mezclado varias decisiones en una sola prueba. |

Se eligió SC-1 porque permite aislar mejor el costo de extensión y comparar OLD contra NEW con una métrica sencilla y defendible. SC-2 y SC-3 se analizaron en la Fase 2, pero no se implementaron.

## Restricción

La implementación debe preservar el comportamiento existente no relacionado con SC-1. No se incluyen cambios adicionales solo por estilo.

## Evidencia esperada

- Implementación y demostración funcional.
- Clases y archivos creados o modificados.
- Comparación del costo de cambio OLD contra NEW.
- Ejecución reproducible.
