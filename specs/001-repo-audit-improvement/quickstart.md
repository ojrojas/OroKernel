# Quickstart Validation Guide: Repository Audit

## Prerequisites

- .NET SDK 10.0.102 or later (verify with `dotnet --version`)
- Git checkout on branch `001-repo-audit-improvement`

## Setup

```bash
# From repository root
dotnet restore
dotnet build --no-restore
```

## Validation Scenarios

### Scenario 1: Baseline Tests Pass

```bash
dotnet test
```

**Expected**: All 64 tests pass. This confirms the starting state before any changes.

### Scenario 2: Build Passes

```bash
dotnet build
```

**Expected**: Build succeeds with no errors. This must remain true after every change.

### Scenario 3: Dependency Audit

```bash
dotnet list package --vulnerable
dotnet list package --deprecated
```

**Expected**: Output lists any vulnerable or deprecated packages. After Phase 8, no vulnerable packages should remain.

### Scenario 4: Duplicate Code Detection

Use manual code review to identify duplicate logic across the `src/Shared/` source files.

**Expected**: After Phase 7, no significant code duplication remains.

### Scenario 5: Security Scan

Manual code review for:
- Hardcoded secrets or credentials
- Injection vulnerabilities in string concatenation
- Insecure deserialization patterns
- Path traversal in file operations

**Expected**: After Phase 5, no critical or high-severity vulnerabilities remain.

### Scenario 6: Coverage Measurement

```bash
dotnet test --collect:"XPlat Code Coverage"
# Then use reportgenerator or coverage tool to view results
```

**Expected**: After Phase 6, business logic coverage >= 90%, critical path coverage >= 95%.

### Scenario 7: Full Validation

```bash
dotnet build
dotnet test
dotnet list package --vulnerable
```

**Expected**: All pass with zero vulnerabilities.

## Report Formats

See [data-model.md](./data-model.md) for the schema of each report artifact. Final reports are produced in the `specs/001-repo-audit-improvement/` directory.
