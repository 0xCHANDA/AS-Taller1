---
name: csharp-codebase-map
description: "Map an existing C# repository before analysis: solutions, projects, references, frameworks, entry points, public APIs, tests and composition roots."
license: MIT
compatibility: opencode
metadata:
  language: csharp
  workflow: discovery
---

# C# codebase mapping

Collect:

- `.sln`, `.slnx`, `.csproj`, `Directory.Build.*`, `global.json` and package management files.
- Target frameworks, nullable settings, implicit usings, analyzers and language version.
- Project references and package references.
- Executable entry points, ASP.NET startup/composition roots, hosted services and dependency registration.
- Domain/application/infrastructure/UI boundaries inferred from code, not names alone.
- Public types and interfaces likely to be externally consumed.
- Test projects, frameworks and integration-test infrastructure.
- Current git state and build/test baseline.

Return a concise map, dependency arrows, risky cycles, unknowns and scopes suitable for parallel SOLID audits. Do not propose refactors during mapping.
