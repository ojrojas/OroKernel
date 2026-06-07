# Change Log

## 2026-06-06 — Repository Audit, Security Hardening & Quality Improvement

### Added
- Tests: `ErrorTests.cs` (6 tests for Error record type)
- Tests: `ResultTests.cs` (7 tests for Result/Result\<T\>)
- Tests: `RetryDelegatingHandlerTests.cs` (3 tests for retry logic)
- Tests: `AuditableDbContextIntegrationTests.cs` (6 integration tests for audit trail)
- Reports: Security, Code Quality, Tech Debt, Dependency, Coverage, Health Score, Final Validation, Changelog
- Spec: 001-repo-audit-improvement suite (spec, plan, tasks)
- Constitution: `.specify/memory/constitution.md` v1.0.0

### Changed
- `README.md` — Updated features, structure, dependencies; added production note on identity provider URL
- `LICENSE` — Replaced placeholder with full AGPL v3.0 text

### Fixed
- Directory renamed: `src/Shared/Entites/` → `src/Shared/Entities/`
- File renamed: `src/Shared/Interfaces/IIBussines.cs` → `IBusinessRule.cs`
- `BaseSpecificationTests.cs`: 5 broken tests corrected to match actual `ToExpression()` API

### Removed
- `src/Shared.Tests/UnitTest1.cs` (empty placeholder)
- `Directory.Packages.props`: `OroCQRS` v1.0.0 PackageVersion (unused dependency)
