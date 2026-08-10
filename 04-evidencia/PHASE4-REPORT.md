# PHASE 4 EVIDENCE REPORT — CIERRE BLOQUEADO

> **Agente:** Phase4-Evidence-Engineer (P4-E03)  
> **Rama:** `agent/phase4-overnight-20260810-0354`  
> **Corte:** 2026-08-10 ~04:18 UTC  
> **Estado final:** CIERRE BLOQUEADO — baseline técnica PASS, evidencia de caracterización/comparación AUSENTE

---

## 1. GATE DE RAMA — PASS

| Verificación | Resultado |
|---|---|
| Rama activa | `agent/phase4-overnight-20260810-0354` |
| Git diff (producción) | Vacío — sin ediciones a `01-diagnostico/`, `02-diseno/`, `03-src/`, `docs/`, `scripts/` |
| Untracked | Solo `04-evidencia/` |

**Comando ejecutado:**
```
scripts/phase4-git-readonly.sh branch
scripts/phase4-git-readonly.sh status
```

**Evidencia:** `04-evidencia/logs/20260810-041801/opencode.stderr.log:21-24`

---

## 2. VALIDACIÓN DE WORKBENCH — TOOL_BLOCKED

El comando `scripts/validate-phase4-workbench.sh` fue bloqueado por permisos del entorno (restricción de seguridad que solo permite `scripts/phase4-safe-dotnet.sh` y `scripts/phase4-git-readonly.sh`).

**Evidencia:** `04-evidencia/logs/20260810-041801/opencode.stderr.log:31-33`

---

## 3. BASELINE TÉCNICA (safe wrapper)

### 3.1 OLD — PASS

| Comando | Resultado |
|---|---|
| `scripts/phase4-safe-dotnet.sh build 03-src/original/HaciendaOLD/p_mvcHacienda/p_mvcHacienda.csproj` | **PASS** — Build succeeded, 5 warnings, 0 errors |
| `scripts/phase4-safe-dotnet.sh web-smoke 03-src/original/HaciendaOLD/p_mvcHacienda/p_mvcHacienda.csproj` | **PASS** — Application startup observable |

Warnings OLD (CS8625 ×2, CS8618 ×2, CS8619 ×1 en `AccountController.cs`, `LoginViewModel.cs`, `UsuarioService.cs`).

**Evidencia:** `opencode.stderr.log:34-96`

### 3.2 NEW — PASS

| Comando | Resultado |
|---|---|
| `scripts/phase4-safe-dotnet.sh build 03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj` | **PASS** — Build succeeded, 2 warnings, 0 errors |
| `scripts/phase4-safe-dotnet.sh web-smoke 03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj` | **PASS** — Application startup observable |

Warnings NEW (CS8618 ×2 en `LoginViewModel.cs` únicamente; `AccountController` y `UsuarioService` ya no generan advertencias).

**Evidencia:** `opencode.stderr.log:98-152`

### 3.3 NEW Verifier — PASS

| Comando | Resultado |
|---|---|
| `scripts/phase4-safe-dotnet.sh build 03-src/redisenado/HaciendaNEW/HaciendaNEW.Verification/HaciendaNEW.Verification.csproj` | **PASS** — Build succeeded, 2 warnings, 0 errors |
| `scripts/phase4-safe-dotnet.sh run 03-src/redisenado/HaciendaNEW/HaciendaNEW.Verification/HaciendaNEW.Verification.csproj` | **PASS** — `TODAS LAS VERIFICACIONES PASARON.` |

