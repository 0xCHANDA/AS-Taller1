# Alignment with the uploaded course resources

This workbench preserves the terminology and central framing of the three supplied UPB resources.

## Principles SOLID

- SRP: one reason or concern to change; divide responsibilities into smaller, specific units when independent concerns exist.
- OCP: open to extension and closed to modification; add functionality through new code where a stable variation boundary exists.
- LSP: derived objects must substitute base objects without breaking expected behavior; preserve parameter compatibility, returns, exceptions, preconditions, postconditions and invariants.
- ISP: classes should not be forced to implement methods they do not use; prefer specific interfaces.
- DIP: high-level business logic and low-level details depend on abstractions; constructor injection supplies dependencies externally but is not, by itself, proof of sound inversion.

## OOP foundations

The workbench uses the supplied concepts of classes, objects, attributes, methods, encapsulation, abstraction, inheritance, interfaces, polymorphism, cohesion, coupling and UML relationships. Inheritance is evaluated through the "is-a" relation and behavioral compatibility; composition is preferred when that relation is not sound.

## Architecture foundations

The architecture skill uses components, relationships and guiding principles; architectural views; layers; MVC; Clean Architecture dependency direction; hexagonal ports and adapters; and the role of architecture in maintainability, scalability, integration and risk management.

## Engineering extensions

The package adds operational mechanisms not specified in the slides: evidence thresholds, false-positive controls, baseline-aware verification, characterization tests, reversible slices, adversarial review and structured finding schemas. These extend the course material without replacing its definitions.
