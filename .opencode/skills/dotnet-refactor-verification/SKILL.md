---
name: dotnet-refactor-verification
description: Verify C# refactors with baseline-aware dotnet build, test, format and targeted behavioral checks while separating pre-existing failures from regressions.
license: MIT
compatibility: opencode
metadata:
  language: csharp
  workflow: verification
---

# .NET refactor verification

## Detect first

Find solution/project entry point, SDK version, target frameworks, test projects and repository-specific scripts. Use existing CI commands when available.

## Baseline

Record command, exit code, warning/error counts and failing tests before edits. Never claim success merely because the post-refactor state matches an already broken baseline; state both.

## Verification ladder

1. Compile the smallest affected project.
2. Run targeted tests for affected behavior.
3. Run the full relevant test suite.
4. Verify formatting only when configured or requested.
5. Inspect the diff for public API, dependency and behavior changes.

Typical commands:

```bash
dotnet build <solution-or-project>
dotnet test <test-project> --no-restore
dotnet test <solution>
dotnet format <solution> --verify-no-changes
```

Adjust to the repository. Do not force `--no-restore` when restore has not occurred.

## Report

List exact commands, outcomes, pre-existing failures, regressions, skipped checks, coverage gaps and residual risk.
