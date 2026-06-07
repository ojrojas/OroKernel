# OroKernel

OroKernel is a shared library for .NET applications that provides reusable components for identity systems and data management. It includes base entities, automatic auditing, identity helper services, and domain-event primitives.

## Features

- **Base Entities**: Abstract classes for entities with GUID identifiers and generic `TId` support, value objects, and domain-event support.
- **Automatic Auditing**: `AuditableDbContext` tracks create/update/delete operations and writes `AuditEntry` records.
- **Identity Helpers**: `ClaimsUserInfoService` and `IdentityClientService` helpers to populate auditing user info and integrate with identity providers.
- **Domain Events**: Lightweight domain-event primitives (`IDomainEvent`, `IWithDomainEvents`)  for decoupled side-effect handling.
- **Unit Tests**: Test projects using xUnit, Moq, and EF Core InMemory for fast unit and integration tests.

## Project Structure

```
OroKernel/
├── Directory.Packages.props          # Centralized NuGet package version management
├── global.json                       # .NET SDK configuration (10.0.102)
├── nuget.config                      # NuGet sources configuration
├── OroKernel.slnx                    # Solution file
├── nupkgs/                           # Generated NuGet packages
└── src/
    ├── Shared/                       # Main library
    │   ├── Shared.csproj
    │   ├── GlobalUsings.cs
    │   ├── Data/                     # AuditableDbContext and EF helpers
    │   ├── Entities/                 # BaseEntity, BaseValueObject, AuditEntry, Error/Result
    │   ├── Enums/                    # EntityBaseState enum
    │   ├── Events/                   # Domain event primitives
    │   ├── Interfaces/               # Repository, domain event, business rule, identity interfaces
    │   ├── Options/                  # UserInfo, RoleInfo
    │   └── Services/                 # Identity-related services
    └── Shared.Tests/                 # Unit tests for the Shared library
```

## Requirements

- .NET SDK 10.0.102 (see `global.json`) or later
- Target framework: `net10.0`
- Central package versions are defined in `Directory.Packages.props` (EF Core, hosting, testing packages)

## Installation and Setup

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd OroKernel
   ```

2. Restore dependencies and build:
   ```bash
   dotnet restore
   dotnet build
   ```

3. Run unit tests:
   ```bash
   dotnet test
   ```

Note: `Shared.csproj` has `GeneratePackageOnBuild` enabled, so building may produce NuGet packages when packing/creating artifacts.

## Usage

The library offers base entity classes and an `AuditableDbContext` that you can inherit to get automatic audit entries and a consistent entity model.

Register services and providers in DI (example):

```csharp
// Configure a default user info (fallback)
services.Configure<UserInfo>(opts =>
{
    opts.Id = Guid.Empty;
    opts.UserName = "System";
    opts.Email = "system@example.com";
});

// Populate UserInfo from the current request's claims
services.AddTransient<IPostConfigureOptions<UserInfo>, ClaimsUserInfoService>();

// Provider used by AuditableDbContext to obtain per-request user info
services.AddScoped<OroKernel.Shared.Interfaces.IUserInfoProvider, OroKernel.Shared.Services.DefaultUserInfoProvider>();

// (Optional) Register typed HttpClient for identity integration with timeout and a lightweight retry handler
services.AddTransient<OroKernel.Shared.Services.RetryDelegatingHandler>();
services.AddHttpClient<OroKernel.Shared.Interfaces.IIdentityClientService, OroKernel.Shared.Services.IdentityClientService>((sp, client) =>
{
    client.BaseAddress = new Uri("https://identity.example/");
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddHttpMessageHandler<OroKernel.Shared.Services.RetryDelegatingHandler>();
```

Basic examples (see `examples/`):

```csharp
// Inherit from BaseEntity to get automatic GUID ID
public class MyEntity : BaseEntity
{
    public string Name { get; set; }
}

// DbContext
public class MyDbContext : AuditableDbContext
{
    public MyDbContext(DbContextOptions options, IOptions<UserInfo> userInfo)
        : base(options, userInfo) { }

    // DbSets...
}
```

## Examples

The `examples/` folder contains runnable console demos showing both simple usage and DDD/CQRS patterns:

- `examples/UserManagement` — simple example using `BaseEntity` with `Guid` IDs, including `IBusinessRule` implementations and `BaseSpecification<T>` examples with AND/OR/NOT combinators.
- `examples/IdentityManagement` — simple example using `BaseEntity<T, TId>` with `int` IDs.
- `examples/UserManagement.DDD` — layered DDD example (Domain / Application / Infrastructure / Presentation).
- `examples/IdentityManagement.DDD` — DDD example on identification types and CQRS.

Run an example from repository root, for example:

```bash
cd examples/UserManagement
dotnet run
```

Or for the DDD presentation:

```bash
cd examples/IdentityManagement.DDD/src/Presentation
dotnet run
```

See `examples/README.md` for more details per example.

## Testing

Run all tests:

```bash
dotnet test
```

The tests use `Microsoft.EntityFrameworkCore.InMemory`, `xUnit`, and `Moq` for unit and integration testing.

## Dependencies

Major dependencies are managed centrally in `Directory.Packages.props`. The repository uses:

- `Microsoft.EntityFrameworkCore` (InMemory / Sqlite packages referenced centrally)
- `Microsoft.Extensions.Hosting`, `Microsoft.Extensions.DependencyInjection`, `Microsoft.Extensions.Options`
- `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore` for EF diagnostics
- `xUnit` and `Moq` for testing

> **Production note**: When using `IdentityClientService` with `AddHttpClient`, configure the identity provider base URL (e.g. `https://identity.yourdomain.com/`) in your application's configuration. The placeholder `https://identity.example/` in the usage examples must be replaced with your real identity provider URL.

## Contributing

1. Create a branch for your feature: `git checkout -b feature/new-feature`
2. Make your changes and add tests
3. Run tests: `dotnet test`
4. Submit a pull request

## License

This project is licensed under the GNU AGPL v3.0 or later. See the LICENSE file in the project root for details.
