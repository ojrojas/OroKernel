# OroKernel

OroKernel is a shared library for .NET applications, designed to provide reusable components in identity systems and data management. It includes base entities, authentication services, automatic auditing, and support for domain events.

## Features

- **Base Entities**: Abstract classes for entities with GUID identifiers, value objects, and support for domain events.
- **Automatic Auditing**: Database context that automatically logs changes to entities (creation, modification, deletion).
- **Identity Services**: Integration with identity systems to retrieve user and role information.
- **Domain Events**: Support for the domain events pattern with MediatR.
- **Unit Tests**: Complete set of unit tests with xUnit and Moq.

## Project Structure

```
OroKernel/
├── Directory.Packages.props          # Centralized NuGet package version management
├── global.json                       # .NET SDK configuration
├── nuget.config                      # NuGet sources configuration
├── OroKernel.slnx                    # Solution file
├── nupkgs/                           # Generated NuGet packages
└── src/
    ├── Shared/                       # Main library
    │   ├── Shared.csproj
    │   ├── GlobalUsings.cs
    │   ├── Data/
    │   │   └── AuditableDbContext.cs # DB context with auditing
    │   ├── Entities/
    │   │   ├── AuditEntry.cs
    │   │   ├── BaseEntity.cs
    │   │   ├── BaseValueObject.cs
    │   │   └── WithDomainEventBase.cs
    │   ├── Enums/
    │   │   └── EntityBaseState.cs
    │   ├── Events/
    │   │   └── DomainEvents.cs
    │   ├── Interfaces/
    │   │   ├── IAggregateRoot.cs
    │   │   ├── IAuditableEntity.cs
    │   │   ├── IDomainEvent.cs
    │   │   ├── IDomainEventDispatcher.cs
    │   │   ├── IDomainEventHandler.cs
    │   │   ├── IIdentityClientService.cs
    │   │   ├── IOroAppDbContext.cs
    │   │   └── IRepository.cs
    │   ├── Options/
    │   │   ├── RoleInfo.cs
    │   │   └── UserInfo.cs
    │   └── Services/
    │       ├── ClaimsUserInfoService.cs
    │       └── IdentityClientService.cs
    └── Shared.Tests/                  # Test project
        ├── Shared.Tests.csproj
        ├── UnitTest1.cs
        ├── Data/
        │   └── AuditableDbContextTests.cs
        ├── Entities/
        │   ├── BaseEntityTests.cs
        │   ├── BaseValueObjectTests.cs
        │   └── WithDomainEventBaseTests.cs
        ├── Events/
        │   └── DomainEventBaseTests.cs
        └── Services/
            ├── ClaimsUserInfoServiceTests.cs
            └── IdentityClientServiceTests.cs
```

## Requirements

- .NET 10.0.101 or higher
- .NET SDK installed

## Installation and Setup

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd OroKernel
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the solution:
   ```bash
   dotnet build
   ```

## Usage

### Base Entities

```csharp
// Inherit from BaseEntity to get automatic GUID ID
public class MyEntity : BaseEntity
{
    public string Name { get; set; }
}

// For value objects
public record MyValueObject(string Value1, int Value2) : BaseValueObject;
```

### Database Context with Auditing

```csharp
public class MyDbContext : AuditableDbContext
{
    public MyDbContext(DbContextOptions options, IOptions<UserInfo> userInfo)
        : base(options, userInfo) { }

    // Configure your DbSets here
}
```

### Identity Services

```csharp
// Configure ClaimsUserInfoService in the DI container
builder.Services.AddTransient<IPostConfigureOptions<UserInfo>, ClaimsUserInfoService>();

// Use IdentityClientService for identity API calls
var role = await identityClient.GetRoleByIdAsync(roleId);
```

## Testing

Run the unit tests:

```bash
dotnet test
```

The tests include:
- Entity and value object tests
- Service tests with mocks
- Auditing tests in in-memory database

## Dependencies

- **Microsoft.EntityFrameworkCore**: For ORM and database
- **Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore**: Diagnostics for EF Core
- **OroCQRS**: Internal library for CQRS (assuming it's a custom package)
- **xUnit**: Testing framework
- **Moq**: Mocking library
- **MediatR**: For domain events (through interfaces)

## Contributing

1. Create a branch for your feature: `git checkout -b feature/new-feature`
2. Make your changes and add tests
3. Run tests: `dotnet test`
4. Submit a pull request

## License

This project is licensed under the GNU AGPL v3.0 or later. See the LICENSE file in the project root for details.

## Contact

For questions or support, contact the development team.