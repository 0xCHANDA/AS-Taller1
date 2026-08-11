# LIMPIEZA-02-diseno — Manifiesto de transición Fase 3

> **Origen:** la Fase 3 quedó consolidada en `04-evidencia/diseno/` durante la fase de cierre (2026-08-10). El corpus canónico reside en `04-evidencia/diseno/` porque la raíz `02-diseno/` no es escribible por los agentes automatizados.
>
> **Acción esperada del curator/usuario:** ejecutar `git mv` listed abajo. Las razones son contractuales, no estéticas.

## 1. Archivos a MOVER (git mv) — corpus canónico de Fase 3

| Origen | Destino | Razón |
|---|---|---|
| `04-evidencia/diseno/DISENO-TO-BE.md` | `02-diseno/DISENO-TO-BE.md` | Documento único de Fase 3 consolidado. |
| `04-evidencia/diseno/diagramas/TO-BE.puml` | `02-diseno/diagramas/TO-BE.puml` | PUML canónico en notación extendida con FabricadorVacunas, colores y leyenda. |
| `04-evidencia/diseno/TRAZABILIDAD-ADR.md` | `02-diseno/TRAZABILIDAD-ADR.md` | Cadena hallazgo → ADR → elemento TO-BE. |
| `04-evidencia/diseno/LIMPIEZA-02-diseno.md` | `02-diseno/LIMPIEZA-02-diseno.md` | Este manifiesto. |

### 1.1 Secuencia de comandos sugerida

```bash
git mv 04-evidencia/diseno/DISENO-TO-BE.md          02-diseno/DISENO-TO-BE.md
git mv 04-evidencia/diseno/diagramas/TO-BE.puml    02-diseno/diagramas/TO-BE.puml
git mv 04-evidencia/diseno/TRAZABILIDAD-ADR.md     02-diseno/TRAZABILIDAD-ADR.md
git mv 04-evidencia/diseno/LIMPIEZA-02-diseno.md   02-diseno/LIMPIEZA-02-diseno.md
```

## 2. Archivos a ARCHIVAR — `02-diseno/` históricos u obsoletos

| Origen | Destino propuesto | Razón |
|---|---|---|
| `02-diseno/Argumentacion-SOLID-con-evidencia.md` | `04-evidencia/historico/fase3-sustituciones/Argumentacion-SOLID-con-evidencia.md` | Contenido consolidado en `DISENO-TO-BE.md` §4-§9. Mantiene valor histórico referencial. |
| `02-diseno/Hacienda_Plan_Cierre_Sobresaliente.md` | `04-evidencia/historico/fase3-sustituciones/Hacienda_Plan_Cierre_Sobresaliente.md` O eliminar. | Plan de cierre histórica; dice "Fase 3 falta", "Fase 4 falta", "Fase 5 falta". Obsoleto. |
| `02-diseno/Inversiones-de-dependencia.md` | `04-evidencia/historico/fase3-sustituciones/Inversiones-de-dependencia.md` | DIP ahora consolidado en `DISENO-TO-BE.md` §8. |
| `02-diseno/Verificacion-de-herencias-LSP.md` | `04-evidencia/historico/fase3-sustituciones/Verificacion-de-herencias-LSP.md` | LSP ahora consolidado en `DISENO-TO-BE.md` §6. |
| `02-diseno/diagramas/TO-BE-FINAL.puml` | `04-evidencia/historico/fase3-sustituciones/TO-BE-FINAL.puml` | Versión simplista sin FabricadorVacunas; reemplazado por `diseno/diagramas/TO-BE.puml`. |
| `02-diseno/diagramas/fase3 uml 1.png` | `04-evidencia/historico/fase3-sustituciones/fase3-uml-1.png` | PNG aspiracional Fase 3, ya marcado SUPERSEDED. |
| `02-diseno/diagramas/fase3 uml 2.png` | `04-evidencia/historico/fase3-sustituciones/fase3-uml-2.png` | Mismo. |
| `02-diseno/diagramas/fase3 uml 3.png` | `04-evidencia/historico/fase3-sustituciones/fase3-uml-3.png` | Mismo. |
| `02-diseno/diagramas/fase3 uml 4.png` | `04-evidencia/historico/fase3-sustituciones/fase3-uml-4.png` | Mismo. |

