# OpenCode SOLID C# + Phase 4 Workbench

Entorno multiagente para analizar principios SOLID, diseñar refactorizaciones y mejorar código C# existente sin alterar comportamiento de forma accidental.

## Objetivo

El sistema separa descubrimiento, auditoría, planificación, implementación y verificación. Solo `refactor-implementer` puede modificar código de producción. Los auditores son de solo lectura para impedir soluciones contradictorias y refactorizaciones impulsivas.

## Componentes

- **1 agente principal:** `solid-orchestrator`.
- **12 subagentes:** cartografía, cinco auditores SOLID, arquitectura, planificación, implementación, evidencia de Fase 4, pruebas y revisión adversarial.
- **11 skills:** protocolo común, una skill independiente por principio, arquitectura, refactorización, pruebas y reporte.
- **11 comandos:** auditoría integral, auditorías individuales, planificación, aplicación y verificación.

## Instalación en un repositorio C#

Desde la carpeta descomprimida:

```bash
./scripts/install.sh /ruta/al/repositorio
```

O copie manualmente `AGENTS.md`, `opencode.jsonc`, `.opencode/` y `docs/solid/` a la raíz del repositorio.

Luego, desde el repositorio:

```bash
opencode
```

En OpenCode:

```text
/connect
/models
```

El paquete usa routing multimodelo explícito en cada agente, con `openai/gpt-5.6-sol` como modelo principal y `opencode-go/deepseek-v4-flash` para tareas ligeras. Conecte los proveedores requeridos mediante `/connect` y confirme los identificadores configurados con `/models`; no sustituya silenciosamente un modelo si todavía no está disponible en la sesión.

## Fase 4 autónoma

La configuración nocturna congela la arquitectura de Fase 3 y separa dos writers: `refactor-implementer` para producción NEW y `phase4-evidence-engineer` para caracterización/evidencia. Todos los demás agentes son read-only; no hay preguntas, web, acceso externo, delegación de subagentes ni comandos Bash libres.

La guía operativa y los comandos exactos están en [docs/solid/PHASE4-OVERNIGHT.md](docs/solid/PHASE4-OVERNIGHT.md). Antes de dormir, el flujo esperado es:

```bash
scripts/validate-phase4-workbench.sh
scripts/prepare-phase4-worktree.sh
# cambiar al WORKTREE_PATH impreso
scripts/start-phase4-tmux.sh
```

El runner nunca hace push. Los builds se aíslan en una copia temporal para que los `bin/obj` rastreados no contaminen diffs ni métricas.

## Flujo recomendado

1. `/solid-audit` para obtener diagnóstico completo sin editar.
2. Revisar hallazgos confirmados y descartar falsos positivos.
3. `/solid-plan <alcance>` para producir una secuencia de refactorización reversible.
4. `/solid-apply <alcance aprobado>` para implementar en slices pequeños.
5. `/solid-verify <alcance>` para compilar, probar y realizar revisión adversarial.

Para un archivo o clase concreta:

```text
/solid-audit-file src/Domain/Order.cs
/solid-srp src/Domain/Order.cs
/solid-lsp src/Domain/Vehicle.cs
```

## Principio operativo

SOLID no es una cuota de interfaces ni una excusa para multiplicar clases. Una modificación solo se acepta cuando reduce un riesgo demostrado, conserva contratos y deja evidencia verificable.

## Hacienda — ejecución final de Fase 4

Todos los comandos usan el wrapper aislado del repositorio:

```bash
# Compilar y arrancar NEW
scripts/phase4-safe-dotnet.sh build 03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj
scripts/phase4-safe-dotnet.sh web-smoke 03-src/redisenado/HaciendaNEW/p_mvcHacienda/p_mvcHacienda.csproj

# Verificador final
scripts/phase4-safe-dotnet.sh run 03-src/redisenado/HaciendaNEW/HaciendaNEW.Verification/HaciendaNEW.Verification.csproj

# Caracterización OLD primero y NEW después
scripts/phase4-safe-dotnet.sh run 03-src/phase4/Characterization/Old/Characterization.Old.csproj
scripts/phase4-safe-dotnet.sh run 03-src/phase4/Characterization/New/Characterization.New.csproj

# Demo de flujo normal y SC-1 (Lacteo, Carne y Piel)
scripts/phase4-safe-dotnet.sh run 03-src/redisenado/HaciendaNEW/HaciendaNEW.Demo/HaciendaNEW.Demo.csproj
```

Evidencia: `04-evidencia/PHASE4-REPORT.md`, `04-evidencia/characterization/`, `04-evidencia/SC1-METRICA-OCP.md`, `04-evidencia/TOBE-CODE-MATRIX.md` y `04-evidencia/BITACORA-IA.md`.
