---
name: solid-isp-csharp
description: Audit and refactor C# interfaces for Interface Segregation Principle from concrete client and capability perspectives.
license: MIT
compatibility: opencode
metadata:
  principle: isp
  language: csharp
---

# ISP for C#

## Course-aligned rule

A class should not be forced to implement methods it does not use. Prefer small, specific interfaces over a broad interface that imposes irrelevant operations.

## Analyze clients, not size alone

Build a client-to-member usage matrix. An interface violates ISP when clients are coupled to members irrelevant to them or implementations must provide meaningless, throwing or empty members.

## Strong indicators

- Implementations throw `NotSupportedException` for interface members.
- Empty methods exist only to satisfy a contract.
- A change to one interface capability recompiles or affects unrelated clients.
- Read and write clients depend on a combined interface despite needing only one side.
- Testing requires large mocks with irrelevant setup.

## Not sufficient alone

Many members, inheritance between interfaces, or a broad interface whose clients genuinely need the whole cohesive protocol.

## Refactoring options

Extract role or capability interfaces, let a richer interface inherit smaller ones when appropriate, inject the narrowest required contract, and keep cohesive protocols intact.

## Required proof

Name the client, list members it uses and members it is unnecessarily coupled to, then show the practical change or testing consequence.
