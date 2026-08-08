---
name: solid-analysis-protocol
description: Evidence-first protocol for auditing existing C# code for SOLID violations while controlling false positives, scope and semantic risk.
license: MIT
compatibility: opencode
metadata:
  language: csharp
  domain: code-analysis
---

# SOLID analysis protocol

## Sequence

1. Define scope and user goal.
2. Map relevant types, clients, dependencies, tests and runtime paths.
3. Establish current behavior and build/test baseline.
4. State the candidate principle rule precisely.
5. Gather concrete evidence from code and clients.
6. Attempt to disprove the violation.
7. Classify as confirmed, suspected, false positive or accepted trade-off.
8. Recommend the smallest viable intervention.
9. Identify tests and cross-principle consequences.

## Evidence threshold

A confirmed violation requires all of:

- Exact code location.
- A concrete actor, client, contract or variation axis.
- A credible change or substitution scenario.
- An observable maintenance, correctness or coupling consequence.
- A refactoring whose benefit can be explained without invoking fashion.

## False-positive controls

Do not infer a violation solely from class size, method count, conditionals, concrete types, inheritance, absence of interfaces, static methods, enums, constructors or use of `new`. These can be evidence only when connected to a demonstrated design pressure.

## Scope control

Prefer local analysis first, then follow dependencies only as far as needed to establish behavior. Do not redesign unrelated modules. Report uncertainty instead of guessing.
