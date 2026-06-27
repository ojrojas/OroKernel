# Implementation Plan: Domain/Infrastructure Split & Value Objects as Records

**Branch**: (main) | **Spec**: [spec.md](./spec.md) | **Date**: 2026-06-26

## Summary

Replace the monolithic `OroKernel.Shared` library (v1.0.2) with two clean libraries:
- **`OroKernel.Domain`** — pure domain primitives (zero dependencies beyond BCL)
- **`OroKernel.Infrastructure`** — EF Core DbContext, audit entries, HTTP services (depends on Domain)

Eliminate `BaseValueObject` in favor of C# records. Multi-target `net10.0;net11.0`. Remove `DbSet<T>` from `IRepositoryBase<T>`.

## Constitution Check

- **Architecture**: ✅ Domain has zero EF Core references. Clean dependency inversion.
- **Dependencies**: ✅ Domain = 0 NuGet packages. Infrastructure = EF Core 10 only.
- **Testing**: ✅ All 36 tests pass on both TFMs. `BaseValueObjectTests` removed.
- **Documentation**: ✅ README updated, spec created.

## Phases Implemented

### Phase 1 — Skeleton
- Created `src/OroKernel.Domain/OroKernel.Domain.csproj`
- Created `src/OroKernel.Infrastructure/OroKernel.Infrastructure.csproj`
- Created `src/OroKernel.Domain.Tests/OroKernel.Domain.Tests.csproj`
- Added `Directory.Build.props` (LangVersion latest, Nullable, WarningsAsErrors)
- Updated `global.json` to `10.0.301` + `latestMajor`
- Updated `OroKernel.slnx`

### Phase 2 — Domain files
Moved: `BaseEntity`, `WithDomainEventBase`, `Error`, `Result`, `DomainEventBase`, `EntityBaseState`, all interfaces (except 3 infra-specific), `BaseSpecification`.  
Removed `DbSet<T> CurrentContext` from `IRepositoryBase<T>`.

### Phase 3 — Infrastructure files
Moved: `AuditableDbContext`, `AuditEntry`, `AuditEntryProperty`, `PropertyChange`, `IOroAppDbContext`, `IUserInfoProvider`, `IIdentityClientService`, `UserInfo`, `RoleInfo`, `ClaimsUserInfoService`, `DefaultUserInfoProvider`, `IdentityClientService`, `RetryDelegatingHandler`.

### Phase 4 — Value Objects as Records
- Deleted `BaseValueObject.cs`
- Rewrote `Email`, `FullName`, `UserName` (UserManagement.DDD) as `sealed record`
- Rewrote `CountryCode`, `IdentificationTypeId`, `IdentificationTypeName`, `ValidationPattern` (IdentityManagement.DDD) as `sealed record`
- Updated all callers (`UserApplicationService`, `DbContext` converters, `Program.cs`)
- Removed `BaseValueObjectTests.cs`

### Phase 5 — Examples updated
All 4 examples' `.csproj` updated to reference `OroKernel.Domain` + `OroKernel.Infrastructure` and changed to `TargetFrameworks`.  
All `using OroKernel.Shared.*` → `OroKernel.Domain.*` / `OroKernel.Infrastructure.*`.

### Phase 6 — Tests migrated
- Created `OroKernel.Domain.Tests` replacing `Shared.Tests`
- All tests namespaced under `OroKernel.Domain.Tests.*`
- 36 tests passing on both `net10.0` and `net11.0`

### Phase 7 — Documentation
- This plan
- `spec.md` — design spec
- `tasks.md` — task checklist

### Phase 8 — Verification

See `tasks.md` for detailed verification results.

## Key Decisions

| Decision | Choice | Rationale |
|---|---|---|
| Value Object pattern | `sealed record` + `Create()` factory | Equality/hash/immutability free; factory validates |
| `IRepository.CurrentContext` | Removed | EF Core leak violation |
| `UserInfo` / `RoleInfo` location | Infrastructure | Only consumed by infra (`AuditableDbContext`, `IdentityClientService`) |
| Multi-target strategy | EF Core 10 on both TFMs | Pragmatic; EF Core 10 binary-compatible on .NET 11 |
| `global.json` | `10.0.301` + `latestMajor` | Allows .NET 11 SDK without pinning |
| Package version | v2.0.0 | Breaking namespace change from v1.x |