Verificaciones ejecutadas (todas las listadas a continuación pasaron):
- Assembly source apunta a NEW (no OLD)
- Producto/Res integridad y validación
- Constructores Venta TO-BE y legacy
- Venta legacy: retiro y registro exactos
- ValidadorRes, ValidadorPotrero, ValidadorVacuna, ValidadorVenta
- Validadores sin métodos ajenos ni NotImplementedException
- PersistenciaService implementa puertos de persistencia
- Bib_Hacienda sin dependencias técnicas de infraestructura
- Controladores no dependen directamente de Hacienda ni PersistenciaService
- Contrato vendible estrecho (`IInventarioVendible<T>`)
- Potrero.agregar con semántica real de capacidad/tipo/duplicado
- Inventarios sin duplicados ni no-ops
- Venta genérica de Res, Lacteo, Piel
- Venta genérica de Producto definido solo en el verifier (extensibilidad OCP)
- Venta rechaza producto ausente/null sin venta parcial
- Contrato común Res/Ternero/Cebon/Novillo: inmutabilidad de Edad, rangos, Alimentar, vacunación
- Persistencia de ventas legacy y genéricas (incluido producto externo)
- VentaService filtra ventas sin potrero y no falla con lista mixta
- Vista Venta/Index tiene ramas null-safe
- Persistencia legacy aborta en número malformado
- Ventas cargadas desde persistencia se registran en Hacienda y son visibles por VentaService
- Composición de validadores/interceptores centralizada en Program.cs

**Evidencia:** `opencode.stderr.log:446-508`

### 3.4 Tracked generated files

| Métrica | Valor |
|---|---|
| Archivos generados rastreados (bin/obj) | **284** |

Conteo obtenido por `scripts/phase4-git-readonly.sh tracked-generated` en cada invocación del safe wrapper.

**Evidencia:** `opencode.stderr.log:35,60,99,120,447,471`

---

## 4. AGENTES Y GATES DE EVIDENCIA

### 4.1 Codebase Cartographer — TOOL_BLOCKED

Dos invocaciones:
1. `Codebase-Cartographer Agent` — completó sin output de mapa (vacío)
2. `Retry Phase4 map` — completó sin output de mapa (vacío)

Clasificación: **TOOL_BLOCKED**. El agente terminó sin error explícito pero no produjo artefacto de mapeo. No se dispone de PHASE4 EVIDENCE PACK formal.

**Evidencia:** `opencode.stderr.log:154,442-444`

### 4.2 P4-E01 Evidence Writer (OLD characterization) — TOOL_BLOCKED

Dos invocaciones:
1. `Write OLD characterization` — completó sin output de caracterización
2. `Retry OLD evidence` — completó sin output de caracterización

Clasificación: **TOOL_BLOCKED**. No se produjo ningún escenario de caracterización OLD.

**Evidencia:** `opencode.stderr.log:512-515`

### 4.3 Consecuencia sobre Gates 1 y 2

| Gate | Requisito | Estado |
|---|---|---|
| Gate 1 — OLD Characterization | ≥8 escenarios OLD con ID, nombre, precondición, input, operaciones, output observable, post-estado | **AUSENTE** |
| Gate 2 — NEW Characterization + Comparison | Mismos inputs/operaciones, MATCH/MISMATCH, rutas de evidencia | **AUSENTE** |

**Sin Gate 1 ni Gate 2, la preservación de comportamiento no está verificada.** La baseline técnica (build + web-smoke + verifier) demuestra que OLD y NEW compilan y arrancan, pero no prueba equivalencia conductual ante los 8 escenarios mínimos requeridos.

---

## 5. AUDITORÍAS SOLID FINALES

### 5.1 SRP Auditor — TOOL_BLOCKED (INCONCLUSIVE)

El auditor SRP final ejecutó y completó sin output. La ausencia de output no constituye evidencia de ausencia de hallazgos; el resultado es inconcluso.

### 5.2 OCP Auditor — TOOL_BLOCKED (INCONCLUSIVE)

El auditor OCP final ejecutó y completó sin output. La ausencia de output no constituye evidencia de ausencia de hallazgos; el resultado es inconcluso.

### 5.3 ISP Auditor — INCOMPLETO

