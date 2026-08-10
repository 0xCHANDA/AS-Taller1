---
description: Execute the autonomous Phase 4 implementation/evidence workflow
agent: solid-orchestrator
subtask: false
---

Load `.opencode/phase4-overnight.md` and execute it with `USER_IS_UNAVAILABLE=true`.

Start with the baseline and branch/worktree gates. Do not execute if the current branch is not `agent/phase4-overnight-*`. Do not ask questions. Use only versioned hard-deny permissions, the safe build/Git wrappers and the two isolated writers. Produce the final Phase 4 report under `04-evidencia`.
