---

description: "Task list for business rules and specification pattern examples"

---

# Tasks: Business Rules & Specification Pattern Examples

**Input**: Design documents from `specs/002-business-rules-specs-examples/`

**Prerequisites**: plan.md (required), spec.md (required), research.md, data-model.md, quickstart.md

**Tests**: Tests are MANDATORY per constitution (Section IV: Testing Requirements). Include unit tests for all business logic (90%+ coverage) and integration tests for critical paths (95%+ coverage).

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Example source**: `examples/UserManagement/`
- **Library source**: `src/Shared/`
- **Test project**: `src/Shared.Tests/`
- **Specification docs**: `specs/002-business-rules-specs-examples/`

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create directory structure for new example code

- [X] T001 Create `examples/UserManagement/BusinessRules/` directory
- [X] T002 Create `examples/UserManagement/Specifications/` directory
- [X] T003 Create `src/Shared.Tests/Examples/` directory
- [X] T004 Create `src/Shared.Tests/Examples/UserManagement/` directory
- [X] T005 [P] Create `src/Shared.Tests/Examples/UserManagement/BusinessRules/` directory
- [X] T006 [P] Create `src/Shared.Tests/Examples/UserManagement/Specifications/` directory

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Ensure stable prerequisite understanding

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [X] T007 Review `IBusinessRule` interface at `src/Shared/Interfaces/IBusinessRule.cs`
- [X] T008 Review `BaseSpecification<T>` class at `src/Shared/Specification/BaseSpecification.cs`

**Checkpoint**: Foundation ready — user story implementation can now begin

---

## Phase 3: User Story 1 — Business Rule Implementations (Priority: P1) 🎯 MVP

**Goal**: Create concrete `IBusinessRule` implementations in the UserManagement example project that demonstrate real validation patterns.

**Independent Test**: A developer can open `examples/UserManagement/BusinessRules/` and see 3 rule classes with clear naming, condition logic, and error messages.

### Implementation for User Story 1

- [X] T009 [P] [US1] Create `UserEmailMustBeUniqueRule.cs` in `examples/UserManagement/BusinessRules/` — rule checks that no existing user in the database has the same email; takes email and `UserManagementDbContext` via constructor; returns `Error.Conflict` on failure
- [X] T010 [P] [US1] Create `UserMustBeActiveRule.cs` in `examples/UserManagement/BusinessRules/` — rule checks `User.IsActive == true`; takes a `User` object via constructor; returns `Error.Validation` on failure
- [X] T011 [P] [US1] Create `MinimumUserNameLengthRule.cs` in `examples/UserManagement/BusinessRules/` — rule checks `userName.Length >= 3`; takes a username string via constructor; returns `Error.Validation` on failure

**Checkpoint**: At this point, User Story 1 should be complete. Three business rule classes exist and compile.

---

## Phase 4: User Story 2 — Specification Implementations (Priority: P1)

**Goal**: Create concrete `BaseSpecification<T>` subclasses that demonstrate filtering and combinator usage.

**Independent Test**: A developer can open `examples/UserManagement/Specifications/` and see 3 specification classes, each with a clear `ToExpression()` implementation, plus combinator usage examples.

### Implementation for User Story 2

- [X] T012 [P] [US2] Create `ActiveUserSpecification.cs` in `examples/UserManagement/Specifications/` — returns expression `u => u.IsActive == true`; demonstrates single-criteria filtering
- [X] T013 [P] [US2] Create `UserByEmailSpecification.cs` in `examples/UserManagement/Specifications/` — takes email string via constructor; returns expression for case-insensitive email match
- [X] T014 [P] [US2] Create `UserNameContainsSpecification.cs` in `examples/UserManagement/Specifications/` — takes substring via constructor; returns expression for `UserName.Contains(substring, OrdinalIgnoreCase)`
- [X] T015 [US2] Add combinator demonstration section to `Program.cs` or a standalone demo snippet — created `SpecificationDemo.cs` in `examples/UserManagement/Specifications/` showing `.And()`, `.Or()`, `.Not()` usage

**Checkpoint**: At this point, User Stories 1 AND 2 should be complete. Both rules and specs compile correctly.

---

## Phase 5: User Story 3 — Unit Tests (Priority: P1)

**Goal**: Each business rule and specification has at least one passing and one failing test case.

**Independent Test**: If a rule or spec is broken, at least one test fails. All tests pass when implementations are correct.

