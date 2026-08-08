---
name: solid-reporting
description: Produce consistent, deduplicated and decision-ready reports for C# SOLID audits, refactoring plans and verification results.
license: MIT
compatibility: opencode
metadata:
  workflow: reporting
  language: csharp
---

# SOLID reporting

Use the schema in `docs/solid/finding-schema.md`.

## Severity

- **Blocker:** likely correctness, data integrity or runtime substitutability failure.
- **High:** severe change propagation, architectural inversion or fragile core behavior.
- **Medium:** demonstrated maintainability/testability cost with bounded impact.
- **Low:** localized design debt with limited current consequence.

Severity is not principle importance. Confidence is separate from severity.

## Consolidation

Merge findings that share the same root cause. Record all affected principles but choose one primary principle. Preserve disagreements between specialists. Do not inflate counts by reporting the same dependency as SRP, OCP and DIP separately.

## Decision language

Use confirmed, suspected, false positive and accepted trade-off. Provide evidence and uncertainty. Avoid absolute claims such as "this is bad design" without a concrete contract or change scenario.
