# ADR-006 — Extracción de `FabricadorVacunas` desde `Hacienda`

**Estado:** ACEPTADO (refactor 2026-08-10)
**Fecha:** 2026-08-10
**Evidencia:** `Clases/FabricadorVacunas.cs` (170 líneas, NUEVO); `Clases/Hacienda.cs:250-272` (delegación); `04-evidencia/bitacora-ia/BITACORA-IA.md`; `04-evidencia/metricas/SC1-METRICA-OCP.md`.

## Contexto

Antes del refactor, `Hacienda` concentraba cuatro sobrecargas de `crear_vacuna`:

- `crear_vacuna(string, string, DateTime, DateTime, uint)` — bacteriana individual.
- `crear_vacuna(string, string, DateTime, DateTime, enum_l_atenuaciones)` — viva individual.
- `crear_vacuna(string, string, DateTime, DateTime, uint, uint)` — lote bacteriano.
- `crear_vacuna(string, string, DateTime, DateTime, enum_l_atenuaciones, uint)` — lote vivo.

Cada una repetía el mismo patrón de validación: nombre no vacío, lote no vacío, fecha de vencimiento posterior a la de aplicación, lote no duplicado. La duplicación sumaba ~180 líneas.

Adicionalmente, `Hacienda` debía hacer `new List<Vacuna>()` inline y mantener la lista como campo privado. La creación de vacunas no era una responsabilidad cohesiva: la misma clase también coordinaba potreros, reses, ventas y aplicación de vacunas.

Hallazgo asociado: H-08, documentado en `02-diseno/DISENO-TO-BE.md`.

## Alternativas evaluadas

| ID | Descripción | Evaluación |
|---|---|---|
| A | Mantener la creación de vacunas en `Hacienda`. | **Descartada.** Viola SRP: cuatro métodos de creación + lista privada en la misma fachada que coordina seis dominios. Duplicación documentada. |
| B | Extraer una clase concreta `FabricadorVacunas` que tiene referencia a la lista de vacunas. | **Elegida.** Una sola responsabilidad: crear y añadir vacunas. La fachada delega con `return fabricadorVacunas.Crear(...);`. El `new List<Vacuna>()` se mueve al constructor de `Hacienda`. |
| C | Introducir una `IVacunaFactory` con hierarchy de tipos y reflection para construir Bacteriana/Viva. | **Descartada.** Solo existen dos tipos de vacuna. Una jerarquía completa con reflection incrementaría la complejidad sin cliente ni variación real. La clase concreta `FabricadorVacunas` documenta la deuda consciente. Ver `BITACORA-IA.md`, decisión sobre la fábrica de vacunas. |

## Decisión tomada

Alternativa B. Se crea `Bib_Hacienda/Clases/FabricadorVacunas.cs` (170 líneas) con:

- Constructor `FabricadorVacunas(List<Vacuna> l_vacunas)`.
- Cuatro métodos públicos: `Crear` (bacteriana), `Crear` (viva), `CrearLote` (bacteriano), `CrearLote` (vivo).
- Validación común: `ValidarDatosBasicos` (nombre, lote, fechas, duplicado) y `ValidarCantidadYLoteBase` (cantidad, lote base).
- `L_vacunas` propiedad para acceso de sólo lectura.

`Hacienda` añade campo `private readonly FabricadorVacunas fabricadorVacunas` y delega las cuatro sobrecargas de `crear_vacuna` con `return fabricadorVacunas.Crear(...)` o `return fabricadorVacunas.CrearLote(...)`.

## Por qué

La frontera coincide con el change driver. Cuando cambien las reglas de creación de vacunas (período, atenuación, validaciones), solo se modifica `FabricadorVacunas`. La fachada conserva su API pública (`IVacunacion`, `IVentaRes`, `ICreacionVacuna`) intacta.

La clase concreta es aceptable porque:

- Solo hay una implementación prevista.
- El acoplamiento a `Bacteriana` y `Viva` concretas es deuda consciente (ver §5).
- `IFabricadorVacunas` crearía un puerto sin cliente (ver `BITACORA-IA.md`, decisión sobre IFabricadorVacunas).

## Consecuencias positivas

