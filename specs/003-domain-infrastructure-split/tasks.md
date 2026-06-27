# Tasks: Domain/Infrastructure Split & Value Objects as Records

## Phase 1 — Skeleton ✅
- [x] Create `src/OroKernel.Domain/OroKernel.Domain.csproj`
- [x] Create `src/OroKernel.Infrastructure/OroKernel.Infrastructure.csproj`
- [x] Create `src/OroKernel.Domain.Tests/OroKernel.Domain.Tests.csproj`
- [x] Create `Directory.Build.props` (root)
- [x] Update `global.json` → `10.0.301` + `rollForward: latestMajor`
- [x] Update `OroKernel.slnx`
- [x] Verify `dotnet build` passes for Domain + Infrastructure

## Phase 2 — Domain files ✅
- [x] Migrate `Entities/BaseEntity.cs`, `WithDomainEventBase.cs`, `Error.cs`, `Result.cs`
- [x] Migrate `Events/DomainEventBase.cs`
- [x] Migrate `Enums/EntityBaseState.cs`
- [x] Migrate `Interfaces/*` (except `IOroAppDbContext`, `IUserInfoProvider`, `IIdentityClientService`)
- [x] Remove `DbSet<T> CurrentContext` from `IRepositoryBase<T>`
- [x] Migrate `Specification/BaseSpecification.cs`

## Phase 3 — Infrastructure files ✅
- [x] Migrate `Audit/AuditableDbContext.cs`, `AuditEntry.cs`, `AuditEntryProperty.cs`
- [x] Migrate `Interfaces/IOroAppDbContext.cs`, `IUserInfoProvider.cs`, `IIdentityClientService.cs`
- [x] Migrate `Options/UserInfo.cs`, `RoleInfo.cs`
- [x] Migrate `Services/*`
- [x] Verify `dotnet build` for Domain + Infrastructure

## Phase 4 — Value Objects as Records ✅
- [x] Delete `BaseValueObject.cs` from `src/`
- [x] Rewrite `Email.cs` as `sealed record` with `Create()` factory
- [x] Rewrite `FullName.cs` as `sealed record` with `Create()` factory
- [x] Rewrite `UserName.cs` as `sealed record` with `Create()` factory
- [x] Rewrite `CountryCode.cs` as `sealed record` with `Create()` factory
- [x] Rewrite `IdentificationTypeId.cs` as `sealed record` with `Create()` factory
- [x] Rewrite `IdentificationTypeName.cs` as `sealed record` with `Create()` factory
- [x] Rewrite `ValidationPattern.cs` as `sealed record` with `Create()` factory
- [x] Update all callers (services, DbContext converters, Program.cs)
- [x] Remove `BaseValueObjectTests.cs`

## Phase 5 — Examples updated ✅
- [x] `examples/UserManagement` — project ref + namespaces
- [x] `examples/IdentityManagement` — project ref + namespaces
- [x] `examples/UserManagement.DDD` — project ref + namespaces + VOs
- [x] `examples/IdentityManagement.DDD` — project ref + namespaces + VOs

## Phase 6 — Tests migrated ✅
- [x] Create `OroKernel.Domain.Tests` with correct `.csproj`
- [x] Fix `WithDomainEventBaseTests` (TestDomainEvent must be `record`)
- [x] Fix `BaseSpecificationTests` (add `using System.Linq.Expressions`)
- [x] Fix `ClaimsUserInfoServiceTests` (set `UserName` required property)
- [x] Verify `dotnet test` passes (36 tests)

## Phase 7 — Documentation ✅
- [x] Rewrite `README.md`
- [x] Create `specs/003-domain-infrastructure-split/spec.md`
- [x] Create `specs/003-domain-infrastructure-split/plan.md`
- [x] Create `specs/003-domain-infrastructure-split/tasks.md`

## Phase 8 — Verification ✅
- [x] `dotnet build -c Release` for `net10.0` and `net11.0` — 0 warnings, 0 errors
- [x] `dotnet test` — 36/36 passed (both TFMs)
- [x] `dotnet run` for all 4 examples — `successfully!` output
- [x] `grep -r "OroKernel.Shared" src examples --include="*.cs"` — 0
- [x] `grep -r "EntityFrameworkCore" src/OroKernel.Domain --include="*.cs"` — 0
- [x] `grep -r "BaseValueObject" . --include="*.cs"` — 0
- [x] `grep -r "CurrentContext" . --include="*.cs"` — 0
