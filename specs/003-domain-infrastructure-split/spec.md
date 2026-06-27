# Spec 003: Domain/Infrastructure Split + Value Objects as Records + Multi-target .NET 10/11

**Date**: 2026-06-26  
**Status**: Implemented  
**Version**: 2.0.0 (breaking change from 1.0.2)

## Motivation

The original `OroKernel.Shared` library mixed domain primitives (entities, value objects, interfaces, specifications) with infrastructure concerns (EF Core `AuditableDbContext`, audit entities, HTTP services, `IHttpContextAccessor`). This violated the **Dependency Inversion Principle** and forced EF Core as a transitive dependency on every consumer — even those that only wanted the domain abstractions.

Additionally, `BaseValueObject` (a class with manual `Equals`/`GetHashCode`) was redundant: C# records have provided structural equality, immutability, and hashing since C# 9, and .NET 10/11 refine this further.

## Goals

1. Split into **two NuGet packages**: `OroKernel.Domain` (pure BCL) and `OroKernel.Infrastructure` (EF Core + HTTP).
2. Remove `BaseValueObject` — migrate all value objects to `sealed record`.
3. Remove `DbSet<T> CurrentContext` from `IRepositoryBase<T>` to eliminate EF Core leak in domain contracts.
4. Multi-target both libraries to `net10.0;net11.0`.
5. Rename test project from `Shared.Tests` → `OroKernel.Domain.Tests`.
6. Update all 4 examples, solution file, `global.json`, `Directory.Build.props`.
7. Run full verification: build, test, example runs, grep for legacy references.

## What Changed

| Old (OroKernel.Shared) | New Location | Reason |
|---|---|---|
| `Entities/BaseEntity.cs` | `OroKernel.Domain/Entities/` | Domain — no EF |
| `Entities/BaseValueObject.cs` | **Deleted** | Replaced by `record` |
| `Entities/Error.cs` (record) | `OroKernel.Domain/Entities/` | Domain — no EF |
| `Entities/Result.cs` | `OroKernel.Domain/Entities/` | Domain — no EF |
| `Entities/WithDomainEventBase.cs` | `OroKernel.Domain/Entities/` | Domain — `[NotMapped]` is BCL |
| `Events/DomainEventBase.cs` (record) | `OroKernel.Domain/Events/` | Domain — no EF |
| `Enums/EntityBaseState.cs` | `OroKernel.Domain/Enums/` | Domain — no EF |
| `Interfaces/*` (all but 3) | `OroKernel.Domain/Interfaces/` | Domain — no EF |
| `Interfaces/IOroAppDbContext.cs` | `OroKernel.Infrastructure/Interfaces/` | Infrastructure — DbContext |
| `Interfaces/IUserInfoProvider.cs` | `OroKernel.Infrastructure/Interfaces/` | Infrastructure — consumed by DbContext |
| `Interfaces/IIdentityClientService.cs` | `OroKernel.Infrastructure/Interfaces/` | Infrastructure — HttpClient |
| `Data/AuditableDbContext.cs` | `OroKernel.Infrastructure/Audit/` | Infrastructure — EF Core |
| `Entities/AuditEntry.cs` (and PropertyChange) | `OroKernel.Infrastructure/Audit/` | Infrastructure — EF Core owned types |
| `Entities/AuditEntryProperty.cs` | `OroKernel.Infrastructure/Audit/` | Infrastructure — EF Core entity |
| `Options/UserInfo.cs`, `RoleInfo.cs` | `OroKernel.Infrastructure/Options/` | Infrastructure — consumed by infra services |
| `Services/*` | `OroKernel.Infrastructure/Services/` | Infrastructure — HttpClient, IHttpContextAccessor |
| `Specification/BaseSpecification.cs` | `OroKernel.Domain/Specification/` | Domain — only `Expression` (BCL) |

## Value Objects Migration

| Project | VO | Old Base | New Form |
|---|---|---|---|
| `UserManagement.DDD` | `Email` | `BaseValueObject` | `sealed record Email(string Value)` + `Create()` factory |
| `UserManagement.DDD` | `FullName` | `BaseValueObject` | `sealed record FullName(string FirstName, string LastName)` |
| `UserManagement.DDD` | `UserName` | `BaseValueObject` | `sealed record UserName(string Value)` |
| `IdentityManagement.DDD` | `CountryCode` | `BaseValueObject` | `sealed record CountryCode(string Value)` |
| `IdentityManagement.DDD` | `IdentificationTypeId` | `BaseValueObject` | `sealed record IdentificationTypeId(Guid Value)` |
| `IdentityManagement.DDD` | `IdentificationTypeName` | `BaseValueObject` | `sealed record IdentificationTypeName(string Value)` |
| `IdentityManagement.DDD` | `ValidationPattern` | `BaseValueObject` | `sealed record ValidationPattern(string Value)` |

All use a static `Create()` factory for validation. Implicit/explicit operators preserved.

## Key Interface Change

`IRepositoryBase<T>` in `OroKernel.Domain.Interfaces`:

```diff
 public interface IRepositoryBase<T> where T : class, IAggregateRoot
 {
-    DbSet<T> CurrentContext { get; }
     Task AddAsync(T entity, CancellationToken cancellationToken);
     // … unchanged …
 }
```

`DbSet<T>` was an EF Core type leaking into the domain. Removed. Implementations in Infrastructure still use `DbSet<T>` internally.

## Multi-targeting

All three library `.csproj` files use:

```xml
<TargetFrameworks>net10.0;net11.0</TargetFrameworks>
```

`global.json` updated to:

```json
{ "sdk": { "version": "10.0.301", "allowPrerelease": true, "rollForward": "latestMajor" } }
```

EF Core 10.0.9 packages are compatible with .NET 11 at the binary level. No `#if` directives needed.

## NuGet Package v2.0.0

- `OroKernel.Domain.2.0.0.nupkg` — pure domain
- `OroKernel.Infrastructure.2.0.0.nupkg` — depends on `OroKernel.Domain`

The old `OroKernel.Shared` package is **removed** from the repository. Consumers must migrate:
- `using OroKernel.Shared.*` → `using OroKernel.Domain.*` / `using OroKernel.Infrastructure.*`
- `<PackageReference Include="OroKernel.Shared" />` → `OroKernel.Domain` + optionally `OroKernel.Infrastructure`
- `BaseValueObject` subclasses → `sealed record`

## Verification

- ✅ `dotnet build -c Release` for `net10.0` and `net11.0` — 0 warnings, 0 errors
- ✅ `dotnet test` — 36 passed (both TFMs)
- ✅ `dotnet run` on all 4 examples — all complete with `successfully!` message
- ✅ `grep -r "OroKernel.Shared" .` — 0 (only binary artifacts in `bin/`/`obj/`)
- ✅ `grep -r "EntityFrameworkCore" src/OroKernel.Domain` — 0
- ✅ `grep -r "BaseValueObject" .` — 0
- ✅ `grep -r "CurrentContext" .` — 0
