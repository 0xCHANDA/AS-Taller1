# ADR-005 — Contrato de edad en la jerarquía `Res`

**Estado:** ACEPTADO
**Fecha:** 2026-08-10
**Hallazgo relacionado:** H-05
**Evidencia:** `Res.cs`, `Ternero.cs`, `Cebon.cs`, `Novillo.cs` y casos C22 de caracterización.

## Contexto

OLD modela categorías etarias como subtipos: `Ternero` de 0 a 12 meses, `Cebon` de 13 a 48 y `Novillo` mayor a 48. Cada subtipo sobreescribe `Edad` y rechaza valores fuera de su rango, mientras la implementación base acepta cualquier `ushort`. Además, la API pública permite cambiar la edad después de construir el objeto.

El rediseño debe aclarar el contrato de la clase base sin retirar ese setter público, porque hacerlo rompería consumidores de OLD.

## Alternativas

| ID | Alternativa | Evaluación |
|---|---|---|
| A | Eliminar los subtipos y usar `Res` + `Categoria` | Modelo más flexible para envejecimiento, pero cambia constructores, persistencia, switches y API. Descartada para esta entrega. |
| B | Hacer `Edad` inmutable | Simplifica invariantes, pero elimina un setter público de OLD. Descartada por compatibilidad. |
| C | Mantener el setter en `Res` y definir un único contrato: la edad siempre debe pertenecer al rango de la categoría concreta | Elegida. Conserva API y reglas observables. |

## Decisión

`Res.Edad` conserva `get; set;`. La propiedad se implementa una sola vez en la base y llama al método protegido `ValidarEdad`. Cada categoría implementa únicamente su rango:

- `Ternero`: 0–12.
- `Cebon`: 13–48.
- `Novillo`: mayor a 48.

Los constructores validan el mismo rango antes de invocar el constructor base, evitando llamadas virtuales desde el constructor. `Res` también hereda `Nombre` de `Producto`, por lo que ya no mantiene una identidad duplicada.

## Consecuencias

- **Positiva:** el setter y las excepciones de rango observables se conservan.
- **Positiva:** ya no hay tres implementaciones públicas diferentes de la propiedad; el contrato está declarado en `Res`.
- **Positiva:** `Res` y `Producto` comparten una sola propiedad `Nombre`.
- **Negativa:** la validez de una edad sigue dependiendo de la categoría concreta.
- **Negativa:** una res no cambia automáticamente de subtipo al envejecer; ese requisito exigiría composición o State.

## Principios

- **LSP:** el contrato base declara explícitamente el invariante de categoría y todas las implementaciones lo mantienen. No se promete que cualquier edad sea válida para cualquier res.
- **OCP:** no se rediseñó el eje de categorías porque SC-1 se concentra en productos vendibles.

## Verificación

- `HaciendaNEW.Verification` comprueba los límites de las tres categorías, el setter público y que una asignación inválida no cambie el estado.
- C22 ejecuta el mismo setter válido e inválido contra OLD y NEW.
- C23 comprueba que `L_vacunas_aplicadas` también conserve su setter público legacy.
