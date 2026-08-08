---
name: solid-srp-csharp
description: Audit and refactor C# classes and modules for Single Responsibility Principle using actors, reasons to change, cohesion and responsibility boundaries.
license: MIT
compatibility: opencode
metadata:
  principle: srp
  language: csharp
---

# SRP for C#

## Course-aligned rule

A class should have one reason or concern to change and a simple, well-defined responsibility. Splitting into smaller, more specific units can reduce complexity and improve readability, maintenance and extension.

## Analyze by actors and change drivers

List every stakeholder or source of change affecting the type: business policy, persistence schema, presentation format, transport, security, notification, calculation, configuration or external service. Confirm a violation when independent actors force changes to the same unit.

## Strong indicators

- Entity state and business invariants mixed with database or HTTP code.
- Domain calculation mixed with formatting, emailing, logging or file output.
- A coordinator also performs every detailed operation it coordinates.
- Changes requested by unrelated teams repeatedly touch the same class.
- Methods form separate cohesion clusters with few shared invariants.

## Not sufficient alone

Class length, many methods, private helpers, validation in properties, a DTO with several fields, or one class supporting several steps of a single cohesive workflow.

## Refactoring options

Extract a cohesive service, policy, formatter, repository, validator or orchestrator. Keep invariants near the data they protect. Do not turn every method into a class. Define the new boundary by actor and change reason, not noun extraction.

## Required proof

Name at least two independent change drivers, show the code owned by each, and explain why they are not one cohesive responsibility.
