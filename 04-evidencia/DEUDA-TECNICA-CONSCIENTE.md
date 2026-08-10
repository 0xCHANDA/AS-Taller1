# Deuda técnica consciente — Hacienda Phase 4

**Corte:** 2026-08-10
**Propósito:** Declarar explícitamente las limitaciones y compromisos de diseño que el equipo reconoce y acepta como trade-offs, no como omisiones accidentales.

## Deuda declarada (no es blocker de entrega)

### DEUDA-001: PersistenciaService concentra cinco formatos

| Campo | Valor |
|---|---|
| Ubicación | `p_mvcHacienda/Servicios/PersistenciaService.cs:12-642` |
| Naturaleza | SRP interno: una clase implementa parsing/escritura de Reses.txt, Potreros.txt, Vacunas.txt, Ventas.txt y Usuarios.txt |
| Mitigación actual | Los clientes dependen de cinco interfaces segregadas (`IPersistencia.cs`). Cambiar un formato no rompe contratos de clientes. |
| Condición para remediar | Presión de cambio demostrada en formato (ej: migrar de TXT a JSON) o nueva variante de persistencia (ej: base de datos para SC-2/SC-3). |
| Costo estimado de remediación | Medio: extraer un codec por agregado manteniendo los puertos existentes. No requiere cambio en servicios ni controladores. |
| Principio implicado | SRP (interno), DIP (dirección ya corregida). |

### DEUDA-002: Producto.Nombre tiene setter público

| Campo | Valor |
|---|---|
| Ubicación | `Producto.cs:18-30` |
| Naturaleza | Mutabilidad post-construcción del identificador de producto. Un consumidor podría cambiar el nombre después de que el producto esté en un inventario o venta. |
| Mitigación actual | `Res` ya no duplica `Nombre`; la identidad `Producto` es única por instancia. Los inventarios usan `Equals` por referencia. |
| Condición para remediar | Caracterizar consumidores externos del setter; si ninguno depende de mutación post-venta, restringir a `private set` o solo constructor. |
| Costo estimado de remediación | Bajo: `private set` + test de regresión. |
| Principio implicado | Encapsulamiento (no SOLID directo, pero afecta invariantes). |

### DEUDA-003: Categorías etarias como subtipos (no composición)

| Campo | Valor |
|---|---|
| Ubicación | `Res.cs` + `Ternero.cs`, `Cebon.cs`, `Novillo.cs` |
| Naturaleza | El envejecimiento y cambio de categoría no están modelados. Una res no puede "crecer" de Ternero a Cebon sin recrear el objeto. |
| Mitigación actual | `Edad` es inmutable post-construcción; LSP conforme para el contrato observado. Los switches etarios en `Potrero.anadir_res` y persistencia funcionan correctamente. |
| Condición para remediar | Requisito de dominio que exija envejecimiento/cambio de categoría sin perder identidad ni historial (ventas, vacunación). |
| Costo estimado de remediación | Alto: migrar de herencia a composición (`Categoria` como propiedad/estado) afecta constructores, switches, persistencia y API pública. |
| Principio implicado | LSP (actualmente conforme), OCP (switches etarios). |

### DEUDA-004: Excepciones envueltas genéricamente

| Campo | Valor |
|---|---|
| Ubicación | `Hacienda.cs:132-135`, `Potrero.cs`, `Res.cs` (patrón `catch(Exception) { throw new Exception(...) }`) |
| Naturaleza | Pérdida de tipo de excepción original; clientes no pueden capturar `ArgumentNullException` vs `InvalidOperationException` de dominio. |
| Mitigación actual | Los 11 casos de caracterización capturan el mensaje exacto, no el tipo. El comportamiento observable se preserva. |
| Condición para remediar | Cliente que necesite manejo diferenciado por tipo de error (ej: UI que muestre mensajes distintos según causa). |
| Costo estimado de remediación | Medio: reemplazar `throw new Exception` por excepciones específicas o `throw` sin wrap, preservando mensajes. |
| Principio implicado | LSP (contrato de excepciones). |

### DEUDA-005: Vacuna.EstaVencida usa DateTime.Now

| Campo | Valor |
|---|---|
| Ubicación | `Vacuna.cs` (implícito en `EstaVencida()`) |
| Naturaleza | Dependencia estática del reloj del sistema. No se puede testear con fechas controladas sin cambiar el sistema. |
| Mitigación actual | Sin presión de cambio demostrada (no hay requisito de simulación temporal). |
| Condición para remediar | Tests que requieran fechas deterministas o requisito de "time travel" para simular vencimientos. |
| Costo estimado de remediación | Bajo: introducir `IClock` con implementación default `SystemClock` y mock en tests. |
| Principio implicado | DIP (detalle de infraestructura en dominio). |

### DEUDA-006: Hidratación parcial de Hacienda en startup

| Campo | Valor |
|---|---|
| Ubicación | `Program.cs:80-116` |
| Naturaleza | El bloque `try/catch` que carga datos desde `PersistenciaService` puede devolver una `Hacienda` parcialmente cargada si falla a mitad. |
| Mitigación actual | Si falla la carga de potreros, no se cargan reses ni ventas; pero si falla después de potreros, quedan inconsistentes. |
| Condición para remediar | Requisito de atomicidad en startup o carga desde fuente externa no confiable. |
| Costo estimado de remediación | Medio: carga transaccional o factory method con rollback. |
| Principio implicado | Robustez (no SOLID directo). |

### DEUDA-007: Sin tests unitarios automatizados

| Campo | Valor |
|---|---|
| Naturaleza | El proyecto no tiene tests unitarios (solo verifier de integración y characterization runners). La verificación actual depende de ejecución manual. |
| Mitigación actual | 11 characterization cases ejecutados con wrapper seguro; verifier con comprobaciones focalizadas; build verde. |
| Condición para remediar | Cobertura automatizada para regresiones en refactors futuros. |
| Costo estimado de remediación | Medio-alto: proyecto de tests con referencias a Bib_Hacienda, mock/fake de persistencia. |
| Principio implicado | Verificación (transversal). |

## Deuda aceptada como trade-off consciente

| Ítem | Razón de aceptación |
|---|---|
| `Potrero.agregar` existe (heredado de `IInventario<T>`) aunque venta no lo usa | Compatibilidad de API; ISP resuelto vía `IInventarioVendible<T>` para el cliente de venta. |
| `PersistenciaService` no se dividió en codecs por agregado | Sin presión de cambio en formato; dividir sin necesidad viola YAGNI. |
| PNG aspiracionales conservados como historia | Evidencia de Fase 3; el `.puml` normativo los reemplaza. |
| Sin migración a net10 ni retarget automático | OLD es net472; NEW es net8; coherencia con host y SDK disponible. |
| Dos sobrecargas de `vender` (legacy + genérica) | Compatibilidad binaria; el cast a `IInventarioVendible<T>` es seguro. |

## Lo que NO es deuda (falsos positivos rechazados)

- **"Hacienda es grande"** → SRP no se mide por líneas; no hay presión de cambio de actores independientes demostrada.
- **"Switches etarios violan OCP"** → Sin presión repetida de modificación en ese eje.
- **"Crear interfaz por clase"** → Rechazado explícitamente; DIP no obliga a abstraer entidades.
- **"TXT es malo"** → El formato no es el problema; la dirección de dependencia sí lo era (ya corregida).
- **"Concretos en Program violan DIP"** → Program es composition root legítimo.
