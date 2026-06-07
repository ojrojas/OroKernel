# Code Quality Report

## Summary

| Category | Issues Found | Fixed |
|----------|-------------|-------|
| File/directory naming | 2 | 2 |
| Dead code | 2 | 1 |
| Unused imports | 0 | 0 |
| Large functions (>30 lines) | 0 | 0 |
| Large classes (>200 lines) | 0 | 0 |
| Code duplication | 0 | 0 |
| Anti-patterns | 1 | 0 |
| Weak error handling | 0 | 0 |

## Issues

### CQ-001: Directory name typo

| Field | Value |
|-------|-------|
| **Type** | Naming |
| **Location** | `src/Shared/Entites/` |
| **Description** | Directory named `Entites` instead of `Entities` |
| **Severity** | Minor |
| **Status** | Fixed (renamed to `src/Shared/Entities/`) |

### CQ-002: Filename typo

| Field | Value |
|-------|-------|
| **Type** | Naming |
| **Location** | `src/Shared/Interfaces/IIBussines.cs` |
| **Description** | Filename `IIBussines.cs` instead of `IBusinessRule.cs` |
| **Severity** | Minor |
| **Status** | Fixed (renamed to `src/Shared/Interfaces/IBusinessRule.cs`) |

### CQ-003: Empty placeholder test file

| Field | Value |
|-------|-------|
| **Type** | DeadCode |
| **Location** | `src/Shared.Tests/UnitTest1.cs` |
| **Description** | Empty test file with a single [Fact] that has no assertions |
| **Severity** | Minor |
| **Status** | Open (to be removed in T026) |

### CQ-004: Empty marker interfaces

| Field | Value |
|-------|-------|
| **Type** | DeadCode |
| **Location** | `src/Shared/Interfaces/IAggregateRoot.cs`, `IAuditableEntity.cs`, `IDomainEventHandler.cs` |
| **Description** | Interfaces with no members provide no behavioral contract |
| **Severity** | Minor |
| **Status** | AcceptedRisk — provide semantic intent; removing would be breaking change |

### CQ-005: Unused IDomainEventDispatcher implementation

| Field | Value |
|-------|-------|
| **Type** | Architecture |
| **Location** | `src/Shared/Interfaces/IDomainEventDispatcher.cs` |
| **Description** | `DispatchAsync` method defined but no implementation exists in the repository |
| **Severity** | Major |
| **Status** | Open — requires implementation or documentation as future work |
