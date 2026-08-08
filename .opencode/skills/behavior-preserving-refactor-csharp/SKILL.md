---
name: behavior-preserving-refactor-csharp
description: Plan and execute small reversible C# refactors that preserve observable behavior and public contracts while improving SOLID boundaries.
license: MIT
compatibility: opencode
metadata:
  language: csharp
  workflow: refactoring
---

# Behavior-preserving C# refactoring

## Preconditions

- Confirm the finding and intended benefit.
- Capture build/test baseline.
- Identify observable behavior and public contracts.
- Add characterization tests when coverage is insufficient.
- Define a rollback point.

## Slice rules

One slice should have one design intention, a reviewable diff and an independent verification step. Prefer mechanical moves before semantic changes: extract member, move type, introduce parameter, redirect construction at the composition root, then delete obsolete paths only after verification.

## C# considerations

Preserve nullability contracts, accessibility, async behavior, cancellation, disposal ownership, serialization shape, EF Core conventions, ASP.NET registration and exception behavior unless explicitly changing them. Avoid public API expansion by default. Prefer `internal` abstractions when the boundary is internal.

## Stop conditions

Stop when tests regress unexpectedly, a new dependency is required, the approved scope expands, behavior is ambiguous, generated code would be edited, or a public contract must change without authorization.

## Anti-overengineering

Do not add patterns, factories, repositories or interfaces unless they remove a demonstrated coupling or variation problem. Fewer clearer abstractions beat pattern density.
