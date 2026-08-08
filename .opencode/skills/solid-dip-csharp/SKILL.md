---
name: solid-dip-csharp
description: Audit and refactor C# dependency direction so high-level business policy depends on stable abstractions and technical details implement them.
license: MIT
compatibility: opencode
metadata:
  principle: dip
  language: csharp
---

# DIP for C#

## Course-aligned rule

High-level classes containing business logic should not depend directly on low-level implementations. Both depend on abstractions; abstractions do not depend on details. Constructor injection is a common mechanism for supplying dependencies from outside.

## Distinguish DIP from DI

Constructor injection can reduce construction coupling, but DIP is satisfied only when dependency direction protects high-level policy. An interface that merely mirrors a vendor API or lives conceptually with the detail may not invert anything.

## Strong indicators

- Business policy constructs database, filesystem, HTTP, SMTP, clock or vendor SDK objects internally.
- Domain/application projects reference infrastructure projects.
- High-level methods accept concrete low-level types.
- Service locator or global container access hides dependencies.
- Business terminology is absent from the abstraction while technical details leak through it.

## Healthy structure

Define ports in policy terms near the consumer that owns the need. Implement them in infrastructure. Wire concrete implementations in a composition root. Keep DI containers outside domain logic.

## Not sufficient alone

Any use of `new`. Creating values, entities or internal implementation details is normal. Focus on volatile external dependencies and cross-boundary direction.

## Refactoring options

Constructor injection, policy-owned ports, adapters, factories at the composition root, configuration objects and seams for time or external systems. Do not create one-method interfaces without a real boundary or variation.

## Required proof

Identify high-level policy, low-level detail, current dependency arrow, desired abstraction and the concrete change/testing risk caused by the existing direction.
