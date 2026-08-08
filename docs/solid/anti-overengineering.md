# Anti-overengineering gates

Reject or simplify a proposed refactor when any answer is "no":

1. Is there a confirmed problem with exact evidence?
2. Is the affected behavior or change pressure concrete?
3. Is the proposed abstraction more stable than its implementations?
4. Is there at least one real client, variant or boundary that benefits?
5. Can the change be applied in a small reversible slice?
6. Can behavior be protected by tests or direct verification?
7. Is the new design simpler in the relevant change scenario?
8. Does it avoid leaking infrastructure into business policy?
9. Does it preserve meaningful subtype and interface contracts?
10. Is the complexity cost proportionate to current requirements?

Common rejected moves:

- Interface per class.
- Repository wrapper over an ORM without a policy boundary.
- Factory for a single stable construction path.
- Strategy for a closed two-branch condition that does not change.
- Microservice extraction to solve a class-level responsibility problem.
- Base class introduced only to remove duplicated lines.
- Service locator disguised as dependency injection.
