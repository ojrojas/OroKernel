# Data Model: Repository Audit

## Security Finding

| Field | Type | Description |
|-------|------|-------------|
| `Id` | string | Unique identifier for the finding |
| `Severity` | enum | Critical, High, Medium, Low, Info |
| `Title` | string | Short description of the vulnerability |
| `Description` | string | Detailed explanation of the finding |
| `Location` | string | File path and line number where found |
| `Category` | enum | Secret, Injection, Auth, Config, Dependency, XSS, Crypto, Other |
| `Impact` | string | Business and technical impact if exploited |
| `ExploitationScenario` | string | How an attacker could exploit this |
| `RemediationStrategy` | string | Recommended fix or mitigation |
| `Status` | enum | Open, InProgress, Fixed, AcceptedRisk, FalsePositive |
| `RemediatedBy` | string | Reference to the change that fixed it |

**Validation Rules**: Severity must be one of Critical/High/Medium/Low/Info. Status transitions: Open → InProgress → Fixed. Open → AcceptedRisk with justification required.

## Code Quality Issue

| Field | Type | Description |
|-------|------|-------------|
| `Id` | string | Unique identifier |
| `Type` | enum | DeadCode, Duplication, Complexity, Naming, ErrorHandling, AntiPattern, Architecture, Formatting |
| `Location` | string | File path, class, and method |
| `Description` | string | What the issue is |
| `Severity` | enum | Critical, Major, Minor, Info |
| `Effort` | string | Estimated effort (minutes) |
| `RefactoringSuggestion` | string | Proposed change |
| `Status` | enum | Open, Fixed, Won't Fix |

## Dependency Record

| Field | Type | Description |
|-------|------|-------------|
| `PackageName` | string | NuGet package ID |
| `DeclaredVersion` | string | Version in Directory.Packages.props |
| `UsedVersion` | string | Version resolved during build |
| `Status` | enum | Current, Vulnerable, Deprecated, Unused, Excessive |
| `CveIds` | string[] | Known vulnerability IDs (if vulnerable) |
| `Recommendation` | string | Upgrade version or removal suggestion |
| `RiskLevel` | enum | None, Low, Medium, High, Critical |

## Test Coverage Record

| Field | Type | Description |
|-------|------|-------------|
| `Module` | string | Project or namespace |
| `BeforeCoverage` | decimal | Line coverage percentage before changes |
| `AfterCoverage` | decimal | Line coverage percentage after changes |
| `BranchBefore` | decimal | Branch coverage before |
| `BranchAfter` | decimal | Branch coverage after |
| `TestsAdded` | int | Number of new test methods |
| `NewTestFiles` | string[] | Paths to new test files |

## Change Log Entry

| Field | Type | Description |
|-------|------|-------------|
| `Date` | date | When the change was made |
| `Phase` | string | Which plan phase (2-11) |
| `Category` | enum | Security, Quality, Architecture, Test, Dependency, Performance, Documentation |
| `Description` | string | What was changed |
| `Rationale` | string | Why the change was made |
| `FilesChanged` | string[] | List of modified files |
| `VerificationStatus` | enum | Pending, Passed, Failed |
