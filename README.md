# OpenCode SOLID C# Multi-Model Workbench

Entorno multiagente para analizar principios SOLID, diseñar refactorizaciones y mejorar código C# existente sin alterar comportamiento de forma accidental.

## Objetivo

El sistema separa descubrimiento, auditoría, planificación, implementación y verificación. Solo `refactor-implementer` puede modificar código de producción. Los auditores son de solo lectura para impedir soluciones contradictorias y refactorizaciones impulsivas.

## Componentes

- **1 agente principal:** `solid-orchestrator`.
- **11 subagentes:** cartografía, cinco auditores SOLID, arquitectura, planificación, implementación, pruebas y revisión adversarial.
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
