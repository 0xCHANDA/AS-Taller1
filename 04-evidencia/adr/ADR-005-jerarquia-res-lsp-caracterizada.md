# ADR-005 — Jerarquía Res: deuda LSP caracterizada vs composición/estado

**Estado:** ACEPTADO (retrospectivo — con deuda explícita)
**Fecha:** 2026-08-10
**Evidencia:** `Res.cs:12-36`, `Ternero.cs:9-19`, `Cebon.cs:9-19`, `Novillo.cs:9-19`, `Verificacion-de-herencias-LSP.md`

## Contexto

OLD modela categorías etarias como subtipos de `Res`: `Ternero` (0-12 meses), `Cebon` (13-36), `Novillo` (37-48). El defecto original no era la herencia en sí, sino que `Res.Edad` tenía un setter público virtual cuyos overrides fortalecían precondiciones: una instancia de `Res` aceptaba cualquier edad, pero `Ternero.Edad.set` rechazaba >12. Esto rompe LSP: el subtipo no es sustituible donde se espera la base.

Adicionalmente, `Res : Producto` duplicaba `Nombre` sin invocar `base(nombre)`, creando divergencia de identidad.

## Alternativas

| ID | Descripción | Evaluación |
|---|---|---|
| A | Eliminar subtipos y usar composición: `Res` con propiedad `Categoria` (enum) + validación dinámica | **Descartada para esta entrega.** Cambia API pública, requiere migrar constructores, switches etarios y persistencia. Riesgo de regresión masiva sin cobertura de tests. |
| B | Mantener subtipos con setter virtual y documentar violación LSP como trade-off | **Descartada.** Viola LSP formal, evaluable en C3. Riesgo C3 ≤ 2. |
| C | Eliminar setter público de `Edad`; validar rango en constructor de cada subtipo antes de delegar a `base`; resolver identidad `Res : Producto` | **Elegida.** Corrige los defectos observables sin cambiar el modelo de dominio ni la API externa. |

## Decisión

Alternativa C con deuda declarada. Cambios concretos:

1. `Res.cs:23-27`: constructor invoca `base(nombre)`, eliminando `Nombre` duplicado.
2. `Res.cs:34`: `Edad` pasa a ser propiedad de solo lectura (`=> edad`).
3. `Ternero.cs:13-19`, `Cebon.cs:13-19`, `Novillo.cs:13-19`: cada constructor valida su invariante etaria y luego delega a `base(nombre, peso, edad)`.
4. Ningún método común de `Res` muta la edad.

Esto corrige el strengthening de precondiciones en operación polimórfica. No resuelve —ni pretende— el envejecimiento y cambio de categoría como requisito futuro.

## Consecuencias

- **Positivo:** LSP conforme para el contrato observado. `Edad` inmutable post-construcción garantiza que ningún cliente de `Res` reciba un rechazo inesperado.
- **Positivo:** Identidad `Res`/`Producto` unificada; la venta registra exactamente el objeto retornado por `retirar`.
- **Negativo (deuda):** Las categorías siguen siendo subtipos; cambiar de categoría (envejecimiento) no es posible sin recrear el objeto. Esto requeriría composición/estado si el dominio lo exigiera.
- **Negativo (deuda):** `Producto.Nombre` conserva setter público para no romper consumidores; su mutabilidad debe caracterizarse antes de restringirla.

## Principios SOLID

- **LSP:** Cerrado el defecto de fortalecimiento de precondiciones. La matriz de sustitución en `Verificacion-de-herencias-LSP.md` cubre las 7 relaciones.
- **OCP:** Los switches etarios en `Potrero.anadir_res` y persistencia no se tocaron; no hay presión de cambio demostrada para ese eje.

## Verificación

- `HaciendaNEW.Verification/Program.cs`: `VerificarProductoRes` (identidad), `VerificarContratoRes` (rangos e inmutabilidad).
- `Verificacion-de-herencias-LSP.md`: matriz completa de sustitución.
- Test-Guardian: **PASS**. Adversarial-Reviewer: `BLOCKER-API-001 CLOSED`, **0 blockers nuevos**.