El auditor ISP final ejecutó pero no produjo hallazgos confirmados. La auditoría queda incompleta.

### 5.4 DIP Auditor — SIN HALLAZGOS

El auditor DIP final ejecutó y no reportó hallazgos. Consistente con la separación observada: `Bib_Hacienda` sin dependencias técnicas (verificado por el verifier), `PersistenciaService` implementa puertos, controladores no dependen de dominio/persistencia.

### 5.5 Architecture Auditor — TO-BE↔CODE NORMATIVE-SOURCE CONFLICT (CONFIRMED)

El auditor de arquitectura confirma un bloqueo de alineación de evidencia (evidence-alignment blocker):

- **Conflicto de fuente normativa TO-BE↔código (CONFIRMED):** los modelos UML PNG congelados de Fase 3 contienen elementos — ItemVenta, IVendible, ProductoGanadero, interfaces de aplicación y adaptadores — que difieren de los documentos textuales posteriores de Fase 3 y del código actual. La fuente normativa de preferencia no está resuelta (¿los diagramas PNG congelados o los documentos textuales? ¿Cuál de los dos prevalece?), por lo que el mapeo bidireccional TO-BE↔código no puede ser certificado. Esta es una cuestión de alineación arquitectónica entre artefactos de diseño, no de caracterización conductual OLD↔NEW.

- **MVC reachability (SOSPECHADO/CONDICIONAL):** la alcanzabilidad MVC desde controladores hasta vistas no está confirmada como violación; solo es verificable si el contrato TO-BE la exige explícitamente. Se mantiene como hallazgo sospechado y condicional.

### 5.6 LSP Auditor — HALLAZGO CONFIRMADO (ALTA SEVERIDAD)

**ID:** SOLID-LSP-001  
**Principio:** LSP  
**Estado:** CONFIRMADO  
**Severidad:** Alta / Confianza: 97%  
**Ubicación:** `Producto.Nombre` (mutable, setter público) + `IInventario<T>.contiene` / `IInventario<T>.retirar` basados en búsqueda por nombre  
**Evidencia:** `Producto.cs` expone `Nombre` con setter público; `InventarioLacteos`, `InventarioPieles`, `Potrero` comparan por `Nombre` (case-insensitive). `Hacienda.vender<T>` invoca `inventario.retirar(producto)` después de registrar la venta, donde `retirar` busca por nombre.

**Contrato violado:** Colisión de identidad. Si un `Producto` cambia de nombre entre su inserción en el inventario y la venta, o si dos productos comparten nombre (incluso en tipos distintos), `vender<T>` puede:
1. Registrar la venta con el producto B (referencia pasada como argumento)
2. Pero `retirar` remueve el producto A (el que coincide por nombre en el inventario)

**Consecuencia:** El registro de venta (`L_ventas`) contiene B, pero `retirar` remueve A del inventario. B permanece en inventario mientras A es removido, generando inconsistencia entre el registro de venta y el estado real del inventario.

**No se autoriza corrección en producción.** Las puertas de evidencia bloqueadas impiden cualquier edición de producción en esta fase.

---

## 6. TEST GUARDIAN — RECHAZO DE CLASIFICACIÓN DE CIERRE

El Test Guardian argumentó **PASS** para el cierre de Phase 4, pero simultáneamente confirmó que:
- La caracterización OLD obligatoria (Gate 1) está **ausente**
- La comparación OLD↔NEW obligatoria (Gate 2) está **ausente**

**Decisión del Evidence Engineer:** Se rechaza explícitamente la clasificación de cierre del Test Guardian como inconsistente con las puertas congeladas definidas en `.opencode/phase4-overnight.md` (líneas 21-34). Sin Gate 1 y Gate 2 satisfechos, el cierre de Phase 4 no puede declararse PASS.

**Se retienen** las observaciones factuales del Test Guardian sobre build y verifier, que coinciden con lo registrado en la sección 3 de este reporte.

