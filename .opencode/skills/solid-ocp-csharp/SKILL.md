---
name: solid-ocp-csharp
description: Audit and refactor C# extension points for Open/Closed Principle using demonstrated variation axes, stable contracts, polymorphism and composition.
license: MIT
compatibility: opencode
metadata:
  principle: ocp
  language: csharp
---

# OCP for C#

## Course-aligned rule

Software elements should be open for extension and closed for modification. New functionality should usually be added through new code rather than repeated edits to stable code. Correct defects in the defective base component; do not hide a base-class bug in a subclass.

## Confirm a variation axis

Identify what changes independently: pricing rule, payment method, export format, notification channel, validation policy, storage provider or algorithm. Find stable client behavior and repeated edits caused by adding variants.

## Strong indicators

- Every new variant requires modifying the same dispatcher and several unrelated files.
- Stable business logic contains type checks for an extensible family.
- Adding one implementation requires changing consumers that should depend only on a contract.
- A plugin or policy boundary is expected but concrete details leak into the core.

## Not sufficient alone

A finite `switch`, enum, pattern match or conditional. They may be clearest when the set is closed, local and stable. Avoid polymorphism that obscures a genuinely fixed decision table.

## Refactoring options

Strategy, polymorphic command, composition, registry, factory at the composition root, delegates for small algorithms, or an interface owned by the stable policy. Keep construction details outside business logic.

## Required proof

State the extension scenario, the stable component currently modified, prior or probable repeated edits, and why the proposed abstraction is more stable than its implementations.
