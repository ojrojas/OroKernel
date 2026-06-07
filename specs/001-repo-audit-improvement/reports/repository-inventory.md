# Repository Inventory

## Overview

**Project**: OroKernel
**Type**: .NET library (NuGet package: `OroKernel.Shared` v1.0.1)
**SDK**: .NET 10.0.102 (net10.0, C# 13)
**License**: GNU AGPL v3.0
**Repository**: https://github.com/ojrojas/OroKernel

## Technology Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| Runtime | .NET SDK | 10.0.102 |
| Language | C# | 13 |
| ORM | Entity Framework Core | 10.0.x |
| DI | Microsoft.Extensions.DependencyInjection | 10.0.x |
| Options | Microsoft.Extensions.Options | 10.0.x |
| Hosting | Microsoft.Extensions.Hosting | 10.0.x |
| Auth | ASP.NET Core HttpContext + Claims | 10.0.x |

## Project Structure

```
OroKernel/
├── src/Shared/                 # Main library (OroKernel.Shared)
│   ├── Data/                   # AuditableDbContext
│   ├── Entities/               # BaseEntity, BaseValueObject, AuditEntry, Result, Error
│   ├── Enums/                  # EntityBaseState
│   ├── Events/                 # DomainEventBase
│   ├── Exceptions/             # DomainException
│   ├── Interfaces/             # Repository, Specification, DomainEvent contracts
│   ├── Options/                # UserInfo, RoleInfo
│   ├── Services/               # Identity helpers, retry handler
│   └── Specification/          # BaseSpecification pattern
├── src/Shared.Tests/           # Unit tests
├── examples/
│   ├── IdentityManagement/     # Simple example with int IDs
│   ├── IdentityManagement.DDD/ # Layered DDD + CQRS example
│   ├── UserManagement/         # Simple example with Guid IDs
│   └── UserManagement.DDD/     # Layered DDD + CQRS example
└── specs/001-repo-audit-improvement/  # Feature specification
```

## Main Modules

| Module | Responsibility |
|--------|---------------|
| **Data** | `AuditableDbContext` — abstract base with automatic audit trail (tracks create/update/delete) |
| **Entities** | Base entity classes with GUID/typed ID support, value objects, domain event support |
| **Events** | Domain event primitives (`DomainEventBase`) |
| **Interfaces** | Repository pattern, specification pattern, domain event dispatcher, identity client contracts |
| **Services** | Claims-based user info population, identity provider HTTP client, retry handler |
| **Specification** | Specification pattern with AND/OR/NOT combinators |

## Dependencies

See [dependency-inventory.md](./dependency-inventory.md) for full details.

## Build Configuration

- Central Package Management via `Directory.Packages.props`
- Solution file: `OroKernel.slnx`
- NuGet packaging enabled on build (`GeneratePackageOnBuild=true`)
- Implicit usings enabled
- Nullable reference types enabled

## CI/CD

**None configured.** No GitHub Actions, Azure Pipelines, or other CI/CD infrastructure exists.

## Testing

- **Framework**: xUnit 2.9.3 + Moq 4.20.72
- **Coverage**: Coverlet 10.0.0
- **Current tests**: 43 passing (baseline)
- **Test database**: EF Core InMemory provider
