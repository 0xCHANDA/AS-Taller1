---
name: solid-lsp-csharp
description: Audit and refactor C# inheritance and interface implementations for Liskov substitution, behavioral contracts, exceptions and invariants.
license: MIT
compatibility: opencode
metadata:
  principle: lsp
  language: csharp
---

# LSP for C#

## Course-aligned rule

An object of a derived type must be usable where its base type is expected without breaking the client's expected behavior. The subtype must remain behaviorally compatible with the supertype.

## Contract checks

- Input requirements are not strengthened.
- Output guarantees are not weakened.
- Base invariants remain true.
- Unexpected exception categories are not introduced.
- Return values remain compatible and meaningful.
- Mutability, ordering, idempotency and side-effect expectations are preserved.
- Overridden members do not become no-ops or throw `NotSupportedException` for valid base operations.

C# enforces much signature compatibility at compile time, so prioritize semantic contracts over syntax.

## Strong indicators

- A subtype throws for an operation promised by the base type.
- Client code must test runtime type before calling a base member.
- A derived type rejects values valid for the base contract.
- Read-only and writable concepts are modeled in the wrong inheritance direction.
- A subtype silently weakens a postcondition or invariant.

## Refactoring options

Move only truly common behavior to the base, split capability interfaces, invert an inheritance relationship, use composition, or model separate abstractions. A base such as `Vehicle` should promise only behavior valid for all vehicles; fuel and battery operations belong to separate capabilities.

## Required proof

Name the substituting type, the base contract, a concrete client call and the behavioral mismatch. Provide or propose a test that runs the same contract suite against all implementations.
