 # SOLID C# Engineering Rules

## Mission

Analyze and improve existing C# code through evidence-based SOLID reasoning, behavior-preserving refactoring, explicit architectural boundaries, and executable verification.

## Mandatory workflow

1. Discover repository structure, solution files, projects, frameworks, entry points, tests, public APIs, dependency direction and current build status.
2. Establish a baseline before proposing edits. Do not attribute pre-existing failures to a refactor.
3. Separate confirmed violations from risks, smells and style preferences.
4. Audit SRP, OCP, LSP, ISP and DIP independently before consolidating.
5. Produce a minimal, ordered and reversible refactoring plan.
6. Add or identify characterization tests before changing behavior-sensitive code.
7. Apply one coherent slice at a time. Build and test after every slice.
8. Perform an adversarial review for semantic drift, overengineering, public API breaks and new SOLID violations.
9. Report evidence, commands executed, results, remaining uncertainty and deferred risks.

## Non-negotiable analysis rules

- A long class is not automatically an SRP violation. Identify independent actors or change drivers.
- A `switch` or `if` is not automatically an OCP violation. Demonstrate repeated modification pressure caused by a variation axis.
- Inheritance syntax is not sufficient for LSP. Check substitutability, preconditions, postconditions, invariants, exceptions and meaningful behavior.
- A large interface is not automatically an ISP violation. Identify clients forced to depend on operations they do not use.
- Dependency injection is not automatically DIP. Verify that high-level policy depends on an abstraction expressed in policy terms and that details implement it.
- Do not create an interface for every class. Introduce abstractions only at a real variation, testing or architectural boundary.
- Prefer composition over inheritance when the "is-a" contract is not behaviorally valid.
- Preserve externally observable behavior unless the user explicitly authorizes a behavior change.
- Do not add packages, frameworks, analyzers or patterns without a demonstrated need.
- Do not rewrite an entire subsystem when a smaller slice removes the verified risk.

## C# verification

Auto-detect the repository's target framework and test framework. Prefer existing commands and conventions. Typical checks are:

```bash
dotnet restore
dotnet build
dotnet test
dotnet format --verify-no-changes
```

Do not assume all commands are available or appropriate. Record skipped checks and the reason.

## Required finding format

Every finding must include:

- ID and principle.
- Status: confirmed, suspected, false positive or accepted trade-off.
- Severity and confidence.
- Exact location.
- Concrete evidence.
- Violated contract or change pressure.
- Affected client, actor or module.
- Consequence if left unchanged.
- Smallest viable refactor.
- Regression tests required.
- Cross-principle effects and trade-offs.

Use `docs/solid/finding-schema.md` and `docs/solid/report-template.md`.
