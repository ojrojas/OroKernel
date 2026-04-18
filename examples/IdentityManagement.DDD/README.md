# IdentityManagement.DDD

A Domain-Driven Design (DDD) implementation for identity management using the OroKernel Shared library.

## Overview

This project demonstrates a complete DDD architecture for managing identification types (passports, driver's licenses, social security numbers, etc.) with proper separation of concerns across four layers:

- **Domain**: Core business logic, entities, value objects, and domain services
- **Application**: Application services, commands, queries, and DTOs
- **Infrastructure**: Data persistence, external services, and repository implementations
- **Presentation**: Console application demonstrating the system usage

## Architecture

### Domain Layer
- `IdentificationType`: Aggregate root entity inheriting from `BaseEntity<int, int>`
- Value Objects: `IdentificationTypeName`, `CountryCode`, `ValidationPattern`
- Repository interfaces: `IIdentificationTypeRepository`

### Application Layer
- `IdentificationTypeService`: Application service orchestrating business operations
- Commands: `CreateIdentificationTypeCommand`, `UpdateIdentificationTypeCommand`, `DeactivateIdentificationTypeCommand`
- Queries: `GetIdentificationTypeByIdQuery`, `GetAllIdentificationTypesQuery`
- DTOs: `IdentificationTypeDto`

### Infrastructure Layer
- `IdentityManagementDbContext`: EF Core context with auditing support
- `IdentificationTypeRepository`: Repository implementation
- `ServiceCollectionExtensions`: Dependency injection configuration

### Presentation Layer
- Console application demonstrating CRUD operations
- Uses dependency injection and in-memory database for demo purposes

## Features

- Full DDD layered architecture
- Entity Framework Core integration with SQLite/In-Memory
- Automatic auditing via `AuditableDbContext`
- Value object validation and conversion
- Repository pattern implementation
- Command/Query separation (CQRS)
- Dependency injection configuration
- Comprehensive business rules validation

## Running the Demo

```bash
cd examples/IdentityManagement.DDD/src/Presentation
dotnet run
```

The demo will:
1. Create several identification types (Passport, Driver's License, SSN)
2. List all identification types
3. Update an identification type
4. Retrieve a specific identification type
5. Deactivate an identification type
6. List only active identification types

## Dependencies

- .NET 10.0
- OroKernel.Shared library
- Entity Framework Core 9.0
- Microsoft.Extensions.DI 9.0

## Project Structure

```
IdentityManagement.DDD/
├── src/
│   ├── Domain/
│   │   ├── Entities/
│   │   │   └── IdentificationType.cs
│   │   ├── ValueObjects/
│   │   │   ├── IdentificationTypeName.cs
│   │   │   ├── CountryCode.cs
│   │   │   └── ValidationPattern.cs
│   │   ├── Repositories/
│   │   │   └── IIdentificationTypeRepository.cs
│   │   └── Domain.csproj
│   ├── Application/
│   │   ├── Commands/
│   │   │   ├── CreateIdentificationTypeCommand.cs
│   │   │   ├── UpdateIdentificationTypeCommand.cs
│   │   │   └── DeactivateIdentificationTypeCommand.cs
│   │   ├── Queries/
│   │   │   ├── GetIdentificationTypeByIdQuery.cs
│   │   │   └── GetAllIdentificationTypesQuery.cs
│   │   ├── DTOs/
│   │   │   └── IdentificationTypeDto.cs
│   │   ├── Services/
│   │   │   └── IdentificationTypeService.cs
│   │   └── Application.csproj
│   ├── Infrastructure/
│   │   ├── Data/
│   │   │   └── IdentityManagementDbContext.cs
│   │   ├── Repositories/
│   │   │   └── IdentificationTypeRepository.cs
│   │   ├── ServiceCollectionExtensions.cs
│   │   └── Infrastructure.csproj
│   └── Presentation/
│       ├── Program.cs
│       └── Presentation.csproj
└── IdentityManagement.DDD.sln
```

## Key Design Decisions

1. **Aggregate Root**: `IdentificationType` is the aggregate root with business invariants
2. **Value Objects**: Immutable objects for `Name`, `CountryCode`, and `ValidationPattern` with validation
3. **Repository Pattern**: Domain-focused repository interfaces with EF Core implementations
4. **CQRS**: Separate command and query objects for different operations
5. **Auditing**: Automatic audit trail via `AuditableDbContext` from Shared library
6. **Validation**: Business rules enforced in domain entities and value objects
7. **Dependency Injection**: Clean architecture with proper service registration

## Comparison with Non-DDD Version

This DDD version provides:
- Clear separation of concerns across layers
- Domain-focused business logic
- Immutable value objects with validation
- Repository abstractions
- Command/Query objects for better API design
- Enhanced testability and maintainability

The non-DDD version (`examples/IdentityManagement/`) is simpler but mixes concerns across layers.