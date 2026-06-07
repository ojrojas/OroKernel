# Implementation Plan: Repository Audit, Security Hardening and Quality Improvement

**Branch**: `001-repo-audit-improvement` | **Date**: 2026-06-06 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `specs/001-repo-audit-improvement/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/plan-template.md` for the execution workflow.

## Summary

Perform a comprehensive 11-phase audit, security hardening, and quality improvement across the OroKernel repository. Deliver a secure, maintainable, well-tested codebase with measurable improvements in all quality dimensions per the constitution.

## Technical Context

**Language/Version**: .NET 10.0 (C# 13, SDK 10.0.102 via `global.json`)

**Primary Dependencies**: Entity Framework Core 10.0.x, ASP.NET Core 10.0.x, Microsoft.Extensions 10.0.x, xUnit 2.9.3, Moq 4.20.72, Coverlet 10.0.0

**Storage**: EF Core InMemory (tests + examples), EF Core Sqlite (IdentityManagement.DDD example)

**Testing**: xUnit + Moq + Coverlet; `dotnet test` with `--collect:"XPlat Code Coverage"` for coverage

**Target Platform**: .NET library (net10.0), NuGet package `OroKernel.Shared`

**Project Type**: Library (NuGet package with examples)

**Performance Goals**: Reduce test suite execution time; identify and fix any N+1 EF Core queries or inefficient patterns

**Constraints**: Must preserve backward compatibility; no breaking changes to public API without documentation; all existing tests must remain green

**Scale/Scope**: Single library project with 42 existing tests, 4 example projects, ~20 source files

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Constitution Gates

The following MUST be verified against `.specify/memory/constitution.md`:

1. **Security** — Several concerns identified (no CI/CD, no authN/Z enforcement, hardcoded example URL, no secrets management). This plan directly addresses all security concerns in Phases 2 and 5. **PASS**.
2. **Code Quality** — Typo in filename (`IIBussines.cs` → `IBusinessRule.cs`), typo in directory name (`Entites/` → `Entities/`), dead code (`UnitTest1.cs`, `OroCQRS` reference, empty marker interfaces). Addressed in Phases 3 and 7. **PASS**.
3. **Architecture** — Clean layered structure overall; concerns include double `SaveChangesAsync` in auditing, unused domain event infrastructure, empty marker interfaces. Addressed in Phases 1 and 7. **PASS**.
4. **Testing** — 42 tests cover entities/services but missing integration tests, Specification pattern tests, and contains empty `UnitTest1.cs`. Addressed in Phases 4 and 6. **PASS**.
5. **Dependencies** — One unused dependency (`OroCQRS` v1.0.0) declared in `Directory.Packages.props`. Addressed in Phase 8. **PASS**.
6. **Performance** — No major performance concerns identified yet. EF Core audit double-save is a potential concern. Addressed in Phase 9. **PASS**.
7. **Observability** — Some structured logging exists in services. No sensitive data exposure found in logs. Addressed in Phase 9. **PASS**.
8. **Documentation** — README is comprehensive; missing CI/CD docs, incomplete LICENSE (AGPL preamble only), no API or architecture docs beyond README. Addressed in Phase 10. **PASS**.

No gate violations — complexity tracking is not required.

## Project Structure

### Documentation (this feature)

```text
specs/001-repo-audit-improvement/
├── spec.md              # Feature specification
├── plan.md              # This file
├── research.md          # Phase 0 output
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output (interface contracts)
└── checklists/
    └── requirements.md  # Spec quality checklist
```

### Source Code (repository root)

```text
OroKernel/
├── src/
│   └── Shared/
│       ├── Shared.csproj
│       ├── GlobalUsings.cs
│       ├── Data/
│       ├── Entities/            # Rename from Entites/ -> Entities/
│       ├── Enums/
│       ├── Events/
│       ├── Exceptions/
│       ├── Interfaces/
│       ├── Options/
│       ├── Services/
│       └── Specification/
├── src/Shared.Tests/
├── examples/
│   ├── IdentityManagement/
│   ├── IdentityManagement.DDD/
│   ├── UserManagement/
│   └── UserManagement.DDD/
└── specs/
    └── 001-repo-audit-improvement/
```

**Structure Decision**: Single project layout (library) with examples in separate subdirectories. Tests grouped under `src/Shared.Tests/` following .NET conventions.

## Complexity Tracking

No constitution gate violations detected. All plan phases directly address identified issues within the existing project structure.
