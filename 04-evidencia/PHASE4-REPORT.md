# Reporte final — Fase 4 Hacienda

**Corte:** 2026-08-10  
**Estado actual:** COMPLETE (evidencia técnica verificable).  
**Pendientes humanos:** roster/grupo/roles, contraste lectura fría, video (ver `TRAZABILIDAD-FINAL.md`).

## 1. Historia y baseline

El reporte anterior quedó en `CIERRE BLOQUEADO` porque los agentes no podían crear/ejecutar characterization. Ese hecho se conserva en `historico/EVIDENCE-BLOCKED-RECOVERY.md` y `logs/`; ya no representa el estado actual. La autorización humana para `03-src/phase4/Characterization` como infraestructura no productiva desbloqueó los runners. Antes de editar producción se confirmó:

| Gate baseline | Resultado |
|---|---|
| OLD MVC build | PASS, 5 warnings conocidos, 0 errores |
| NEW MVC build | PASS, 2 warnings conocidos, 0 errores |
| NEW verifier | PASS |

## 2. Cambios de producción justificados

| Archivo | Razón concreta |
|---|---|
| `Clases/Res.cs` | C07 probó que NEW rechazaba `Alimentar(0)` aunque OLD lo acepta; se restauró conducta. |
| `Clases/Hacienda.cs` | C10 restauró el mensaje de vacunación OLD. SOLID-LSP-001 se cerró registrando el objeto efectivamente retornado por `retirar`, no un objeto distinto que coincida por nombre. C12 restauró el publisher de evento de vencimiento con lote y fecha. C14/C15/C17 restauraron mensajes de límite OLD con tipo concreto y cantidad, y orden de validación límite antes que vencimiento. |
| `Clases/Carne.cs` | Variante faltante aprobada de SC-1. |
| `Clases/InventarioCarnes.cs` | Inventario concreto requerido para vender Carne por el contrato existente. |

No se modificó código OLD. No hubo modernización cosmética ni paquete nuevo.

## 3. Caracterización y preservación

Se ejecutaron 20 casos determinísticos (11 línea base + 9 extendidos) contra OLD y NEW. Los extendidos cubren: vacuna vencida (C12), aplicación duplicada (C13), límites bacterianos/viva por tipo de res (C14, C15), límites independientes entre tipos de vacuna (C16), orden de validación ante límite+vencimiento combinados (C17), y tres escenarios de reflexión de API pública: semántica de `L_ventas` (C18), superficie de sobrecargas `alimentar_res` (C19), y existencia de `IValidarInformacion` (C20).

| Categoría | Casos | Detalle |
|---|---|---|
| MATCH | 18 | C01–C18 — comportamiento observable idéntico |
| DELIBERATE_STRUCTURAL | 2 | C19, C20 — divergencias estructurales documentadas y aceptadas (consolidación de sobrecargas, granularidad ISP) |

Cero behavioral mismatches. C12, C14, C15 y C17 fueron restaurados al contrato OLD (mensajes exactos de límite con tipo concreto y cantidad, orden de validación límite antes que vencimiento). Mismatches iniciales C07 y C10 también fueron corregidos en iteraciones anteriores. Toda la suite completa fue repetida tras cada iteración. Detalle y outputs retenidos: `characterization/CHARACTERIZATION-MATRIX.md`, `old-output.txt`, `new-output.txt`.

## 4. SC-1 y OCP

SC-1 está seleccionada formalmente en `decisiones/SC1-SELECCION.md`. NEW vende:

- `Lacteo` mediante `InventarioLacteos`;
- `Carne` mediante `InventarioCarnes`;
- `Piel` mediante `InventarioPieles`.

Todas pasan por `Hacienda.vender<T>(IInventarioVendible<T>, T, uint)`. Agregar Carne requirió 0 clases/archivos existentes y 2 clases/archivos nuevos. OLD fue medido en Fase 2 como 4 clases/6 archivos existentes. Métrica y límites: `SC1-METRICA-OCP.md`.

La persistencia V2 conserva ventas de variantes conocidas y desconocidas. Para un subtipo no reconocido al cargar usa `ProductoPersistido` con `TipoOriginal`, nombre y monto; Carne no exigió modificar el adaptador.

## 5. Demo

`HaciendaNEW.Demo` ejecuta un flujo normal (potrero, res, alimentación y venta) y las tres ventas SC-1. Resultado: 4 ventas registradas y `SC-1 LACTEO/CARNE/PIEL: PASS`. Salida retenida en `demo-output.txt`.

## 6. TO-BE ↔ código

Se resolvió el conflicto normativo. `02-diseno/diagramas/TO-BE-FINAL.puml` reemplaza como fuente final editable a los PNG aspiracionales congelados, que se conservan solo como historia. Una versión enriquecida con miembros implementados, colores por principio SOLID, leyenda, notas de composition root y referencias a SC-2/SC-3 está en `TO-BE-FINAL-ENRICHED.puml`. La matriz bidireccional `TOBE-CODE-MATRIX.md` mapea todas las clases/interfaces productivas y no deja elementos fantasma.

**Nota:** Los PNG aspiracionales (`fase3 uml 1..4.png`) quedan formalmente SUPERSEDED. El `.puml` enriquecido es la fuente normativa con correspondencia 1:1 verificada.

## 7. Cierre SOLID focalizado

