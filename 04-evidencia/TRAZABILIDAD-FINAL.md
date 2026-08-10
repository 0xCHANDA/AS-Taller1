# Trazabilidad final — Hacienda Phase 4

**Corte:** 2026-08-10
**Propósito:** Verificar que cada hallazgo, dolor y decisión arquitectónica se conecta con artefactos verificables.

## Cadena Fase 0 → Fase 4

| Fase | Artefacto | Ubicación | Estado |
|---|---|---|---|
| F0 | Lectura en frío (2 hojas) | `00-lectura-en-frio/Analisis-SantiagoHM.md`, `Analisis-SebastianQJ.docx` | Conservado sin modificar |
| F0 | Contraste inicial-final | No realizado | `PENDING_HUMAN_REVIEW` |
| F1 | AS-IS (PDF + PNG) | `01-diagnostico/Hacienda_AS-IS.pdf`, `diagramas/1..4.png` | Conservado |
| F1 | Inventario de hallazgos | `01-diagnostico/inventario de hallazgos.docx` | Conservado |
| F1 | Puntos de dolor priorizados | `01-diagnostico/puntos de dolor priorizados.docx` | Conservado |
| F2 | Baseline SC-1/2/3 | `01-diagnostico/Fase 2 — Los cambios que vienen.docx` | Congelado |
| F3 | TO-BE normativo | `02-diseno/diagramas/TO-BE-FINAL.puml` | Activo |
| F3 | PNG aspiracionales (históricos) | `02-diseno/diagramas/fase3 uml 1..4.png` | SUPERSEDED |
| F3 | Argumentación SOLID | `02-diseno/Argumentacion-SOLID-con-evidencia.md` | Activo |
| F3 | LSP | `02-diseno/Verificacion-de-herencias-LSP.md` | Activo |
| F3 | DIP / composition root | `02-diseno/Inversiones-de-dependencia.md` | Activo |
| F3 | Plan de cierre (obsoleto) | `02-diseno/Hacienda_Plan_Cierre_Sobresaliente.md` | SUPERSEDED |
| F4 | 5 ADR retrospectivos | `04-evidencia/adr/ADR-001..005` | Activo |
| F4 | Caracterización (11 casos) | `04-evidencia/characterization/CHARACTERIZATION-MATRIX.md` | Activo |
| F4 | Métrica OCP SC-1 | `04-evidencia/SC1-METRICA-OCP.md` | Activo |
| F4 | TO-BE ↔ código | `04-evidencia/TOBE-CODE-MATRIX.md` | Activo |
| F4 | Bitácora IA | `04-evidencia/BITACORA-IA.md` | Activo |
| F4 | Demostración SC-1 | `04-evidencia/demo-output.txt` | Activo |
| F4 | Selección SC-1 | `04-evidencia/decisiones/SC1-SELECCION.md` | Activo |
| F4 | SC-2 analizada | `04-evidencia/SC2-ANALIZADA-NO-IMPLEMENTADA.md` | Activo |
| F4 | SC-3 analizada | `04-evidencia/SC3-ANALIZADA-NO-IMPLEMENTADA.md` | Activo |
| F4 | Deuda técnica | `04-evidencia/DEUDA-TECNICA-CONSCIENTE.md` | Activo |
| F4 | Reporte Phase 4 | `04-evidencia/PHASE4-REPORT.md` | Activo |

## Trazabilidad hallazgo → dolor → ADR → código → verificación

| Hallazgo | Dolor | ADR | TO-BE | Código | Verificación |
|---|---|---|---|---|---|
| H-01 ISP validadores | #3 Validación | ADR implícito en TO-BE | Interfaces `IValidador*` segregadas | `IValidadorRes.cs:5-8`, `ValidarRes.cs:11-20` | Verifier: `VerificarValidadores` |
| H-02 SRP Hacienda | #2 Hacienda | ADR-002 | `vender<T>` genérica + `RegistroVenta` | `Hacienda.cs:138-169`, `RegistroVenta.cs:9-22` | Verifier: venta genérica |
| H-03 OCP Potrero | No priorizado | ADR-002 | `IInventario<T>` + implementaciones | `Potrero.cs:13`, `InventarioLacteos.cs`, `InventarioCarnes.cs`, `InventarioPieles.cs` | Verifier: `VerificarInventarios` |
| Persistencia acoplada | #1 Persistencia | ADR-004 | 5 puertos `IPersistencia*` | `IPersistencia.cs:6-36`, `Program.cs:71-77` | Verifier: `VerificarPuertosPersistencia` |
| LSP Res/Producto | No priorizado | ADR-005 | Jerarquía con `Edad` inmutable | `Res.cs:23-36`, `Ternero.cs:9-19`, etc. | Verifier: `VerificarProductoRes`, `VerificarContratoRes` |
| DLL OLD ejecutado como NEW | No diagnosticado | ADR-001 | `ProjectReference` + build verificable | `p_mvcHacienda.csproj:14-16`, `Bib_Hacienda.csproj:1-12` | Verifier: assembly/hash |

## Trazabilidad SC

| SC | Análisis Fase 2 | TO-BE | Implementación | Evidencia |
|---|---|---|---|---|
| SC-1 | Baseline 4/6 | `Producto` + `IInventarioVendible<T>` | Carne añadida: 0 mod, 2 nuevas | `SC1-METRICA-OCP.md`, `demo-output.txt` |
| SC-2 | Baseline 4/5 | No implementado | Análisis documentado | `SC2-ANALIZADA-NO-IMPLEMENTADA.md` |
| SC-3 | Baseline 5/8+1/2 | No implementado | Análisis documentado | `SC3-ANALIZADA-NO-IMPLEMENTADA.md` |

## Brechas sin cerrar

| Brecha | Estado | Responsable |
|---|---|---|
| Roster/grupo/roles | `PENDING_HUMAN_REVIEW` | Integrador humano |
| Contraste lectura fría | `PENDING_HUMAN_REVIEW` | Integrador humano |
| Video | `PENDING_HUMAN_REVIEW` | Equipo |
| README académico (equipo/roles/SC) | Bloqueado por restricción de escritura en raíz | `TOOL_BLOCKED` |
| ADR en `02-diseno/adr/` | Creados en `04-evidencia/adr/` por restricción de ruta | `TOOL_BLOCKED` |
| TO-BE-FINAL.puml en `02-diseno/diagramas/` | Versión enriquecida creada en `04-evidencia/TO-BE-FINAL-ENRICHED.puml` | `TOOL_BLOCKED` |
| Banners SUPERSEDED en archivos `02-diseno/` | Restricción de escritura | `TOOL_BLOCKED` |
| TO-BE-FINAL.png | Sin PlantUML instalado; sin permiso de instalación | `TOOL_BLOCKED` |

## Verificación de enlaces y contenido

- `characterization/CHARACTERIZATION-MATRIX.md`: 11 escenarios, MATCH 11/11, outputs retenidos.
- `characterization/old-output.txt`, `new-output.txt`: existentes, legibles.
- `demo-output.txt`: 4 ventas registradas, SC-1 PASS.
- `BITACORA-IA.md`: 7 decisiones registradas.
- `SC1-METRICA-OCP.md`: OLD 4/6, NEW 0/2.
- `TOBE-CODE-MATRIX.md`: bidireccional, sin tipos fantasma.
- `PHASE4-REPORT.md`: verificación final PASS, build PASS.
- `decisiones/SC1-SELECCION.md`: selección formal documentada.
