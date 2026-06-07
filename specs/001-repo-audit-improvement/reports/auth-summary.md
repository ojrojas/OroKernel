# Authentication and Authorization Summary

## Authentication

The library does **not implement authentication** itself. It consumes claims from an already-authenticated `HttpContext.User`, which is typically set by ASP.NET Core middleware (JWT Bearer, Cookies, OpenID Connect, etc.).

### Claims Processing

`ClaimsUserInfoService` (implements `IPostConfigureOptions<UserInfo>`) reads these claims from `HttpContext.User`:

| Claim Type | UserInfo Property |
|------------|------------------|
| `ClaimTypes.NameIdentifier` / `sub` | `Id` (Guid) |
| `ClaimTypes.Name` / `username` | `UserName` (required) |
| `ClaimTypes.Email` | `Email` |
| `state` | `State` (EntityBaseState) |

### User Info Provider Chain

1. `ClaimsUserInfoService` — populates `UserInfo` from current HTTP request claims
2. `DefaultUserInfoProvider` — returns `UserInfo` from `IOptions<UserInfo>` (scoped)

## Authorization

The library provides **no authorization enforcement**. There are no:
- Role-based checks
- Policy-based checks
- Permission attributes or middleware

The `RoleInfo` model exists but is not integrated with any authorization mechanism.

## Identity Integration

`IdentityClientService` communicates with an external identity provider via typed `HttpClient`:

- `GetUser(userId)` — GET `api/getuserbyid/{userId}`
- `GetRoleIds(userId)` — GET `api/getrolesidbyuserid/{userId}`
- `GetRole(roleId)` — GET `api/getrolebyid/{roleId}`

Authentication tokens are **not handled** by the library — HTTP clients must be configured with auth headers externally.

## Security Status

| Aspect | Status |
|--------|--------|
| Authentication | Consumer-side (reads HttpContext claims) |
| Authorization enforcement | None |
| Token management | Not implemented |
| Identity provider integration | HTTP client only (no auth headers) |
| Audit trail | Yes (AuditableDbContext) |
