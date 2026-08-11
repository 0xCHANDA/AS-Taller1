# Diseño TO-BE — HaciendaNEW

Este documento explica la arquitectura que está implementada en `03-src/redisenado/HaciendaNEW`. El diagrama editable es `diagramas/TO-BE.puml` y su render vigente es `diagramas/TO-BE.png`.

## 1. Alcance del rediseño

Se implementó la SC-1: vender productos derivados del ganado —lácteos, carne y piel— sin crear un método de venta por cada producto. También se hicieron cambios internos para separar responsabilidades, dividir contratos de validación y desacoplar los servicios de la persistencia.

No se implementaron SC-2 ni SC-3. Tampoco se reescribieron autenticación, concurrencia o persistencia transaccional; quedan como deuda porque no hacen parte de la solicitud elegida.

## 2. Convención del UML

| Color | Significado |
|---|---|
| Blanco | Diseño conservado |
| Verde | OCP y punto de extensión de productos |
| Naranja | SRP |
| Azul | ISP |
| Rosado | DIP |
| Morado | Jerarquía revisada frente a LSP |
| Gris | Infraestructura y composition root |
| Amarillo | Elemento nuevo de la SC-1 |

El UML incluye dominio, contratos, reglas, eventos, persistencia, servicios, controllers, interceptores, modelos y composition root. Los métodos repetitivos de los controllers aparecen resumidos con `...`; sus dependencias y operaciones principales sí están representadas.

## 3. SRP — responsabilidades separadas

### Registro de ventas

`Hacienda` ya no guarda directamente la lista como responsabilidad propia. `RegistroVenta` registra las ventas y expone una vista de solo lectura. `Hacienda.L_ventas` conserva la lista viva que ofrecía OLD para no modificar el contrato público existente.

Beneficio: la política de venta coordina la operación y el registro mantiene el historial. Costo aceptado: la fachada conserva un acceso mutable por compatibilidad.

### Creación de vacunas

Los cuatro métodos públicos de `Hacienda.crear_vacuna` se mantienen, pero delegan en `FabricadorVacunas`. La frontera se eligió porque las validaciones de nombre, lote, fechas y cantidad cambian por una razón distinta a vender o alimentar reses.

Beneficio: `Hacienda` deja de contener la implementación repetida de creación. Costo aceptado: `FabricadorVacunas` aún conoce `Bacteriana` y `Viva`; con solo dos tipos no se justificó una fábrica genérica ni otra interfaz.

## 4. OCP — eje de productos vendibles

El eje de variación demostrado es “agregar un tipo de producto vendible”.

- `Producto` representa la identidad común.
- `IInventarioVendible<T>` ofrece únicamente `contiene` y `retirar`.
- `IInventario<T>` añade `agregar`.
- `Hacienda.vender<T>` contiene una sola política de venta.
- `Lacteo`, `Carne` y `Piel` se agregan como productos concretos con sus inventarios.

Una variante adicional, por ejemplo `Lana`, necesita `Lana : Producto` e `InventarioLanas : IInventario<Lana>`. No obliga a modificar `Hacienda.vender<T>`, `Venta`, `RegistroVenta` o persistencia.

OCP no se afirma para todo el sistema. Agregar un tercer tipo de vacuna todavía requiere modificar `ICreacionVacuna`, `FabricadorVacunas` y las sobrecargas de `Hacienda`.

## 5. LSP — jerarquías

### Producto

| Relación | ES-UN | Contrato y sustituibilidad |
|---|---|---|
| `Res : Producto` | Una res es un producto que puede venderse | Conserva `Nombre`; la venta genérica no necesita conocer su subtipo. |
| `Lacteo : Producto` | Un lácteo es un producto vendible | No añade precondiciones ni cambia resultados. |
| `Carne : Producto` | La carne es un producto vendible | No añade precondiciones ni cambia resultados. |
| `Piel : Producto` | La piel es un producto vendible | No añade precondiciones ni cambia resultados. |

Se usa herencia porque la política de venta necesita tratar todas las variantes como `Producto`. Componer un objeto sin comportamiento adicional no agregaría una frontera útil.

### Reses

| Relación | Rango | Postcondiciones e invariantes | Excepciones |
|---|---|---|---|
| `Ternero : Res` | 0–12 meses | Conserva nombre y peso; edad queda dentro del rango ternero. | Rechaza edad mayor a 12. |
| `Cebon : Res` | 13–48 meses | Conserva nombre y peso; edad queda dentro del rango cebón. | Rechaza edad menor a 13 o mayor a 48. |
| `Novillo : Res` | Mayor a 48 meses | Conserva nombre y peso; edad queda dentro del rango novillo. | Rechaza edad menor o igual a 48. |

`Res` define el contrato común: `Edad` debe pertenecer al rango de la categoría concreta. El setter público legacy se conserva. La propiedad ya no se sobreescribe con contratos diferentes; el setter base llama a `ValidarEdad`, implementado por cada categoría, tanto para construcción como para cambios posteriores. Esto mantiene la API y las reglas observables de OLD.

Se conserva herencia porque las categorías intervienen polimórficamente en vacunación, pesos y potreros. Si el negocio exige que una misma res envejezca y cambie de categoría sin recrearse, debería reemplazarse por composición/estado.

### Vacunas

