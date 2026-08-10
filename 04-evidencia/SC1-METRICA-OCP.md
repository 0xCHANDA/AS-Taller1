# SC-1 — métrica empírica OCP (regenerada 2026-08-10)

## Reglas de conteo

Solo se cuentan clases y archivos de producción (excluye demo, characterization, verifier, evidencia, `bin`, `obj`, generados). La métrica se regenera contra el estado final del refactor 2026-08-10.

## Eje de variación medido

"Agregar un nuevo producto vendible" (lácteos, carne, piel y variantes). Una nueva variante hipotética "Lana" se modela así:

| Arquitectura | Clases existentes modificadas | Archivos existentes modificados | Clases nuevas | Archivos nuevos |
|---|---:|---:|---:|---:|
| OLD / AS-IS estimado para "Lana" | 4 | 6 | 0 | 0 |
| NEW / final tras refactor 2026-08-10 | 0 | 0 | 2 | 2 |

## OLD

La baseline 4/6 de Fase 2 corresponde a modificar la política y modelo centrados en res: `Hacienda`, `Venta`, `PersistenciaService` y el servicio de consulta de ventas; además alcanzaría el contrato histórico `IVentaRes.cs` y la vista `Views/Venta/Index.cshtml`. OLD no posee un producto vendible común ni un inventario abstraído. La cifra es contrafactual y condicionada al alcance mínimo documentado; no se editó OLD.

## NEW (estado final)

Para incorporar la variante "Lana" se agregarían únicamente:

1. `Clases/Lana.cs` — `Lana : Producto`.
2. `Clases/InventarioLanas.cs` — `IInventario<Lana>`.

No se modificarían `Hacienda.vender<T>`, `Venta`, `RegistroVenta`, `IInventarioVendible<T>`, `IInventario<T>`, `PersistenciaService` ni la vista `Venta/Index.cshtml` para reconocer Lana. El formato V2 guarda el nombre de tipo y recarga tipos no conocidos como `ProductoPersistido`, conservando tipo original, nombre y monto sin un `if` nuevo por variante.

## Refactor 2026-08-10 (impacto sobre SC-1)

El refactor añadió `FabricadorVacunas` y un nuevo constructor `Hacienda(RegistroVenta, FabricadorVacunas)` para DIP pedagógico. Estos cambios **no afectan el eje SC-1**: la política de venta genérica ya era independiente del tipo concreto de `Producto` y la nueva extracción solo mueve la lógica de creación de vacunas, no la de venta.

| Métrica SC-1 | Antes (2026-08-09) | Después (2026-08-10) |
|---|---|---|
| Clases existentes modificadas para añadir Carne | 0 | 0 |
| Clases nuevas para añadir Carne | 2 (Carne + InventarioCarnes) | 2 (Carne + InventarioCarnes) |
| Eje OCP sigue aislado en SC-1 | sí | sí |

## Eje NO cubierto (deuda consciente)

El segundo eje natural de variación — "agregar un nuevo tipo de vacuna" — sigue dependiendo de extender `ICreacionVacuna`, `FabricadorVacunas` y `Hacienda.crear_vacuna`. No se introdujo una `IVacunaFactory` con reflection porque solo existen dos tipos (`Bacteriana`, `Viva`); añadir la abstracción completa incrementaría la complejidad sin cliente ni variación real. Ver `BITACORA-IA.md` (entradas 20-22).

## Interpretación

El resultado apoya OCP en el eje aprobado "tipo de producto vendible": la política estable permanece cerrada y la capacidad crece agregando implementaciones. No afirma que toda la aplicación sea cerrada a cualquier cambio. El costo de dos clases nuevas es dominio/inventario real, no inflación de métrica.
