# ADR-003 — TO-BE reducido al vertical materializado vs UML aspiracional completo

**Estado:** ACEPTADO (retrospectivo)
**Fecha:** 2026-08-10
**Evidencia:** `TOBE-CODE-MATRIX.md`, `PHASE4-REPORT.md:54-55`, `TO-BE-FINAL.puml`

## Contexto

La Fase 3 produjo cuatro PNG (`fase3 uml 1..4.png`) que describían una arquitectura con `ItemVenta`, `IVendible`, `ProductoGanadero`, categoría, AppServices con interfaces, commands, `ResultadoAplicacion`, puertos/adapters/serializadores e `InicializadorHacienda`. Ninguno de estos tipos existe en el código fuente implementado.

La implementación real (`03-src/redisenado/HaciendaNEW`) sigue un modelo diferente: `Producto` + `IInventarioVendible<T>` + venta genérica; validadores por capacidad sin jerarquía ancha; persistencia con cinco puertos segregados implementados por un solo servicio. El UML aspiracional y el código implementado describen dos soluciones incompatibles para el mismo problema.

## Alternativas

| ID | Descripción | Evaluación |
|---|---|---|
| A | Implementar todas las cajas del UML aspiracional | **Descartada.** Decenas de tipos sin presión de cambio demostrada consumirían el plazo. Riesgo de quedar sin tests, SC funcional ni evidencia (escenario de fracaso #2 del plan de cierre). |
| B | Mantener los PNG como normativos y declarar el código como "parcial" | **Descartada.** Viola correspondencia UML↔código 1:1 (C2/C4 limitados a 3). |
| C | Reemplazar los PNG aspiracionales por un `.puml` editable que describa exactamente el código implementado | **Elegida.** Correspondencia 1:1 verificable; sin tipos fantasma. |

## Decisión

Alternativa C. `02-diseno/diagramas/TO-BE-FINAL.puml` reemplaza como fuente normativa a los PNG de Fase 3. Los PNG se conservan como evidencia histórica en `02-diseno/diagramas/` con banners de "SUPERSEDED". La matriz `TOBE-CODE-MATRIX.md` verifica bidireccionalmente que cada elemento del PUML tiene código y cada clase productiva tiene representación.

## Consecuencias

- **Positivo:** Cumple correspondencia UML↔código 1:1 exigida por la rúbrica.
- **Positivo:** Reduce el riesgo de inconsistencia documental (escenario de fracaso #3).
- **Negativo:** Pierde la ambición arquitectónica de puertos/adapters/serializadores completos.
- **Negativo:** `PersistenciaService` sigue implementando cinco puertos en una clase; la segregación es solo a nivel de interfaz.

## Principios SOLID

- **ISP:** Las cinco interfaces de persistencia corresponden a clientes reales observables; no se crearon repositorios por entidad.
- **DIP:** Dirección de dependencia verificada: dominio → contratos ← infraestructura.
- **YAGNI:** No se implementan abstracciones sin cliente/variación demostrada.

## Verificación

- `TOBE-CODE-MATRIX.md`: matriz bidireccional completa.
- `TO-BE-FINAL.puml`: fuente editable con correspondencia 1:1.
- `HaciendaNEW.Verification`: comprobaciones de puertos, validadores y composition root.