- **SRP:** `RegistroVenta` mantiene historial separado; validadores específicos y composition root conservan sus fronteras. No se dividió Hacienda solo por tamaño.
- **OCP:** Carne se añadió sin tocar la política genérica de venta ni persistencia.
- **LSP:** la jerarquía de res mantiene edad inmutable por subtipo. El hallazgo de identidad queda cerrado: la venta registra exactamente el objeto retirado; verifier lo prueba incluso si se solicita otro objeto con el mismo nombre.
- **ISP:** venta depende de `IInventarioVendible<T>` (contiene/retirar), no de agregar; servicios usan puertos de persistencia por capacidad.
- **DIP:** dominio depende de contratos; `PersistenciaService` los implementa y `Program.cs` compone detalles.

## 8. Verificación final

| Verificación | Resultado |
|---|---|
| OLD characterization runner | PASS, 20 escenarios (11 línea base + 9 extendidos) |
| NEW MVC build | PASS, 2 warnings CS8618 preexistentes en `LoginViewModel` |
| NEW verifier build/run | PASS, `TODAS LAS VERIFICACIONES PASARON.` |
| NEW characterization runner | PASS, 20 escenarios; 18 MATCH, 2 divergencias estructurales, 0 behavioral mismatches, 0 regresiones |
| Demo + SC-1 | PASS |
| TO-BE/código | PASS |
| Bitácora IA | PASS (`BITACORA-IA.md`) |

El mensaje del SDK sobre workloads es informativo; restore/build terminan correctamente. El wrapper confirma que cada ejecución deja source/status sin cambios.

## 9. Deuda técnica consciente

La deuda completa está documentada en `DEUDA-TECNICA-CONSCIENTE.md`. Resumen:

- `PersistenciaService` aún concentra formatos de varios agregados; no se amplió el refactor sin presión del cambio aprobado.
- `Producto.Nombre` tiene setter público; se conserva para no romper consumidores.
- Categorías etarias como subtipos (no composición); LSP conforme pero sin envejecimiento.
- Excepciones envueltas genéricamente; tipo original se pierde.
- `Vacuna.EstaVencida` usa `DateTime.Now`; sin `IClock`.
- Hidratación parcial de `Hacienda` en startup; sin atomicidad.
- Sin tests unitarios automatizados; cobertura mediante characterization runners y verifier.
- Los cuatro PNG históricos no fueron regenerados porque no hay herramienta PlantUML instalada/autorizada; el `.puml` editable es normativo y documenta el comando.
- OLD se ejecuta mediante su DLL legacy referenciada por el MVC, coherente con la baseline disponible; OLD source net472 sigue sin targeting pack en este host.
- Permanecen 2 warnings de nulabilidad en `LoginViewModel`, no relacionados con el slice.

## 10. Reproducción exacta

```bash
scripts/phase4-safe-dotnet.sh build 03-src/original/HaciendaOLD/p_mvcHacienda/p_mvcHacienda.csproj
scripts/phase4-safe-dotnet.sh run 03-src/phase4/Characterization/Old/Characterization.Old.csproj
scripts/phase4-safe-dotnet.sh build 03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj
scripts/phase4-safe-dotnet.sh web-smoke 03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj
scripts/phase4-safe-dotnet.sh build 03-src/redisenado/HaciendaNEW/HaciendaNEW.Verification/HaciendaNEW.Verification.csproj
scripts/phase4-safe-dotnet.sh run 03-src/redisenado/HaciendaNEW/HaciendaNEW.Verification/HaciendaNEW.Verification.csproj
scripts/phase4-safe-dotnet.sh run 03-src/phase4/Characterization/New/Characterization.New.csproj
scripts/phase4-safe-dotnet.sh build 03-src/redisenado/HaciendaNEW/HaciendaNEW.Demo/HaciendaNEW.Demo.csproj
scripts/phase4-safe-dotnet.sh run 03-src/redisenado/HaciendaNEW/HaciendaNEW.Demo/HaciendaNEW.Demo.csproj
```

## 11. Artefactos de cierre (creados en este pack de evidencia)

| Artefacto | Ubicación | Propósito |
|---|---|---|
| 5 ADR retrospectivos | `04-evidencia/adr/ADR-001..005` | Toolchain, modelo SC-1, TO-BE reducido, puertos de persistencia, jerarquía Res/LSP |
| SC-2 analizada no implementada | `SC2-ANALIZADA-NO-IMPLEMENTADA.md` | Chip/geolocalización: alcance, riesgos, razón de no implementación |
| SC-3 analizada no implementada | `SC3-ANALIZADA-NO-IMPLEMENTADA.md` | Historia clínica: alcance, riesgos, razón de no implementación |
| TO-BE enriquecido | `TO-BE-FINAL-ENRICHED.puml` | Miembros, colores SOLID, leyenda, composition root, notas SC-2/SC-3 |
| Trazabilidad final | `TRAZABILIDAD-FINAL.md` | Cadena F0→F4, hallazgo→dolor→ADR→código→verificación |
| Deuda técnica consciente | `DEUDA-TECNICA-CONSCIENTE.md` | 7 deudas declaradas con mitigación y condición de remediación |

## 12. Bloqueos vigentes

| Bloqueo | Impacto | Estado |
|---|---|---|
| Escritura en `02-diseno/adr/` | ADR deben copiarse manualmente desde `04-evidencia/adr/` | `TOOL_BLOCKED` |
| Escritura en `02-diseno/diagramas/` | PUML enriquecido debe copiarse manualmente | `TOOL_BLOCKED` |
| Escritura en `README.md` (raíz) | README académico no actualizable por este agente | `TOOL_BLOCKED` |
| Banners SUPERSEDED en `02-diseno/` | Archivos obsoletos no marcables | `TOOL_BLOCKED` |
| Generación PNG desde PUML | Sin PlantUML instalado/instalable | `TOOL_BLOCKED` |
| Roster/grupo/roles/video | Depende de decisión humana | `PENDING_HUMAN_REVIEW` |
