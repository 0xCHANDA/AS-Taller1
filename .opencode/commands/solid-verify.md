---
description: Verify a SOLID refactor and perform adversarial review
agent: solid-orchestrator
subtask: false
---


Verify the refactor in `$ARGUMENTS` or the current diff. Invoke `test-guardian` and `adversarial-reviewer`. Compare against baseline, inspect public contracts and dependency direction, and report regressions, overengineering, remaining violations and residual uncertainty. Do not make corrective edits unless explicitly requested.