### Tests for User Story 3 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [X] T016 [P] [US3] Create `UserEmailMustBeUniqueRuleTests.cs` in `src/Shared.Tests/Examples/UserManagement/BusinessRules/` — test that rule passes when email is unique, fails with `Error.Conflict` when email exists
- [X] T017 [P] [US3] Create `UserMustBeActiveRuleTests.cs` in `src/Shared.Tests/Examples/UserManagement/BusinessRules/` — test that rule passes for active user, fails with `Error.Validation` for inactive user
- [X] T018 [P] [US3] Create `MinimumUserNameLengthRuleTests.cs` in `src/Shared.Tests/Examples/UserManagement/BusinessRules/` — test that rule passes for 3+ char names, fails for shorter names
- [X] T019 [P] [US3] Create `ActiveUserSpecificationTests.cs` in `src/Shared.Tests/Examples/UserManagement/Specifications/` — test `IsSatisfiedBy` for active/inactive; test `ToExpression().Compile()` returns correct results
- [X] T020 [P] [US3] Create `UserByEmailSpecificationTests.cs` in `src/Shared.Tests/Examples/UserManagement/Specifications/` — test exact match, case-insensitive match, and no-match cases
- [X] T021 [P] [US3] Create `UserNameContainsSpecificationTests.cs` in `src/Shared.Tests/Examples/UserManagement/Specifications/` — test substring match, case-insensitive, and no-match
- [X] T022 [US3] Create `SpecificationCombinatorTests.cs` in `src/Shared.Tests/Examples/UserManagement/Specifications/` — test `.And()`, `.Or()`, `.Not()` combinations return correct Boolean logic results

**Checkpoint**: All three user stories should now be complete. 7 new test files with pass/fail coverage for every rule and spec.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final validation and documentation updates

- [X] T023 Run `dotnet build` and fix any compile errors (0 errors, 0 warnings — PASS)
- [X] T024 Run `dotnet test` and verify all existing + new tests pass (87/87 — PASS)
- [X] T025 Update `README.md` if needed to reference the new business rule and specification examples
- [X] T026 Run quickstart.md validation scenarios end-to-end

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies — can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion
- **User Story 1 (Phase 3)**: Depends on Foundational — no dependencies on other stories
- **User Story 2 (Phase 4)**: Depends on Foundational — independent of US1
- **User Story 3 (Phase 5)**: Depends on US1 and US2 completion (tests test the rules and specs)
- **Polish (Phase 6)**: Depends on all user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational — No dependencies on other stories
- **User Story 2 (P1)**: Can start after Foundational — independent of US1
- **User Story 3 (P1)**: Depends on US1 and US2 — tests validate the implementations

### Within Each User Story

- Individual file creation tasks (marked [P]) can run in parallel
- Tests (Phase 5) should be written before implementation code per TDD convention

### Parallel Opportunities

- T009, T010, T011 (US1) can run in parallel
- T012, T013, T014 (US2) can run in parallel
- T016 through T021 (US3) can run in parallel
- T022 (combinator tests) depends on T019-T021 completion

---

## Parallel Example: User Story 1 (Business Rules)

```bash
# Launch all business rule tasks in parallel:
Task: "Create UserEmailMustBeUniqueRule at examples/UserManagement/BusinessRules/UserEmailMustBeUniqueRule.cs"
Task: "Create UserMustBeActiveRule at examples/UserManagement/BusinessRules/UserMustBeActiveRule.cs"
Task: "Create MinimumUserNameLengthRule at examples/UserManagement/BusinessRules/MinimumUserNameLengthRule.cs"
```

## Parallel Example: User Story 2 (Specifications)

```bash
# Launch all specification tasks in parallel:
Task: "Create ActiveUserSpecification at examples/UserManagement/Specifications/ActiveUserSpecification.cs"
Task: "Create UserByEmailSpecification at examples/UserManagement/Specifications/UserByEmailSpecification.cs"
Task: "Create UserNameContainsSpecification at examples/UserManagement/Specifications/UserNameContainsSpecification.cs"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup (directories)
2. Complete Phase 2: Foundational (review interfaces)
3. Complete Phase 3: User Story 1 (business rules)
4. **STOP and VALIDATE**: Compile, verify rules exist

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 (Business Rules) → Test independently → Compile-check
3. Add User Story 2 (Specifications) → Test independently → Compile-check
4. Add User Story 3 (Tests) → Run `dotnet test` → All pass
5. Add Polish (Final Validation) → Build + test + update docs

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story is independently completable and verifiable
- Run `dotnet build` after every phase to catch compile errors early
- Run `dotnet test` after Phase 5 to validate all tests pass
- No new NuGet dependencies should be added
