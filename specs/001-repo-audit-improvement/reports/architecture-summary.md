# Architecture Summary

## Layered Architecture

```
┌──────────────────────────────────────────────────────────────┐
│                    Application / Entry Point                   │
│  (Examples: UserManagement, IdentityManagement, etc.)         │
├──────────────────────────────────────────────────────────────┤
│                        Services Layer                          │
│  ClaimsUserInfoService    IdentityClientService                │
│  DefaultUserInfoProvider  RetryDelegatingHandler               │
├──────────────────────────────────────────────────────────────┤
│                      Interface Contracts                       │
│  IUserInfoProvider  IIdentityClientService  IRepository        │
│  IDomainEventDispatcher  IDomainEventHandler  ISpecification   │
├──────────────────────────────────────────────────────────────┤
│                        Data Layer                              │
│  AuditableDbContext (SaveChangesAsync → AuditEntry)            │
├──────────────────────────────────────────────────────────────┤
│                   Entities / Domain Model                       │
│  BaseEntity  BaseValueObject  AuditEntry  AuditEntryProperty   │
│  Error  Result  DomainEventBase  EntityBaseState               │
└──────────────────────────────────────────────────────────────┘
```

## Data Flow

### Audit Trail Flow

1. Application calls `AuditableDbContext.SaveChangesAsync()`
2. `OnBeforeSaveChanges()`:
   - Gets current user info via `IUserInfoProvider.GetUserInfo()`
   - Iterates `ChangeTracker.Entries()` for Added/Modified/Deleted
   - Creates `AuditEntry` records with entity state, user info, timestamp
   - Stores property changes in `TemporaryProperties` collection
3. `base.SaveChangesAsync()` (first call)
4. `OnAfterSaveChanges()`:
   - Resolves entity IDs from temporary properties
   - Serializes changes to JSON
   - Persists `AuditEntry` + `AuditEntryProperty` records
   - Calls `base.SaveChangesAsync()` again (second save)

### Identity Integration Flow

```
HttpClient (typed) → RetryDelegatingHandler (3 retries, exponential backoff)
    → Identity Provider API
        → GET api/getuserbyid/{userId}
        → GET api/getrolesidbyuserid/{userId}
        → GET api/getrolebyid/{roleId}
```

### Claims-Based User Info Flow

```
HttpRequest → HttpContext.User.Claims
    → ClaimsUserInfoService.PostConfigure()
        → Parses Sub/NameIdentifier, UserName, Email, State
        → Populates IOptions<UserInfo>
    → IUserInfoProvider.GetUserInfo()
    → AuditableDbContext uses for audit trail
```

## Architectural Observations

### Strengths
- Clean separation of concerns with well-defined layers
- Centralized package management
- Proper use of dependency injection and options pattern
- Specification pattern with combinators (And/Or/Not)
- Value object pattern implemented correctly

### Concerns
- **Double SaveChangesAsync**: `OnAfterSaveChanges()` calls `base.SaveChangesAsync()` a second time after `SaveChangesAsync()` already called it once
- **Lazy audit entry resolution**: Uses `ChangeTracker.Entries().FirstOrDefault()` which may return incorrect entry for bulk operations
- **Unused infrastructure**: `IDomainEventDispatcher` has a method but no implementation; `IDomainEventHandler<T>` is empty
- **Empty marker interfaces**: `IAggregateRoot`, `IAuditableEntity` have no members
