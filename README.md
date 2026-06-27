# OroKernel

OroKernel is a **two-library .NET kernel** that provides reusable primitives for building DDD-inspired applications.  
It enforces a **strict Domain / Infrastructure separation**: the domain layer has zero external dependencies beyond the BCL.

| Library | Responsibility | Dependencies |
|---|---|---|
| `OroKernel.Domain` | Pure domain primitives: entities, value objects (as records), specifications, domain events, business rules, repository contracts | **Zero** (BCL only) |
| `OroKernel.Infrastructure` | EF Core `AuditableDbContext`, audit entries, identity client services, HTTP retry handler, user-info providers | `OroKernel.Domain` + EF Core 10 |

Both libraries target **`net10.0` and `net11.0`**.

## Features

- **Base Entities**: `BaseEntity`, `BaseEntity<TId>`, `BaseEntity<T, TId>` with `Guid.CreateVersion7()` auto-IDs, equality, and domain-event support.
- **Value Objects as `record`**: Built on C# records (`sealed record Email(string Value)`) for structural equality, immutability, and zero boilerplate.
- **Automatic Auditing**: `AuditableDbContext` tracks create/update/delete operations and writes `AuditEntry` records.
- **Specification Pattern**: `BaseSpecification<T>` with AND / OR / NOT combinators.
- **Business Rules**: `IBusinessRule` with `Error` result pattern.
- **Domain Events**: Lightweight `IDomainEvent` / `IDomainEventDispatcher` / `IWithDomainEvents` primitives.
- **Identity Helpers**: `ClaimsUserInfoService`, `IdentityClientService`, `RetryDelegatingHandler`.
- **Multi-target**: `net10.0` + `net11.0` in every library and example.

## Project Structure

```
OroKernel/
├── Directory.Build.props              # C# latest, Nullable, WarningsAsErrors — inherited by all projects
├── Directory.Packages.props           # Centralized NuGet package version management
├── global.json                        # .NET 10.0.301 SDK (latestMajor roll‑forward allows .NET 11)
├── OroKernel.slnx                     # Solution file
├── nupkgs/                            # Generated NuGet packages
├── specs/
│   └── 003-domain-infrastructure-split/  # Specification, plan, and tasks for this refactor
└── src/
    ├── OroKernel.Domain/              # Pure domain library (no EF Core)
    │   ├── Entities/                  # BaseEntity, Error, Result, WithDomainEventBase
    │   ├── Enums/                     # EntityBaseState
    │   ├── Events/                    # DomainEventBase
    │   ├── Interfaces/                # IAggregateRoot, IBusinessRule, IDomainEvent*, IRepository, ISpecification
    │   └── Specification/            # BaseSpecification<T> + combinators
    │
    ├── OroKernel.Infrastructure/      # Infrastructure library (EF Core)
    │   ├── Audit/                     # AuditableDbContext, AuditEntry, AuditEntryProperty
    │   ├── Interfaces/                # IOroAppDbContext, IUserInfoProvider, IIdentityClientService
    │   ├── Options/                   # UserInfo, RoleInfo
    │   └── Services/                  # ClaimsUserInfoService, DefaultUserInfoProvider, IdentityClientService, RetryDelegatingHandler
    │
    └── OroKernel.Domain.Tests/        # Unit and integration tests for both libraries

examples/
├── UserManagement/                    # Simple BaseEntity + Guid demo (IBusinessRule, BaseSpecification)
├── IdentityManagement/                # BaseEntity<T, TId> + int demo
├── UserManagement.DDD/                # Layered DDD (Domain / Application / Infrastructure / Presentation)
└── IdentityManagement.DDD/            # DDD + CQRS on identification types
```

## Requirements

- .NET SDK 10.0.301 or later (see `global.json`)
- Target frameworks: `net10.0` and `net11.0`
- Central package versions defined in `Directory.Packages.props`

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

3. Run all tests:
   ```bash
   dotnet test
   ```

> Both `OroKernel.Domain` and `OroKernel.Infrastructure` have `GeneratePackageOnBuild` enabled (`v2.0.0`).

## Usage

### Domain layer

```csharp
using OroKernel.Domain.Entities;
using OroKernel.Domain.Interfaces;

// Inherit from BaseEntity for auto‑GUID IDs, domain events, and entity equality
public class MyEntity : BaseEntity, IAggregateRoot
{
    public string Name { get; init; } = string.Empty;
}
```

### Infrastructure layer

```csharp
using OroKernel.Infrastructure.Audit;
using OroKernel.Infrastructure.Options;

public class MyDbContext : AuditableDbContext
{
    public MyDbContext(DbContextOptions options, IOptions<UserInfo> userInfo)
        : base(options, userInfo) { }

    public DbSet<MyEntity> MyEntities { get; set; } = null!;
}
```

Register services:

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
services.AddScoped<IUserInfoProvider, DefaultUserInfoProvider>();

// (Optional) Register typed HttpClient for identity integration with retry
services.AddTransient<RetryDelegatingHandler>();
services.AddHttpClient<IIdentityClientService, IdentityClientService>((sp, client) =>
{
    client.BaseAddress = new Uri("https://identity.example/");
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddHttpMessageHandler<RetryDelegatingHandler>();
```

### Value Objects as records

All value objects should be modeled as `sealed record` types. Equality, immutability, and hashing come for free:

```csharp
public sealed record Email(string Value)
{
    public static Email Create(string value) =>
        new(Normalize(value));

    private static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("…");
        // … validation …
        return value.Trim().ToLowerInvariant();
    }

    public static implicit operator string(Email e) => e.Value;
    public static explicit operator Email(string v) => Create(v);
}
```

## Examples

The `examples/` folder contains runnable console demos:

| Example | Pattern | Tech |
|---|---|---|
| `examples/UserManagement` | Simple `BaseEntity` + Guid | `IBusinessRule`, `BaseSpecification<T>` |
| `examples/IdentityManagement` | `BaseEntity<T, TId>` + int | EF Core converters |
| `examples/UserManagement.DDD` | Layered DDD | Domain / Application / Infrastructure / Presentation |
| `examples/IdentityManagement.DDD` | DDD + CQRS | Value Objects as records |

Run an example:

```bash
cd examples/UserManagement
dotnet run
```

## Testing

```bash
dotnet test
```

Tests use `xUnit`, `Moq`, and `Microsoft.EntityFrameworkCore.InMemory`. Both `net10.0` and `net11.0` target frameworks are tested.

## Dependencies

Major dependencies are managed centrally in `Directory.Packages.props`:

- `Microsoft.EntityFrameworkCore.*`  10.0.x — Infrastructure only
- `Microsoft.Extensions.*`           10.0.x — Infrastructure only
- `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore` — Infrastructure only
- `xUnit`, `Moq`, `coverlet.collector` — Tests only

> **OroKernel.Domain has zero NuGet dependencies.** It relies solely on the .NET BCL.

## Contributing

1. Create a branch: `git checkout -b feature/my-feature`
2. Make changes and add tests
3. Run all tests: `dotnet test`
4. Submit a pull request

## License

This project is licensed under the GNU AGPL v3.0 or later.