- `Hacienda` pasa de 464 a 301 líneas.
- Validación de vacunas centralizada en `FabricadorVacunas.ValidarDatosBasicos` y `ValidarCantidadYLoteBase`.
- `Hacienda` cumple una responsabilidad más cohesiva: coordinación de agregados.
- Las cuatro sobrecargas de `Hacienda.crear_vacuna` se reducen a cuatro líneas de delegación.
- `Hacienda` admite composición externa: `new Hacienda(registroVentas, fabricadorVacunas)` desde `Program.cs`.

## Consecuencia negativa / costo aceptado

- **`FabricadorVacunas` sigue acoplada a `Bacteriana` y `Viva` concretas.** Si aparece un tercer tipo de vacuna (por ejemplo, una `Toxoide`), la fábrica concreta debe modificarse. No se introdujo `IVacunaFactory` con reflection para no incrementar la complejidad sin cliente real.
- **`FabricadorVacunas` comparte referencia a la lista de Hacienda.** Dos clases tienen acceso de escritura al mismo `List<Vacuna>`. Aceptado por exclusión mutua en el dominio y testabilidad. Si esto fuera un problema, se introduciría una `IHacienda` o se movería `l_vacunas` a `FabricadorVacunas`. No es el caso actual.
- **Las validaciones comunes solo se eliminan parcialmente.** El método `ValidarCantidadYLoteBase` duplica la lógica de `ValidarDatosBasicos` para el caso de lotes. Aceptado por la claridad de los nombres y por evitar un parámetro booleano.

## Principios SOLID involucrados

- **SRP:** principal. La responsabilidad de crear vacunas se aísla.
- **OCP:** parcialmente. El eje "tipo de producto vendible" sigue cerrado. El eje "tipo de vacuna" no está cerrado (deuda consciente).
- **DIP:** secundario. El constructor `Hacienda(registroVentas, fabricadorVacunas)` aplica inyección de dependencias y externalización de construcción. NO es DIP porque `RegistroVenta` y `FabricadorVacunas` son concretos, no abstracciones. DIP requeriría `IRegistroVenta` o `IFabricadorVacunas` que no se justifican.

## Razón anti-overengineering

- **NO se introdujo `IVacunaFactory`** porque la variación actual es de solo dos tipos. Una jerarquía completa con reflection consume más líneas de las que ahorra y añade un puerto sin cliente.
- **NO se introdujo `IFabricadorVacunas`** para evitar un puerto sin uso. El usuario de la fábrica es solo `Hacienda`.
- **NO se dividió `FabricadorVacunas` en `FabricadorVacunasBacterianas` + `FabricadorVacunasVivas`** porque las cuatro sobrecargas viven juntas por la validación común y la composición simple.
- **NO se reescribió `Res.Alimentar` o `Res.aplicar_vacuna`** porque la coordinación con eventos y subtipos es cohesiva.

## Evidencia en código

- `Bib_Hacienda/Bib_Hacienda/Clases/FabricadorVacunas.cs` (Nueva, 170 líneas).
- `Bib_Hacienda/Bib_Hacienda/Clases/Hacienda.cs:22` (campo `fabricadorVacunas`).
- `Bib_Hacienda/Bib_Hacienda/Clases/Hacienda.cs:58-64` (constructor injection / DI, no DIP).
- `Bib_Hacienda/Bib_Hacienda/Clases/Hacienda.cs:250-272` (delegación de `crear_vacuna`).
- `p_mvcHacienda/Program.cs:79-85` (composition root con `new Hacienda(registroVentas, fabricadorVacunas)`).

## Evidencia Fase 4

- `HaciendaNEW.Verification`: PASS (29/29 verificaciones, ninguna regresión).
- `Characterization.New`: 19 MATCH, 1 DELIBERATE_STRUCTURAL, 0 BEHAVIORAL_MISMATCH. Idéntico a OLD para C01-C19 salvo la interfaz monolítica C20, retirada deliberadamente por ISP.
- `04-evidencia/bitacora-ia/BITACORA-IA.md` recoge las decisiones sobre `FabricadorVacunas`, DI y los límites del rediseño.
- `04-evidencia/metricas/SC1-METRICA-OCP.md` confirma que el refactor no impacta la métrica SC-1.
