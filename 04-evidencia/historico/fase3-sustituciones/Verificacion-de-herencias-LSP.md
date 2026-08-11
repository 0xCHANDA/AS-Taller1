# Verificación de herencias y sustitución — Fase 3

**Alcance:** jerarquías y contratos implementados en `03-src/redisenado/HaciendaNEW/Bib_Hacienda`.  
**Corte:** 2026-08-10.  
**Resultado:** no quedan blockers confirmados en la revisión final focalizada. La conclusión se limita a los contratos observados y caracterizados; no afirma corrección universal.

## Criterios LSP aplicados

Para cada subtipo se revisaron: precondiciones, postcondiciones, invariantes, excepciones, estado observable y significado del comportamiento heredado. La sintaxis `:` por sí sola no se tomó como evidencia.

## Matriz de sustitución

| Relación | Contrato esperado | Evidencia | Evaluación |
|---|---|---|---|
| `Res : Producto` | Toda res debe tener un único nombre de producto válido y conservarlo al observarla como `Producto`. | `Producto.cs:13-29` valida y almacena `Nombre`; `Res.cs:23-27` invoca `base(nombre)` y ya no declara otro nombre. | **Conforme para el contrato observado.** Se cerró la divergencia de estado base/derivado. |
| `Lacteo : Producto` y `Piel : Producto` | Construcción con nombre y uso en venta genérica sin requisitos ocultos. | `Lacteo.cs:9-13`; `Piel.cs:9-13`; `Hacienda.cs:145-169`. | **Conforme.** No fortalecen precondiciones respecto del constructor de `Producto`. |
| `Ternero`, `Cebon`, `Novillo : Res` | Después de construido, cualquier cliente de `Res` puede leer edad, alimentar, vacunar y vender sin setters derivados que rechacen valores aceptados por la base. | `Res.cs:31-36` hace `Edad` inmutable; validación de rango en `Ternero.cs:13-19`, `Cebon.cs:13-19`, `Novillo.cs:13-19`; comportamiento común en `Res.cs:39-108`. | **Conforme con el modelo vigente.** Las precondiciones de construcción expresan la invariante de cada categoría; no existe mutación polimórfica de edad. |
| `Bacteriana`, `Viva : Vacuna` | Cada vacuna debe exponer tipo y decidir aplicabilidad para cualquier `Res` válida, sin operación no soportada. | `Vacuna.cs:35-42`; `Bacteriana.cs:28-38`; `Viva.cs:30-40`; límites polimórficos de `Res.cs:75-93`. | **Conforme en los caminos cubiertos.** Ambos subtipos implementan las operaciones abstractas con significado. |
| `Potrero : IInventario<Res>` | `agregar`, `contiene` y `retirar` deben modificar/consultar estado o fallar explícitamente; nunca ser no-op silencioso. | `Potrero.cs:202-263`; `IInventario.cs:8-11`; `IInventarioVendible.cs:8-12`. | **Conforme tras la remediación.** Se eliminó el `agregar` vacío identificado por la auditoría. |
| Inventarios de lácteos y pieles | Implementaciones sustituibles por sus respectivos `IInventario<T>`: alta, consulta y retiro coherentes. | `InventarioLacteos.cs:8-62`; `InventarioPieles.cs:8-62`. | **Conforme en los escenarios caracterizados.** Duplicados y ausencias fallan explícitamente. |
| Validadores concretos | Un cliente de cada interfaz solo debe poder invocar la validación prometida; no debe recibir `NotImplementedException` por capacidades ajenas. | `IValidador*.cs`; `Clases/Validaciones/Validar*.cs`; se eliminaron `Validacion` e `IValidarInformacion`. | **Conforme.** La antigua falsa jerarquía fue sustituida por contratos de capacidad. |

## Análisis específico de edad

El defecto original no era que cada categoría tuviera un rango, sino que un setter público virtual de `Res.Edad` aceptaba valores que los overrides podían rechazar. La solución conserva las reglas del dominio y elimina el punto de sustitución inválido:

- `Res.Edad` solo tiene getter (`Res.cs:34`).
- El valor se fija una vez en el constructor base (`Res.cs:23-27`).
- Cada clase concreta valida su categoría antes de delegar (`Ternero/Cebon/Novillo.cs:9-19`).
- Ningún método común de `Res` cambia la categoría mediante edad.

Esto evita fortalecer precondiciones en una operación polimórfica. No resuelve —ni pretende resolver— un requisito futuro de envejecimiento y cambio de categoría; ese eje requeriría una decisión de dominio separada, probablemente composición/estado en lugar de identidad por subtipo.

## Contrato de venta y sustitución

`Hacienda.vender<T>` opera sobre `IInventarioVendible<T>` y exige:

- inventario no nulo;
- producto no nulo;
- `contiene(producto) == true` antes del registro;
- retiro efectivo mediante `retirar(producto)`.

`Potrero`, `InventarioLacteos` e `InventarioPieles` cumplen esas capacidades. La sobrecarga histórica con `IInventario<T>` delega a la estrecha. Como `IInventario<T> : IInventarioVendible<T>`, el cast está garantizado para implementaciones conformes; para `null`, el cast conserva `null` y la implementación estrecha mantiene `ArgumentNullException(nameof(inventario))`.

## Evidencia de caracterización existente

`HaciendaNEW.Verification/Program.cs` contiene comprobaciones focalizadas para:

- identidad `Res`/`Producto` (`VerificarProductoRes`);
- contratos de venta legacy y genérica;
- ausencia de métodos falsos en validadores;
- efecto real de `Potrero.agregar`;
- inventarios sin duplicados/no-op;
- rangos e inmutabilidad de edad (`VerificarContratoRes`).

La remediación final de API compiló la biblioteca y el verifier; Test-Guardian emitió **PASS**. Adversarial-Reviewer confirmó `BLOCKER-API-001 CLOSED` y **0 blockers nuevos**.

## Incertidumbres y deuda diferida

- No se ejecutó en esta fase documental una suite nueva: el código quedó congelado tras las verificaciones focalizadas.
- No hay prueba binaria externa precompilada de la sobrecarga histórica de venta.
- `Producto.Nombre` sigue teniendo setter público; se conservó para no romper consumidores existentes. Su mutabilidad debe caracterizarse antes de restringirla.
- Las clases envuelven varias excepciones en `Exception`; es deuda de precisión contractual, no un blocker nuevo confirmado.
- `Vacuna.EstaVencida()` usa `DateTime.Now`; no se introdujo `IClock` sin presión demostrada.