| Relación | ES-UN | Precondiciones | Postcondiciones |
|---|---|---|---|
| `Bacteriana : Vacuna` | Es una vacuna con período | Período entre 2 y 4 | `Tipo=Bacteriana`; conserva lote y fechas. |
| `Viva : Vacuna` | Es una vacuna con atenuación | Atenuación 10, 20 o 30 | `Tipo=Viva`; conserva lote y fechas. |

La composición no reemplaza bien estas relaciones porque `Hacienda` y `Res` necesitan operar sobre cualquier `Vacuna` y consultar `Tipo`/`PuedeAplicarseA` con el mismo contrato.

## 6. ISP — contratos según clientes reales

OLD obligaba a cada validador a implementar cuatro operaciones y los métodos que no correspondían lanzaban `NotImplementedException`. NEW define:

- `IValidadorRes`, usado para reses;
- `IValidadorPotrero`, usado para potreros;
- `IValidadorVacuna`, usado para vacunas;
- `IValidadorVenta`, usado para ventas.

Los servicios de aplicación reciben solo el puerto de persistencia que necesitan:

| Cliente | Contrato usado |
|---|---|
| `PotreroService` | `IPersistenciaPotreros`, `IPersistenciaReses` |
| `ResService` | `IPersistenciaReses`, `IPersistenciaVentas` |
| `VacunaService` | `IPersistenciaVacunas`, `IPersistenciaPotreros`, `IPersistenciaReses` |
| `UsuarioService` | `IPersistenciaUsuarios` |

No se creó una interfaz por servicio ni `IFabricadorVacunas` porque no existe otro consumidor o implementación que justifique esos contratos.

## 7. DIP, DI y composition root

| Alto nivel | Bajo nivel | Abstracción | Implementación | Composición |
|---|---|---|---|---|
| `PotreroService` | archivos TXT | `IPersistenciaPotreros`, `IPersistenciaReses` | `PersistenciaService` | `p_mvcHacienda/Program.cs` |
| `ResService` | archivos TXT | `IPersistenciaReses`, `IPersistenciaVentas` | `PersistenciaService` | `Program.cs` |
| `VacunaService` | archivos TXT | `IPersistenciaVacunas`, `IPersistenciaPotreros`, `IPersistenciaReses` | `PersistenciaService` | `Program.cs` |
| `UsuarioService` | archivos TXT | `IPersistenciaUsuarios` | `PersistenciaService` | `Program.cs` |

La dirección es aplicación → contratos ← infraestructura. `Program.cs` registra la única instancia de `PersistenciaService` detrás de cinco puertos y construye los servicios.

`Hacienda(RegistroVenta, FabricadorVacunas)` es inyección por constructor, pero no DIP: ambos colaboradores son concretos. Se conservó también `Hacienda()` para compatibilidad con consumidores legacy.

## 8. Solicitudes futuras

| Solicitud | Estado del diseño | Cambios esperados |
|---|---|---|
| SC-1 productos derivados | Implementada | Nuevas variantes se agregan con producto + inventario. |
| SC-2 chip/geolocalización | Analizada, no preparada completamente | Afectaría `Res`, controller, persistencia y vista. |
| SC-3 historia clínica | Analizada, no preparada completamente | Requiere entidad clínica, relación con `Res`, persistencia, controller y vista. |

La arquitectura se diseñó principalmente para SC-1, porque fue la solicitud elegida. Los puertos de persistencia reducen parte del acoplamiento para SC-2/SC-3, pero no se afirma que esas solicitudes ya sean aditivas.

## 9. Decisiones arquitectónicas

Los ADR contienen la referencia mínima al problema que motivó cada decisión, como exige el enunciado:

- ADR-002: modelo genérico para SC-1 y política de venta.
- ADR-003: UML limitado al diseño realmente implementado.
- ADR-004: puertos de persistencia por cliente.
- ADR-005: contrato de edad y jerarquía de reses.
- ADR-006: extracción de creación de vacunas.

ADR-001 documenta únicamente la decisión técnica para poder compilar NEW y ejecutar OLD; no se presenta como una aplicación de SOLID.

## 10. Métrica SC-1

Se usa el mismo alcance en ambos lados: implementar lácteos, carne y piel.

| Arquitectura | Clases existentes modificadas | Archivos existentes modificados | Clases nuevas | Archivos nuevos |
|---|---:|---:|---:|---:|
| OLD estimado | 4 | 6 | 0 | 0 |
| NEW | 0 | 0 | 6 | 6 |

En OLD se habrían modificado `Venta`, `Hacienda`, `ResController`, `PersistenciaService` y las vistas `Res/Index` y `Venta/Index`, siguiendo el alcance mínimo definido en Fase 2 con una categoría dentro del modelo actual.

En NEW se agregaron `Lacteo`, `Carne`, `Piel`, `InventarioLacteos`, `InventarioCarnes` e `InventarioPieles`. El costo nuevo representa dominio real; la evidencia de OCP es que la política estable no se modifica por cada variante.

## 11. Costos y deuda aceptada

- La lista viva `Hacienda.L_ventas` y los setters de `Edad`/`L_vacunas_aplicadas` se conservan por compatibilidad con OLD.
- `PersistenciaService` sigue siendo una clase grande; la mejora está en la dirección de dependencias, no en dividir archivos por dividirlos.
- Autenticación y autorización no se corrigieron porque no pertenecen a SC-1. Siguen siendo deuda importante del hallazgo de seguridad.
- La creación de un tercer tipo de vacuna todavía modifica código existente.
- Las categorías etarias no soportan envejecimiento automático.

La implementación y la evidencia ejecutable se encuentran en `03-src` y `04-evidencia`.
