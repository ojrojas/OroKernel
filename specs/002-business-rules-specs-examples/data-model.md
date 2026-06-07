# Data Model: Business Rules & Specification Pattern Examples

## Overview

This feature introduces concrete example implementations of `IBusinessRule` and `BaseSpecification<T>` within the existing `UserManagement` example project. No new persistent entities are created — the examples extend the existing `User` entity with validation and filtering logic.

---

## Existing Entity: User

| Field | Type | Notes |
|---|---|---|
| Id | Guid | Inherited from `BaseEntity` |
| UserName | string | Must be non-empty for business rule validation |
| Email | string | Must be unique per business rule; validated for format |
| FirstName | string | Display name component |
| LastName | string | Display name component |
| IsActive | bool | Used by specification filters |
| CreatedAt | DateTime | Auto-set on creation |

*Defined in: `examples/UserManagement/User.cs`*

---

## New Business Rule Classes

### UserEmailMustBeUniqueRule

| Aspect | Detail |
|---|---|
| Input | Email (string), UserDbContext (to query existing users) |
| Condition | No existing user has the same email |
| Error on failure | `Error.Conflict("A user with this email already exists.")` |
| Constructor | Takes email and DbContext (or Func<UserManagementDbContext>) |

### UserMustBeActiveRule

| Aspect | Detail |
|---|---|
| Input | User entity |
| Condition | `user.IsActive == true` |
| Error on failure | `Error.Validation("User account is deactivated.")` |
| Constructor | Takes a User object |

### MinimumUserNameLengthRule

| Aspect | Detail |
|---|---|
| Input | UserName (string) |
| Condition | `userName.Length >= 3` |
| Error on failure | `Error.Validation("Username must be at least 3 characters long.")` |
| Constructor | Takes the username string |

---

## New Specification Classes

### ActiveUserSpecification

| Aspect | Detail |
|---|---|
| Filter | `user => user.IsActive == true` |
| Expression | `Expression<Func<User, bool>>` returning active users |
| Combinators | Can be combined with AND/OR/NOT |

### UserByEmailSpecification

| Aspect | Detail |
|---|---|
| Filter | `user => user.Email == email` |
| Expression | Matches users with the given email (case-insensitive) |
| Constructor | Takes the email string to match |

### UserNameContainsSpecification

| Aspect | Detail |
|---|---|
| Filter | `user => user.UserName.Contains(substring, OrdinalIgnoreCase)` |
| Expression | Matches users whose username contains the given substring |
| Constructor | Takes the substring to search for |

---

## Validation Rules

| Rule | Applies To | Description |
|---|---|---|
| Email uniqueness | UserEmailMustBeUniqueRule | No two users may share the same email address |
| Active status | UserMustBeActiveRule | Certain operations require an active user |
| Minimum length | MinimumUserNameLengthRule | Username must be at least 3 characters |

## State Transitions

N/A — the examples demonstrate validation and filtering but do not modify entity state.
