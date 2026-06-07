# Contracts: Repository Audit Reports

This directory defines the contract schemas for all audit deliverables.

## Report Files

The following report files are produced during the audit workflow:

| Report | File | Produced In |
|--------|------|-------------|
| Repository Health Score | `specs/001-repo-audit-improvement/reports/health-score.md` | Phase 11 |
| Security Findings Report | `specs/001-repo-audit-improvement/reports/security-report.md` | Phase 2 (+ updates in Phase 5) |
| Code Quality Report | `specs/001-repo-audit-improvement/reports/code-quality-report.md` | Phase 3 |
| Technical Debt Report | `specs/001-repo-audit-improvement/reports/tech-debt-report.md` | Phase 3 |
| Test Coverage Report | `specs/001-repo-audit-improvement/reports/coverage-report.md` | Phase 4 (+ updates in Phase 6) |
| Dependency Report | `specs/001-repo-audit-improvement/reports/dependency-report.md` | Phase 8 |
| Performance Report | `specs/001-repo-audit-improvement/reports/performance-report.md` | Phase 9 |
| Change Log | `specs/001-repo-audit-improvement/reports/changelog.md` | Continuous |
| Final Validation Report | `specs/001-repo-audit-improvement/reports/final-validation.md` | Phase 11 |

## Repository Health Score Schema

```json
{
  "security": 0-10,
  "architecture": 0-10,
  "codeQuality": 0-10,
  "performance": 0-10,
  "testing": 0-10,
  "documentation": 0-10,
  "finalVerdict": "PRODUCTION READY | READY WITH MINOR RISKS | NEEDS IMPROVEMENT | HIGH RISK | CRITICAL SECURITY RISK"
}
```

## Security Finding Schema

See [data-model.md](../data-model.md) for Security Finding entity fields.

Each finding is reported with severity, impact, exploitation scenario, and remediation strategy.

## Dependency Record Schema

See [data-model.md](../data-model.md) for Dependency Record entity fields.

Each package is classified as Current, Vulnerable, Deprecated, Unused, or Excessive.