---

## 7. ADVERSARIAL REVIEWER — TOOL_BLOCKED

Dos invocaciones del adversarial reviewer retornaron vacías (sin output de revisión). Clasificación: **TOOL_BLOCKED**.

**Evidencia:** `opencode.stderr.log:534-538`

---

## 8. SC-1 — PENDING_HUMAN_REVIEW

La selección de SC-1 (Hacienda — productos derivados del ganado) es inferida del contexto del repositorio pero **no está formalmente declarada por el equipo**. No se encontró documento de decisión de SC en el repositorio.

| Campo | Valor |
|---|---|
| SC inferida | SC-1 (derivados: lácteos, pieles) |
| Estado | **PENDING_HUMAN_REVIEW** |
| Evidencia de implementación | `vender<T>` genérico funcional sobre `IInventarioVendible<T>` (verificado) |
| Productos concretos | `Lacteo`, `Piel`, `ProductoVerificador` (extensibilidad OCP demostrada) |
| Persistencia | TXT con formato V2 para ventas genéricas + legacy 7-campos |
| Demo/Métrica | **AUSENTE** |

---

## 9. CONSOLIDACIÓN DE BLOQUEOS

| Bloqueo | Causa raíz | Impacto |
|---|---|---|
| Gate 1 (≥8 OLD characterization) | P4-E01 evidence writer retornó vacío ×2 (TOOL_BLOCKED) | Sin baseline conductual OLD |
| Gate 2 (OLD↔NEW comparison) | Depende de Gate 1 | Sin verificación de preservación |
| Gate 4 (demo + métricas + evidencia) | Depende de Gates 1-3 | AUSENTE |
| Workbench validation | Bloqueo de permisos (solo safe wrappers) | Sin validación completa de entorno |
| Cartographer (evidencia pack) | Retornó vacío ×2 (TOOL_BLOCKED) | Sin PHASE4 EVIDENCE PACK |
| Adversarial review | Retornó vacío ×2 (TOOL_BLOCKED) | Sin revisión de cierre |
| SC-1 formal | Sin declaración del equipo | PENDING_HUMAN_REVIEW |

---

## 10. COMANDOS EXACTOS EJECUTADOS (safe wrapper)

```bash
# Baseline
scripts/phase4-safe-dotnet.sh build 03-src/original/HaciendaOLD/p_mvcHacienda/p_mvcHacienda.csproj
scripts/phase4-safe-dotnet.sh web-smoke 03-src/original/HaciendaOLD/p_mvcHacienda/p_mvcHacienda.csproj
scripts/phase4-safe-dotnet.sh build 03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj
scripts/phase4-safe-dotnet.sh web-smoke 03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj

# Verifier
scripts/phase4-safe-dotnet.sh build 03-src/redisenado/HaciendaNEW/HaciendaNEW.Verification/HaciendaNEW.Verification.csproj
scripts/phase4-safe-dotnet.sh run 03-src/redisenado/HaciendaNEW/HaciendaNEW.Verification/HaciendaNEW.Verification.csproj

# Git (read-only)
scripts/phase4-git-readonly.sh branch
scripts/phase4-git-readonly.sh status
scripts/phase4-git-readonly.sh tracked-generated
```

---

## 11. RUTAS DE EVIDENCIA DISPONIBLES

| Artefacto | Ruta |
|---|---|
| Log completo de la sesión (stdout) | `04-evidencia/logs/20260810-041801/opencode.stdout.log` |
| Log completo de la sesión (stderr) | `04-evidencia/logs/20260810-041801/opencode.stderr.log` (550 líneas) |
| Este reporte | `04-evidencia/PHASE4-REPORT.md` |
| Verifier source | `03-src/redisenado/HaciendaNEW/HaciendaNEW.Verification/Program.cs` |
| Safe scripts | `scripts/phase4-safe-dotnet.sh`, `scripts/phase4-git-readonly.sh` |
| Contrato de fase | `.opencode/phase4-overnight.md` |
| Estado previo (auditoría Fase 3) | `ESTADO_ACTUAL_Y_PLAN_CIERRE.md` |

