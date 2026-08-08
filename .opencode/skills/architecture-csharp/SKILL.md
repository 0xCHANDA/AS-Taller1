---
name: architecture-csharp
description: Analyze C# architecture, component responsibilities, dependency direction, cohesion, coupling, layers, MVC, Clean Architecture and ports/adapters.
license: MIT
compatibility: opencode
metadata:
  language: csharp
  domain: software-architecture
---

# Architecture analysis for C#

## Course-aligned foundation

Software architecture describes system components, their externally visible properties, their relationships and the principles guiding design and evolution. It is a high-level blueprint responsible for major quality attributes.

Relevant structures:

- Layers: presentation, business logic and data access with explicit responsibilities.
- MVC: Model owns data/business logic, View presents, Controller coordinates interaction.
- Clean Architecture: source dependencies point from external technical details toward internal business logic; inner layers know nothing about outer layers.
- Hexagonal architecture: business core exposes ports; adapters translate between the core and external technologies.
- Cohesion and coupling: seek cohesive components and low unnecessary coupling.

## Audit procedure

1. Map projects/namespaces to responsibilities.
2. Build the dependency direction between components.
3. Locate business rules and technical details.
4. Compare actual boundaries with declared architecture.
5. Evaluate change propagation, testability and integration risk.
6. Recommend the smallest boundary correction.

## Guardrails

Architecture is contextual. Do not prescribe microservices, DDD, Clean Architecture or extra layers merely because they exist. A modular monolith may be superior. Do not turn every class into a port or adapter.
