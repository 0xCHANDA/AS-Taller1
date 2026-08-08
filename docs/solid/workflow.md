# Multiagent workflow

```text
User request
    |
    v
solid-orchestrator
    |
    +--> codebase-cartographer
    |
    +--> srp-auditor -----+
    +--> ocp-auditor -----+
    +--> lsp-auditor -----+--> consolidation --> refactor-planner
    +--> isp-auditor -----+                         |
    +--> dip-auditor -----+                         v
    +--> architecture-auditor              refactor-implementer
                                                     |
                                                     +--> test-guardian
                                                     +--> adversarial-reviewer
```

## Why one writer

Parallel auditors improve recall because each principle has different evidence. Parallel writers are dangerous: they can introduce conflicting abstractions, edit the same contracts and make verification ambiguous. Therefore only `refactor-implementer` writes production code.

## Why independent audits

SRP studies change drivers; OCP studies variation axes; LSP studies behavioral substitutability; ISP studies client-specific contracts; DIP studies dependency direction. A single generic reviewer tends to collapse these into vague "clean code" commentary.
