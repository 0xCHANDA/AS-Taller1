# ADR-002 — Modelo de SC-1: venta genérica con `Producto` + `IInventarioVendible<T>`

**Estado:** ACEPTADO (retrospectivo)
**Fecha:** 2026-08-10
**Evidencia:** `Hacienda.cs:138-169`, `Producto.cs:9-31`, `IInventarioVendible.cs:8-12`, `04-evidencia/metricas/SC1-METRICA-OCP.md`

## Contexto

SC-1 exige vender lácteos, carne, piel y potenciales variantes futuras. OLD no tiene un producto vendible común: `vender_res` opera sobre `Res` directamente. Existen dos caminos: agregar métodos de venta paralelos (`vender_lacteo`, `vender_piel`, etc.) o modelar un producto e inventario genérico que permita una sola política de venta.

## Alternativas

| ID | Descripción | Evaluación |
|---|---|---|
| A | Métodos paralelos por tipo (`vender_lacteo`, `vender_piel`, `vender_carne`) | **Descartada.** Cada nueva variante modifica `Hacienda` y `PersistenciaService`; viola OCP para el eje aprobado. |
| B | Jerarquía `IVendible` + `VentaProducto` separada de `VentaRes` | Requiere duplicar registro, persistencia y vistas. **Sobrecosto sin beneficio claro.** |
| C | `Producto` abstracto + `IInventarioVendible<T>` + venta genérica única | **Elegida.** Una política, muchas implementaciones; nueva variante = nuevo `Producto` + nuevo `IInventario<T>`. |

## Decisión

Alternativa C. Se define `Producto` como clase abstracta con `Nombre` validado. `IInventarioVendible<T>` expone `contiene(T)` y `retirar(T)`. `Hacienda.vender<T>(IInventarioVendible<T>, T, uint)` implementa la política única. La sobrecarga histórica `vender<T>(IInventario<T>, T, uint)` delega a la estrecha (`Hacienda.cs:138-142`).

## Consecuencias

- **Positivo:** Carne se añadió con 0 clases existentes modificadas, 2 nuevas (`Carne.cs`, `InventarioCarnes.cs`).
- **Positivo:** `IInventarioVendible<T>` separa consulta/retiro de agregado (ISP). Clientes de venta no dependen de `agregar`.
- **Negativo:** Requiere que cada variante implemente un `IInventario<T>` completo, aunque el patrón es repetitivo.
- **Trade-off:** La sobrecarga histórica con `IInventario<T>` se conserva para compatibilidad binaria; el cast a `IInventarioVendible<T>` es seguro para implementaciones conformes.

## Principios SOLID

- **OCP:** Política cerrada a modificación, abierta a extensión por nuevas implementaciones.
- **ISP:** `IInventarioVendible<T>` es estrictamente lo que el cliente de venta necesita.
- **SRP:** `RegistroVenta` separa el historial de la operación de venta.

## Verificación

- `HaciendaNEW.Verification` comprueba venta de Res, Lacteo, Piel, producto definido en el verifier y rechazo atómico.
- Demo: `HaciendaNEW.Demo` ejecuta 4 ventas (res + SC-1 lácteo/carne/piel).
- Métrica: `04-evidencia/metricas/SC1-METRICA-OCP.md`.
