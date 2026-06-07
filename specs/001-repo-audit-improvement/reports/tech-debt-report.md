# Technical Debt Report

| ID | Item | Type | Effort | Status |
|----|------|------|--------|--------|
| TD-001 | Directory name typo `Entites` → `Entities` | Naming | 5m | Fixed |
| TD-002 | Filename typo `IIBussines` → `IBusinessRule` | Naming | 2m | Fixed |
| TD-003 | Remove empty `UnitTest1.cs` | Dead code | 2m | Pending (T026) |
| TD-004 | Unused `OroCQRS` dependency | Dead code | 2m | Pending (T025) |
| TD-005 | Double `SaveChangesAsync` in audit | Architecture | 2h | Open |
| TD-006 | Empty marker interfaces (`IAggregateRoot`, etc.) | Dead code | 30m | Accepted |
| TD-007 | Missing `IDomainEventDispatcher` implementation | Architecture | 4h | Open |
| TD-008 | No integration tests for `AuditableDbContext` | Testing | 4h | Pending (T028) |
| TD-009 | Missing unit tests for `Error.cs`, `Result.cs`, `RetryDelegatingHandler.cs` | Testing | 2h | Pending (T027) |
| TD-010 | Incomplete `LICENSE` file | Documentation | 10m | Pending (T030) |

## Total Estimated Effort: ~13 hours remaining