### 2.1 Secuencia de comandos sugerida (archivado)

```bash
mkdir -p 04-evidencia/historico/fase3-sustituciones
git mv 02-diseno/Argumentacion-SOLID-con-evidencia.md 04-evidencia/historico/fase3-sustituciones/Argumentacion-SOLID-con-evidencia.md
git mv 02-diseno/Hacienda_Plan_Cierre_Sobresaliente.md 04-evidencia/historico/fase3-sustituciones/Hacienda_Plan_Cierre_Sobresaliente.md
git mv 02-diseno/Inversiones-de-dependencia.md        04-evidencia/historico/fase3-sustituciones/Inversiones-de-dependencia.md
git mv 02-diseno/Verificacion-de-herencias-LSP.md     04-evidencia/historico/fase3-sustituciones/Verificacion-de-herencias-LSP.md
git mv 02-diseno/diagramas/TO-BE-FINAL.puml           04-evidencia/historico/fase3-sustituciones/TO-BE-FINAL.puml
git mv 02-diseno/diagramas/fase3\ uml\ 1.png          04-evidencia/historico/fase3-sustituciones/fase3-uml-1.png
git mv 02-diseno/diagramas/fase3\ uml\ 2.png          04-evidencia/historico/fase3-sustituciones/fase3-uml-2.png
git mv 02-diseno/diagramas/fase3\ uml\ 3.png          04-evidencia/historico/fase3-sustituciones/fase3-uml-3.png
git mv 02-diseno/diagramas/fase3\ uml\ 4.png          04-evidencia/historico/fase3-sustituciones/fase3-uml-4.png
```

## 3. Archivos a ELIMINAR (opcional)

| Origen | Razón |
|---|---|
| `02-diseno/diagramas/uml estado actual link` | Atajo vacío o a una imagen ya archivada. Verificar contenido antes de eliminar. |

## 4. Documentos que NO requieren acción

- `02-diseno/adr/` (vacío).
- `04-evidencia/adr/ADR-001..005.md` (conservados tal cual; el usuario confirmó mantenerlos).
- `04-evidencia/TO-BE-FINAL-ENRICHED.puml` (conservado como precedente detallado; el PUML canónico vive en `diseno/diagramas/TO-BE.puml`).
- `docs/ADRs para la Refactorización del Dominio de Hacienda.md` (ADR ранний, no canónico).
- `docs/Auditoria-SOLID-consolidada.md` (auditoría previa, no canónica).
- `docs/solid/*` (templates/workflow del agente, no entregable académico).

## 5. Estado final esperado de `02-diseno/`

```
02-diseno/
├── DISENO-TO-BE.md                ← consolidado (movido desde 04-evidencia/diseno/)
├── TRAZABILIDAD-ADR.md            ← cadena (movido desde 04-evidencia/diseno/)
├── LIMPIEZA-02-diseno.md          ← este manifiesto (movido desde 04-evidencia/diseno/)
├── diagramas/
│   └── TO-BE.puml                 ← canónico (movido desde 04-evidencia/diseno/)
└── adr/                            ← queda para uso futuro; los 5 ADR siguen en 04-evidencia/adr/
```

## 6. Disclaimer

Este agente:

- NO tiene permisos de escritura sobre `02-diseno/**`.
- NO ejecuta `git mv` desde `04-evidencia/` ni desde ningún path.
- NO borra archivos.

El usuario (o un agente con permisos de `02-diseno/**`, por ejemplo `phase4-finisher`) debe ejecutar las acciones descritas.
