# Dependency Report

## Overview

All NuGet package versions are managed centrally via `Directory.Packages.props`. Resolved versions are based on the current restore state with `net10.0` target.

---

## Shared (Library)

| Package | Requested | Resolved |
|---|---|---|
| `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore` | 10.0.7 | 10.0.7 |
| `Microsoft.EntityFrameworkCore.Tools` | 10.0.7 | 10.0.7 |

---

## Shared.Tests (Test Project)

| Package | Requested | Resolved |
|---|---|---|
| `coverlet.collector` | 10.0.0 | 10.0.0 |
| `Microsoft.EntityFrameworkCore.InMemory` | 10.0.7 | 10.0.7 |
| `Microsoft.NET.Test.Sdk` | 18.4.0 | 18.4.0 |
| `Moq` | 4.20.72 | 4.20.72 |
| `xunit` | 2.9.3 | 2.9.3 |
| `xunit.runner.visualstudio` | 3.1.5 | 3.1.5 |

---

## Example Projects (each: IdentityManagement, IdentityManagement.DDD, UserManagement, UserManagement.DDD)

| Package | Requested | Resolved |
|---|---|---|
| `Microsoft.EntityFrameworkCore.InMemory` | 10.0.7 | 10.0.7 |
| `Microsoft.Extensions.DependencyInjection` | 10.0.7 | 10.0.7 |
| `Microsoft.Extensions.Hosting` | 10.0.7 | 10.0.7 |
| `Microsoft.Extensions.Options` | 10.0.7 | 10.0.7 |

---

## Removed Dependencies

- **OroCQRS v1.0.0** — Removed from `Directory.Packages.props` (unused — no project referenced it).

---

## Vulnerability Status

- `dotnet list package --vulnerable`: No vulnerable packages found.
- `dotnet list package --deprecated`: No deprecated packages found.
- NuGet audit: Enabled at `low` level (catches all CVEs).

---

## Upgrade Notes

All resolved versions match requested versions — no pending updates within the current `10.0.x` / `18.x` / `4.x` range.
