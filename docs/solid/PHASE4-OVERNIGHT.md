# Phase 4 Autonomous Workbench

Este documento describe el workbench preparado para ejecutar Fase 4 de forma autónoma. No es evidencia de que Fase 4 ya haya sido ejecutada.

## Estado técnico de entrada

- OpenCode validado: `1.18.1`.
- Source NEW: SDK-style `net8.0`.
- MVC NEW: `ProjectReference` a Bib NEW.
- Verifier: `03-src/redisenado/HaciendaNEW/HaciendaNEW.Verification`.
- Runtime disponible: .NET 10; runtime 8 ausente. Los wrappers usan `DOTNET_ROLL_FORWARD=Major`.
- Existen 284 archivos `bin/obj` rastreados. Los builds nocturnos se realizan sobre una copia temporal para no contaminar Git ni métricas.
- `bubblewrap` (`bwrap`) está instalado y es obligatorio: monta el repositorio real read-only, deshabilita red y deja writable solo la copia temporal.
- El blocker histórico de ventas genéricas está cerrado en el source actual y cubierto por el verifier. Debe revalidarse en baseline, no asumirse.

## Aislamiento de writers

`refactor-implementer` es el único writer de producción y solo puede editar:

- `03-src/redisenado/HaciendaNEW/Bib_Hacienda/**`
- `03-src/redisenado/HaciendaNEW/p_mvcHacienda/**`

`phase4-evidence-engineer` solo puede editar:

- rutas `Verification`/`HaciendaNEW.Verification`;
- rutas `Characterization`;
- `04-evidencia/**`.

Los auditores, planner, cartographer, guardian y adversarial reviewer son read-only. Ningún subagente puede delegar. Todos tienen `question`, web, external directory y doom loop en `deny`.

El runner usa `opencode --pure`; por tanto, el plugin global de notificaciones detectado en la instalación no se carga durante la ejecución nocturna.

## Gates

1. Baseline reproducible en worktree nocturno limpio.
2. Ocho o más casos reales establecidos primero contra OLD.
3. Los mismos casos contra NEW, con comparación explícita.
4. Diferencias clasificadas antes de editar producción.
5. SC-1 vertical implementada con slice quirúrgico si todavía falta algo.
6. Build/verifier independiente.
7. Demo, métrica reproducible y evidencia.
8. Correspondencia TO-BE↔código, build completo, caracterización completa y revisiones finales.

## Métricas

La medición de SC-1 debe conservar tres estados: baseline OLD documentado, NEW PRE-SC y NEW POST-SC. Se cuentan por separado archivos/clases existentes modificados y archivos/clases nuevos. Se excluyen pruebas, evidencia, `bin`, `obj`, logs y generados. Los conteos deben derivarse de listas exactas, no de memoria del agente.

## Git y builds

- `prepare-phase4-worktree.sh` exige baseline limpio y crea un worktree hermano con branch `agent/phase4-overnight-YYYYMMDD-HHMM`.
- `phase4-safe-dotnet.sh` copia `03-src` a un directorio temporal, ejecuta sin restore y comprueba que el estado real de Git no cambió.
- `phase4-checkpoint.sh` es la única vía de commit local: exige branch nocturna, scope explícito, gate PASS y ausencia de generados/logs/secrets.
- No hay push automático.

## Estrategia de tokens y costo

SEARCH BEFORE READ. El cartographer crea un único PHASE4 EVIDENCE PACK compacto. El implementer recibe solo el slice. Los auditores incrementales se invocan únicamente por principio afectado; la auditoría SOLID completa ocurre una vez al final. El snapshot de `opencode stats` queda en el directorio único de logs de cada corrida.

## Comandos de preparación

Desde un checkout limpio:

```bash
scripts/validate-phase4-workbench.sh
scripts/phase4-safe-dotnet.sh build 03-src/redisenado/HaciendaNEW/HaciendaNEW.Verification/HaciendaNEW.Verification.csproj
scripts/phase4-safe-dotnet.sh run 03-src/redisenado/HaciendaNEW/HaciendaNEW.Verification/HaciendaNEW.Verification.csproj
scripts/prepare-phase4-worktree.sh
```

Luego, dentro del `WORKTREE_PATH` impreso:

```bash
scripts/validate-phase4-workbench.sh
scripts/start-phase4-tmux.sh
```

Sin tmux:

```bash
scripts/run-phase4-overnight.sh
```

## Interpretación de blockers

`PROVIDER_BLOCKED`, `TOOL_BLOCKED` y `SLICE_BLOCKED` son blockers locales. El run continúa con trabajo independiente. Solo las condiciones de parada global descritas en `.opencode/phase4-overnight.md` detienen toda la ejecución. Las decisiones que requieren juicio del equipo se registran como `PENDING_HUMAN_REVIEW`.
