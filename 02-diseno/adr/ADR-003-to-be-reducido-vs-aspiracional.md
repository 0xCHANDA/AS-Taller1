# ADR-003 — UML fiel al código vs diseño aspiracional

**Estado:** ACEPTADO
**Fecha:** 2026-08-10
**Evidencia:** `02-diseno/diagramas/TO-BE.puml`, `02-diseno/diagramas/TO-BE.png` y `HaciendaNEW.Verification`.

### Contexto

El UML contractual debe describir la implementación real y no puede incluir clases inventadas ni omitir clases relevantes. El diseño implementado usa `Producto`, `IInventarioVendible<T>`, validadores por capacidad y cinco puertos de persistencia. Los hallazgos H-02 y H-04 muestran las dos concentraciones que el diagrama debe explicar: dominio en `Hacienda` y persistencia en `PersistenciaService`.

1. Coordinar una venta
2. Administrar el historial de ventas

| ID | Descripción | Evaluación |
|---|---|---|
| A | Diseñar una arquitectura ideal con comandos, repositorios y servicios que no existen en código | **Descartada.** Introduce tipos sin cliente ni evidencia. |
| B | Mostrar únicamente el vertical de SC-1 | **Descartada.** Omitiría controllers, servicios, eventos y reglas que siguen siendo parte del sistema. |
| C | Reemplazar los PNG aspiracionales por un `.puml` editable que describa exactamente el código implementado | **Elegida.** Correspondencia 1:1 verificable; sin tipos fantasma. |

Mantener dentro de `Hacienda` el registro de las ventas.

`List`

No afecta el comportamiento del programa, pero sí deja muchas responsabilidades a la clase de `Hacienda`.

- **Positivo:** Cumple correspondencia UML↔código 1:1 exigida por la rúbrica.
- **Positivo:** El profesor puede recorrer cada elemento entre UML y código.
- **Negativo:** Pierde la ambición arquitectónica de puertos/adapters/serializadores completos.
- **Negativo:** `PersistenciaService` sigue implementando cinco puertos en una clase; la segregación es solo a nivel de interfaz.

Crear un `RegistroVenta`, responsable de registrar ventas (o sea de guardarlas), así toda la responsabilidad de las ventas no queda en `Hacienda`.

### Decisión

Elegimos la opción 2.

- `02-diseno/diagramas/TO-BE.puml`: fuente editable con correspondencia 1:1.
- `02-diseno/diagramas/TO-BE.png`: render final del diseño.
- La lista de clases del PUML se compara con los tipos de producción y el render se regenera desde la misma fuente.
