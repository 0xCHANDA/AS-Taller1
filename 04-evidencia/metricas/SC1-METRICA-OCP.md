# SC-1 — métrica antes y después

Solicitud implementada: vender productos derivados del ganado —lácteos, carne y piel—.

## Regla de conteo

Se compara exactamente la misma capacidad en OLD y NEW. Solo cuentan clases y archivos de producción. No se cuentan demo, caracterización, verificador, documentación ni archivos generados.

## Resultado

| Arquitectura | Clases existentes modificadas | Archivos existentes modificados | Clases nuevas | Archivos nuevos |
|---|---:|---:|---:|---:|
| OLD / AS-IS estimado | 4 | 6 | 0 | 0 |
| NEW / TO-BE implementado | 0 | 0 | 6 | 6 |

## OLD

Clases existentes que habría que modificar:

1. `Venta`: dejar de representar únicamente una res.
2. `Hacienda`: agregar la política para vender productos derivados.
3. `ResController`: exponer la operación que hoy concentra la venta real.
4. `PersistenciaService`: guardar y cargar los nuevos tipos vendidos.

Archivos existentes: los cuatro anteriores, `Views/Res/Index.cshtml` y `Views/Venta/Index.cshtml`.

El alcance mínimo de Fase 2 modela la categoría del derivado dentro de las clases actuales; por eso no estima clases nuevas. La cifra OLD es contrafactual porque el sistema original no se modificó.

## NEW

Clases y archivos agregados:

1. `Lacteo.cs`
2. `InventarioLacteos.cs`
3. `Carne.cs`
4. `InventarioCarnes.cs`
5. `Piel.cs`
6. `InventarioPieles.cs`

No fue necesario modificar `Hacienda.vender<T>`, `Venta`, `RegistroVenta`, `IInventarioVendible<T>`, `IInventario<T>`, `PersistenciaService` ni la vista de ventas para reconocer cada variante.

## Interpretación

OLD resolvería la capacidad reabriendo clases y vistas existentes. NEW introduce seis piezas de dominio, pero no modifica la política estable ni sus consumidores por cada tipo. En este caso, agregar código nuevo es preferible a distribuir condicionales de categoría sobre seis archivos existentes.

La conclusión es local al eje “tipo de producto vendible”. No se afirma que toda la aplicación cumpla OCP ni que agregar un nuevo tipo de vacuna sea igualmente aditivo.
