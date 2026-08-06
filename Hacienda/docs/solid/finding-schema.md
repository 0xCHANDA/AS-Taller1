# Finding schema

```yaml
id: SOLID-<PRINCIPLE>-NNN
primary_principle: SRP | OCP | LSP | ISP | DIP | ARCH
related_principles: []
status: confirmed | suspected | false-positive | accepted-trade-off
severity: blocker | high | medium | low
confidence: 0-100
scope: project/namespace/type/member
locations:
  - path: path/to/File.cs
    symbol: Namespace.Type.Member
    lines: start-end
evidence:
  - concrete observation
contract_or_change_pressure: precise rule or scenario
actor_or_client: affected stakeholder/type/module
consequence: current or credible impact
counterevidence: evidence against the finding
recommendation: smallest viable refactor
required_tests:
  - behavior to protect
tradeoffs:
  - cost or downside
acceptance_criteria:
  - verifiable completion condition
```

A location without a contract/change scenario is not a confirmed finding.
