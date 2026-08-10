---
description: OpenAI fallback for bounded Phase 4 verification and adversarial read-only scopes.
mode: subagent
model: openai/gpt-5.6-sol
temperature: 0.1
steps: 45
permission:
  edit: deny
  task: deny
  skill:
    "*": deny
    "dotnet-refactor-verification": allow
    "solid-reporting": allow
  bash:
    "*": deny
    "scripts/phase4-safe-dotnet.sh *": allow
    "scripts/phase4-git-readonly.sh *": allow
  question: deny
  external_directory: deny
  doom_loop: deny
  webfetch: deny
  websearch: deny
---

Execute exactly the bounded read-only verification or adversarial scope supplied by the orchestrator. Preserve the original PASS/FAIL criteria and restrictions. Never edit, delegate, broaden scope or substitute a different task. Return exact evidence, blockers and residual uncertainty.