**Nota sobre referencias a líneas de log:** todas las referencias del tipo `opencode.stderr.log:NN-MM` en este reporte apuntan a rangos de línea cuya existencia posicional ha sido verificada contra el archivo de 550 líneas en disco. La correspondencia semántica línea↔evidencia no ha sido re-validada independientemente para cada referencia.

---

## 12. RIESGOS PENDIENTES

| ID | Riesgo | Severidad | Gate afectado |
|---|---|---|---|
| R-01 | Sin caracterización OLD: diferencia de conducta no detectada (ej. mensaje de venta, Alimentar(0), vacunación completa) | Blocker | Gate 1, 2, 4 |
| R-02 | Sin comparación OLD↔NEW: regresiones silenciosas posibles en `vender_res`, `L_ventas`, TXT legacy | Blocker | Gate 2, 4 |
| R-03 | Colisión de identidad SOLID-LSP-001 (Producto.Nombre mutable): venta genérica registra B y retira A; B permanece en inventario | Alta | Gate 3 (SC-1 vertical) |
| R-04 | SC-1 no declarada formalmente: implementación podría no alinearse con expectativas del equipo | Media | Gate 3, 4 |
| R-05 | Sin adversarial review: falsos positivos o sobre-ingeniería no detectados | Media | Gate 4 |
| R-06 | Build OLD usa net8 con HintPath a DLL legacy; Bib fuente net472 no compila en este host | Media | Gate 1 (baseline) |

---

## 13. ACCIONES DIFERIDAS

| ID | Acción | Responsable | Dependencia |
|---|---|---|---|
| A-01 | Ejecutar 8 caracterizaciones OLD con runners aislados | Evidence Engineer | Ninguna |
| A-02 | Ejecutar comparación OLD↔NEW sobre los 8 escenarios | Evidence Engineer | A-01 |
| A-03 | Declarar formalmente SC-1 (o la SC real del equipo) | Equipo humano (PENDING_HUMAN_REVIEW) | Ninguna |
| A-04 | Resolver colisión de identidad SOLID-LSP-001 (Nombre inmutable o identidad por clave) — requiere slice de producción autorizado independientemente | Refactor Implementer | A-02, decisión de equipo |
| A-05 | Completar demo + métricas SC-1 — responsabilidad del Evidence Engineer; no requiere slice de producción | Evidence Engineer | A-01, A-02, A-03 |
| A-06 | Las auditorías finales SRP, OCP, LSP, ISP, DIP y arquitectura ya se ejecutaron una vez; no se reprograman | — | — |

---

## 14. VEREDICTO FINAL

**Phase 4 CIERRE: BLOQUEADO**

La baseline técnica es sólida: OLD compila y arranca, NEW compila y arranca, todas las verificaciones listadas del verifier pasaron. Sin embargo, las puertas de evidencia conductual (Gate 1: ≥8 caracterizaciones OLD; Gate 2: comparación OLD↔NEW) están **ausentes** porque el agente de evidencia P4-E01 retornó sin output en dos intentos (TOOL_BLOCKED). Sin estas puertas, no es posible certificar preservación de comportamiento, que es el requisito fundamental de Phase 4.

El verifier demuestra corrección estructural del diseño NEW (contratos, validadores, persistencia, controladores, extensibilidad OCP), pero **no sustituye la caracterización conductual** exigida por las puertas congeladas.

No se realizaron ediciones de producción. El diff de Git está vacío. Solo existen artefactos de evidencia nuevos bajo `04-evidencia/`.

---

**Sin checkpoint final.** El cierre permanece bloqueado hasta que las puertas 1 y 2 sean satisfechas con evidencia reproducible.
