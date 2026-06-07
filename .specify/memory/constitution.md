<!--
  Sync Impact Report
  Version change: (template) -> 1.0.0
  Modified principles: N/A (initial fill from template)
  Added sections: Core Principles I-VIII, Repository Review Workflow,
    Mandatory Security Checklist, Pull Request Standards, Output Format, Governance
  Removed sections: N/A
  Templates requiring updates:
    - .specify/templates/plan-template.md ✅ updated (Constitution Check gates)
    - .specify/templates/tasks-template.md ✅ updated (testing mandatory, principle-driven task categories)
    - .specify/templates/spec-template.md ✅ no changes needed
    - README.md ✅ no changes needed
  Follow-up TODOs: None
-->

# OroKernel Constitution

## Core Principles

### I. Security First

Security takes precedence over convenience.

The agent MUST identify and remediate:

- OWASP Top 10 vulnerabilities
- Hardcoded secrets and credential leaks
- SQL Injection, XSS, CSRF, SSRF, RCE
- Insecure deserialization
- Broken authentication and access control
- Sensitive data exposure
- Dependency vulnerabilities

Every security finding MUST be assigned a severity (Critical, High, Medium,
Low), explained with impact, and remediated with safe fixes when possible.

### II. Code Quality

All code MUST follow SOLID, DRY, KISS, YAGNI, and Clean Code principles.

The agent MUST eliminate:

- Dead code, duplicate logic, unused dependencies and imports
- Excessive complexity, large functions and classes
- Hidden side effects

Readability MUST be preferred over cleverness.

### III. Architecture Integrity

Architectural consistency MUST be preserved.

Evaluate layer separation, dependency boundaries, module cohesion, coupling,
domain modeling, and design patterns.

Refactor only when the resulting architecture is objectively simpler, safer, or
more maintainable.

### IV. Testing Requirements

Every modification MUST be validated.

The agent MUST run existing tests, fix broken tests, add missing tests, and
increase coverage where practical.

Coverage targets:

- Business logic: 90%+
- Critical paths: 95%+
- Overall repository: 80%+

Required test types: unit tests, integration tests, and end-to-end tests when
applicable.

### V. Dependency Governance

All dependencies MUST be analyzed.

The agent MUST identify vulnerable, deprecated, unmaintained, and redundant
packages; and recommend safe upgrades, long-term supported alternatives, and
dependency reduction where possible.

### VI. Performance Optimization

The agent MUST identify N+1 queries, slow database operations, inefficient
algorithms, memory leaks, excessive network calls, and blocking operations.

Optimize only when measurable improvements exist. Avoid premature optimization.

### VII. Observability

The system MUST be diagnosable via structured logging, error tracking,
monitoring, metrics, and tracing.

Sensitive information MUST never appear in logs.

### VIII. Documentation

Maintain documentation as part of development: README, installation
instructions, environment variables, deployment guides, API documentation, and
architecture documentation.

All documentation MUST reflect the current implementation.

## Repository Review Workflow

### Phase 1: Discovery

Analyze project structure, frameworks, dependencies, build system, CI/CD
pipelines, and security configuration. Produce an architecture summary.

### Phase 2: Risk Assessment

Generate a prioritized list of security, reliability, performance, and
maintainability risks ranked by severity and business impact.

### Phase 3: Improvement Plan

Create a remediation roadmap: critical issues first, then high-risk issues,
code quality improvements, performance improvements, and documentation
improvements.

### Phase 4: Implementation

Apply improvements incrementally. After every significant change, run tests,
validate behavior, and verify security impact.

### Phase 5: Verification

Produce a security report, quality report, test report, and performance report.

## Mandatory Security Checklist

Before approving any change, verify:

- [ ] No hardcoded secrets
- [ ] Input validation exists
- [ ] Output sanitization exists
- [ ] Authentication is secure
- [ ] Authorization is enforced
- [ ] Least privilege is applied
- [ ] Sensitive data is protected
- [ ] Dependencies are audited
- [ ] Logging is safe
- [ ] Error handling is secure

## Pull Request Standards

Every pull request MUST contain:

- **Summary**: What changed
- **Reason**: Why it changed
- **Security Impact**: Security implications
- **Testing Evidence**: Executed tests and results
- **Migration Notes**: Breaking changes and migration steps

## Output Format

Reports MUST follow this format:

Repository Health Score:

- Security: X/10
- Architecture: X/10
- Code Quality: X/10
- Performance: X/10
- Testing: X/10
- Documentation: X/10

Then: Critical Findings, Recommended Improvements, Applied Refactors, Security
Report, and Final Verdict (Production Ready / Needs Improvement / High Risk).

## Governance

This Constitution supersedes all other practices. Amendments require
documentation, approval, and a migration plan. All PRs and reviews MUST verify
compliance. Complexity MUST be justified.

**Version**: 1.0.0 | **Ratified**: 2026-06-06 | **Last Amended**: 2026-06-06
