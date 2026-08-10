# Phase 4 overnight runtime contract

`USER_IS_UNAVAILABLE=true`

This run prepares and executes Phase 4 implementation/evidence only. Phase 3 architecture is frozen. Do not ask questions, redesign architecture, generate UML, install packages, use the network, or perform destructive Git operations.

## Non-negotiable rules

- SEARCH BEFORE READ: exact symbol search, then relevant line window. Do not reread complete academic documents.
- Preserve the project's Spanish naming, simple student-level C#, observable strings and existing abstraction ceiling.
- Use one compact PHASE4 EVIDENCE PACK containing only repo map, build commands, OLD/NEW paths, relevant symbols, public contracts, TO-BE inventory, selected SC, characterization inventory, latest diff and blockers.
- `codebase-cartographer` runs once normally. Planner gets one normal plan and at most one narrower replan.
- `refactor-implementer` is the only production writer and receives only one fully specified slice at a time.
- `phase4-evidence-engineer` is the only characterization/evidence writer and cannot edit production.
- Provider fallback never changes authority: `refactor-implementer-fallback` mirrors the production writer exactly; `phase4-evidence-engineer-fallback` mirrors the evidence writer exactly; `phase4-readonly-fallback` remains read-only without delegation.
- Implementer never verifies itself. Auditors, guardian and adversarial reviewer are read-only and cannot delegate.
- SC-1 Hacienda (derived cattle products) is selected unless repository evidence unequivocally shows another formal team choice. If human confirmation is missing, record `PENDING_HUMAN_REVIEW` without blocking safe work.

## Workflow and gates

1. PHASE4 BASELINE: before any long session, require explicit `OLD_CAN_BUILD`, `OLD_CAN_EXECUTE`, `NEW_CAN_BUILD` and `NEW_CAN_EXECUTE` PASS results from `scripts/phase4-preflight.sh`, using the Bubblewrap-safe wrapper. Then verify the NEW verifier and tracked generated files. Stop globally if unreproducible.
2. CARTOGRAPHER ONCE: create/update the compact evidence pack.
3. PHASE4 EVIDENCE PACK: freeze paths, contracts, TO-BE inventory, baseline and blockers.
4. PLANNER: define minimal reversible slices; no second design phase.
5. GATE 1 — OLD CHARACTERIZATION: establish at least eight real OLD scenarios before production changes.
6. GATE 2 — NEW CHARACTERIZATION + COMPARISON: same inputs/operations; record MATCH/MISMATCH and evidence paths.
7. BEHAVIOR DIFFERENCE TRIAGE: authorized SC difference, genuine non-semantic noise, or blocker. Never normalize semantic drift.
8. GATE 3 — SC-1 VERTICAL: domain → inventory/sale → persistence where behavior requires → demo/verifier. Treat generic-sale persistence loss as part of the vertical if reproduced.
9. TARGETED BUILD + TEST using `scripts/phase4-safe-dotnet.sh`.
10. Incremental OCP/DIP/architecture audits only for principles affected by the slice.
11. GATE 4 — DEMO + METRICS + EVIDENCE.
12. TO-BE ↔ CODE consistency in both directions.
13. Full final safe build, full characterization, one final SOLID audit, Test Guardian and adversarial review.
14. PHASE 4 REPORT with commands, outputs, evidence paths, risks and `PENDING_HUMAN_REVIEW` entries.

## Retry policy

- Transient command: one retry.
- Agent failure: one retry with narrower scope.
- For a preferred OpenCode Go agent, retry the identical call at most once.
- On `PROVIDER_BLOCKED`, do not try other models from OpenCode Go. Invoke the mapped OpenAI fallback exactly once with the identical slice/scope, PASS/FAIL criteria and restrictions.
- Mapping: `refactor-implementer` → `refactor-implementer-fallback`; `phase4-evidence-engineer` → `phase4-evidence-engineer-fallback`; `test-guardian` or `adversarial-reviewer` → `phase4-readonly-fallback`.
- If that fallback fails, record `BLOCKED` and continue independent safe work.
- Identical doom-loop call: zero retries.
- Then emit `PROVIDER_BLOCKED`, `TOOL_BLOCKED` or `SLICE_BLOCKED` and continue independent safe work.

## Global stop conditions

Stop the whole run only if the Fase 3 baseline is not reproducible; branch/worktree is wrong; changes appear outside the workspace; change attribution is lost; observable behavior needs an unauthorized change; a critical dependency must be installed; the repository appears corrupt; safe rollback/checkpointing is unavailable; or multiple providers leave no safe independent work. A local blocker does not stop unrelated work.

## Git and evidence

- No push, merge, rebase, reset, clean, stash, switch, global Git config or direct commit.
- Local checkpoints are allowed only through `scripts/phase4-checkpoint.sh`, only on `agent/phase4-overnight-*`, with explicit scope and a PASS gate file.
- Build/test only through the safe wrapper so tracked `bin/obj` and production metrics remain uncontaminated.
- AI log entries are candidates, not invented team decisions.
