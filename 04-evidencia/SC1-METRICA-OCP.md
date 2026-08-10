# SC-1 — métrica empírica OCP

## Reglas de conteo

Se cuentan solo clases y archivos de producción. Se excluyen demo, characterization, verifier, evidencia, `bin`, `obj` y generados. La medición OLD conserva la baseline de Fase 2 auditada en `ESTADO_ACTUAL_Y_PLAN_CIERRE.md:128-134`; la medición NEW usa el slice real que completó la variante faltante Carne.

| Arquitectura | Clases existentes modificadas | Archivos existentes modificados | Clases nuevas | Archivos nuevos |
|---|---:|---:|---:|---:|
| OLD / AS-IS estimado para SC-1 | 4 | 6 | 0 | 0 |
| NEW / completar Carne sobre el punto de variación | 0 | 0 | 2 | 2 |

## OLD

La baseline 4/6 de Fase 2 corresponde a modificar la política y modelo centrados en res: `Hacienda`, `Venta`, `PersistenciaService` y el servicio de consulta de ventas; además alcanzaría el contrato histórico `IVentaRes.cs` y la vista `Views/Venta/Index.cshtml`. OLD no posee un producto vendible común ni un inventario abstraído. La cifra es contrafactual y condicionada al alcance mínimo documentado; no se editó OLD.

## NEW (resultado real)

Para incorporar la variante concreta faltante se agregaron únicamente:

1. `Clases/Carne.cs` — `Carne : Producto`.
2. `Clases/InventarioCarnes.cs` — implementación de `IInventario<Carne>`.

No se modificaron `Hacienda.vender<T>`, `Venta`, `RegistroVenta`, `IInventarioVendible<T>` ni persistencia para reconocer Carne. El formato V2 ya guarda el nombre de tipo y recarga tipos no conocidos como `ProductoPersistido`, conservando tipo original, nombre y monto sin un `if` nuevo por variante.

## Interpretación

El resultado apoya OCP en el eje aprobado “tipo de producto vendible”: la política estable permanece cerrada y la capacidad crece agregando implementaciones. No afirma que toda la aplicación sea cerrada a cualquier cambio. El costo de dos clases nuevas es dominio/inventario real, no inflación de métrica.
