# Research: Business Rules & Specification Pattern Examples

## Unknowns Resolved

No [NEEDS CLARIFICATION] markers existed in the spec. All decisions were informed by existing codebase conventions.

## Pattern Research

### IBusinessRule Usage Patterns

The `IBusinessRule` interface (`src/Shared/Interfaces/IBusinessRule.cs`) defines:

```csharp
public interface IBusinessRule
{
    bool IsSatisfied();
    Error? Error { get; }
}
```

**Common implementation pattern**: Each rule takes its dependencies via constructor, implements `IsSatisfied()` to check the condition, and sets `Error` to a descriptive `Error` object when the rule fails.

**Naming convention**: `[Condition]Rule.cs` (e.g., `UserEmailMustBeUniqueRule`).

### BaseSpecification<T> Usage Patterns

The `BaseSpecification<T>` class (`src/Shared/Specification/BaseSpecification.cs`) provides:

- `ToExpression()` → returns `Expression<Func<T, bool>>` for LINQ queries
- `IsSatisfiedBy(T entity)` → evaluates against a single entity
- Combinators: `.And(other)`, `.Or(other)`, `.Not()`

**Naming convention**: `[Filter]Specification.cs` (e.g., `ActiveUserSpecification`).

### Codebase Conventions

| Convention | Standard |
|---|---|
| Namespaces | `UserManagement.*` for example code |
| Test assertions | xUnit `Assert.*` (no FluentAssertions) |
| Mocking | Moq with `Mock<T>` |
| Project structure | Flat (non-DDD) for the basic UserManagement example |

## Decisions

- **Decision**: Add example code to `examples/UserManagement/` (not DDD variant) — it is the simpler, more accessible example project
- **Decision**: Use xUnit `Assert.*` for tests (consistent with existing test suite)
- **Decision**: Business rules will use `Error` from `OroKernel.Shared.Entities` for failure results
- **Decision**: Specifications will filter in-memory `List<User>` collections — no EF Core query translation needed for examples
