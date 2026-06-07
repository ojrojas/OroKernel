# Quickstart: Business Rules & Specification Pattern Examples

## Prerequisites

- .NET SDK 10.0.102 or later
- Repository checkout on branch `002-business-rules-specs-examples`

## Setup

```bash
dotnet restore
dotnet build
```

## Validation Scenarios

### Scenario 1: Example project compiles and runs

```bash
cd examples/UserManagement
dotnet run
```

**Expected**: Application starts, demonstrates CRUD operations, prints audit entries, and exits cleanly with no errors. The business rules and specifications are exercised as part of the demo output.

### Scenario 2: All tests pass

```bash
dotnet test
```

**Expected**: 64 + N new tests pass (where N is the number of new business rule + specification tests). Zero failures, zero errors.

### Scenario 3: Business rule — email uniqueness

The example project creates a user with email "john.doe@example.com". A second attempt to create a user with the same email should trigger the `UserEmailMustBeUniqueRule` and display the validation error.

**Expected**: The console output shows a conflict error message for duplicate email.

### Scenario 4: Business rule — active user check

The example deactivates a user and then attempts an operation requiring an active user.

**Expected**: The console output shows a validation error indicating the user account is deactivated.

### Scenario 5: Specification — active users filter

The example applies `ActiveUserSpecification` to the list of users and displays only active users.

**Expected**: The console output shows only users where `IsActive == true`.

### Scenario 6: Specification — combinator usage

The example applies `ActiveUserSpecification.And(UserByEmailSpecification)` to demonstrate composite filtering.

**Expected**: The console output shows the combined filter results correctly.

## Report Formats

See [data-model.md](./data-model.md) for entity definitions. Final implementation tasks are defined in [tasks.md](./tasks.md).
