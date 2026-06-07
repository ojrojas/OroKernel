# Research: Repository Audit - OroKernel

## Technology Decisions

### Decision: .NET SDK 10.0 (net10.0, C# 13)
**Rationale**: Already configured via `global.json` (10.0.102, allowPrerelease) and all project files target `net10.0`. No change needed.
**Alternatives considered**: None — SDK version is fixed by project configuration.

### Decision: xUnit 2.9.3 + Moq 4.20.72 for testing
**Rationale**: Already established in the repository. xUnit is the standard .NET testing framework; Moq provides mocking. Both are well-maintained.
**Alternatives considered**: NUnit (not adopted), FluentAssertions (could be added but not required for baseline).

### Decision: Coverlet 10.0.0 for code coverage
**Rationale**: Already configured in `Directory.Packages.props`. Integrates with `dotnet test --collect:"XPlat Code Coverage"`.
**Alternatives considered**: None — already configured and standard.

### Decision: EF Core 10.0.x InMemory for unit tests
**Rationale**: Already used across all test files. Appropriate for unit testing query/audit logic without a real database.
**Alternatives considered**: SQLite in-memory (used in one example, heavier for unit tests).

## Architecture Decisions

### Decision: Keep existing layered architecture (Data, Entities, Events, Interfaces, Options, Services)
**Rationale**: Clean separation of concerns. Each folder serves a distinct purpose. Rename `Entites/` to `Entities/` for consistency.
**Alternatives considered**: Merging folders (rejected — reduces cohesion).

### Decision: Keep marker interfaces but document their purpose
**Rationale**: `IAggregateRoot`, `IAuditableEntity` provide semantic intent even without methods. `IDomainEventHandler<T>` is truly empty and should either be implemented or removed.
**Alternatives considered**: Remove empty interfaces entirely (breaking change if consumers depend on them).

### Decision: Address unused domain event dispatcher infrastructure
**Rationale**: `IDomainEventDispatcher` has a `DispatchAsync` method but no implementation. Either implement or document as future work.
**Alternatives considered**: Remove interface (breaking change for consumers who reference it).

## Security Decisions

### Decision: Hardcoded example URL in README is acceptable as example placeholder
**Rationale**: Clearly documented as `https://identity.example/` — a reserved domain for documentation. Add a note that production deployments must configure a real URL.
**Alternatives considered**: Remove from README (reduces usefulness of example code).

### Decision: No CI/CD pipeline to create
**Rationale**: Out of scope for this feature phase. Document as missing infrastructure.
**Alternatives considered**: Create GitHub Actions workflow (scope expansion — belongs in a separate feature).

### Decision: Keep `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore` in Shared.csproj
**Rationale**: Provides EF Core error details in development. Has no runtime cost without middleware registration.
**Alternatives considered**: Move to a conditional package reference (premature optimization).

## Testing Decisions

### Decision: 90% business logic coverage target
**Rationale**: Per constitution Section IV. Current coverage is moderate (~60-70%) — achievable with additional unit tests for untested methods.
**Alternatives considered**: 80% (too low for a library), 100% (impractical for edge cases).

### Decision: Create integration tests for AuditableDbContext
**Rationale**: The double `SaveChangesAsync` pattern and audit trail logic are integration-sensitive and not fully covered by existing unit tests.
**Alternatives considered**: Only unit tests (misses integration-level behavior).

## Dependency Decisions

### Decision: Remove unused `OroCQRS` v1.0.0 from Directory.Packages.props
**Rationale**: Declared but not referenced by any `.csproj`. Removing reduces clutter and avoids confusion.
**Alternatives considered**: Keep as placeholder (signal intent — but unclear intent without usage).

### Decision: No nuget.config to create
**Rationale**: Default NuGet sources (`nuget.org`) suffice. Only needed if private feeds are required.
**Alternatives considered**: Create with explicit nuget.org source (redundant — already default).
