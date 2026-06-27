# OroKernel

OroKernel is a **two-library .NET kernel** that provides reusable primitives for building DDD-inspired applications.
It enforces a **strict Domain / Infrastructure separation**: the domain layer has zero external dependencies beyond the BCL.

| Library | Responsibility | Dependencies |
|---|---|---|
| `OroKernel.Domain` | Pure domain primitives: entities, value objects (as records), specifications, domain events, business rules, repository contracts | **Zero** (BCL only) |
| `OroKernel.Infrastructure` | EF Core `AuditableDbContext`, audit entries, identity client services, HTTP retry handler, user-info providers | `OroKernel.Domain` + EF Core 10 |

Both libraries target **`net10.0` and `net11.0`**.

---

## Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Requirements](#requirements)
- [Getting Started](#getting-started)
- [Usage](#usage)
  - [Entities](#entities)
  - [Value Objects](#value-objects)
  - [Specification Pattern](#specification-pattern)
  - [Business Rules](#business-rules)
  - [Domain Events](#domain-events)
  - [Auditable DbContext](#auditable-dbcontext)
  - [Dependency Injection](#dependency-injection)
- [Examples](#examples)
- [Testing](#testing)
- [Dependencies](#dependencies)
- [Contributing](#contributing)
- [License](#license)

---

## Features

- **Base Entities**: `BaseEntity`, `BaseEntity<TId>`, `BaseEntity<T, TId>` with `Guid.CreateVersion7()` auto-IDs, value-based equality, and domain-event support.
- **Value Objects as `record`**: Built on C# sealed records (`sealed record Email(string Value)`) — structural equality, immutability, and hashing come for free. No base class needed.
- **Automatic Auditing**: `AuditableDbContext` tracks create/update/delete operations and writes `AuditEntry` records with user context.
- **Specification Pattern**: `BaseSpecification<T>` with AND / OR / NOT combinator operators.
- **Business Rules**: `IBusinessRule` returning `Error` for domain validation failures.
- **Domain Events**: Lightweight `IDomainEvent` / `IDomainEventDispatcher` / `IDomainEventHandler<T>` / `IWithDomainEvents` primitives.
- **Repository Contracts**: `IRepositoryBase<T>`, `IRepository<T>`, `IRepository<TEntity, TId>` — fully decoupled from EF Core.
- **Identity Helpers**: `ClaimsUserInfoService`, `IdentityClientService`, `RetryDelegatingHandler` for claims extraction, OIDC integration, and HTTP retries.
- **Multi-target**: `net10.0` + `net11.0` in every library, example, and test project.

---

## Project Structure

```
OroKernel/
├── Directory.Build.props              # C# latest, Nullable, WarningsAsErrors (inherited by all projects)
├── Directory.Packages.props           # Centralized NuGet package version management
├── global.json                        # .NET SDK 10.0.301 (latestMajor roll-forward allows .NET 11)
├── OroKernel.slnx                     # Solution file
├── nupkgs/                            # Generated NuGet packages
├── specs/
│   └── 003-domain-infrastructure-split/  # Design spec, plan, and task checklist for this refactor
├── src/
│   ├── OroKernel.Domain/              # Pure domain library — zero NuGet dependencies
│   │   ├── Entities/                  # BaseEntity, WithDomainEventBase, Error, Result
│   │   ├── Enums/                     # EntityBaseState (INACTIVE, ACTIVE, MODIFIED, DELETED)
│   │   ├── Events/                    # DomainEventBase (abstract record)
│   │   ├── Interfaces/                # IAggregateRoot, IBusinessRule, IDomainEvent*, IRepository*, ISpecification, IWithDomainEvents
│   │   └── Specification/             # BaseSpecification<T> + AND / OR / NOT combinators
│   │
│   ├── OroKernel.Infrastructure/      # Infrastructure library — depends on Domain + EF Core 10
│   │   ├── Audit/                     # AuditableDbContext (abstract), AuditEntry, AuditEntryProperty, PropertyChange
│   │   ├── Interfaces/                # IOroAppDbContext, IUserInfoProvider, IIdentityClientService
│   │   ├── Options/                   # UserInfo, RoleInfo
│   │   └── Services/                  # ClaimsUserInfoService, DefaultUserInfoProvider, IdentityClientService, RetryDelegatingHandler
│   │
│   └── OroKernel.Domain.Tests/        # 36 unit + integration tests (xUnit, Moq, EF Core InMemory)

examples/
├── UserManagement/                    # Simple BaseEntity + Guid demo with IBusinessRule and BaseSpecification<T>
├── IdentityManagement/                # BaseEntity<T, TId> + int demo with EF Core value converters
├── UserManagement.DDD/                # Layered DDD example (Domain / Application / Infrastructure / Presentation)
└── IdentityManagement.DDD/            # DDD + CQRS example on identification types with value objects as records
```

---

## Requirements

- .NET SDK **10.0.301** or later (see `global.json`; `.NET 11` is also supported via `latestMajor` roll-forward)
- Target frameworks: `net10.0` and `net11.0`
- Central package versions managed in `Directory.Packages.props`

---

## Getting Started

1. **Clone** the repository:

   ```bash
   git clone <repository-url>
   cd OroKernel
   ```

2. **Restore and build**:

   ```bash
   dotnet restore
   dotnet build
   ```

3. **Run all tests**:

   ```bash
   dotnet test
   ```

4. **Run an example**:

   ```bash
   dotnet run --project examples/UserManagement/UserManagement.csproj
   ```

> Both `OroKernel.Domain` and `OroKernel.Infrastructure` produce NuGet packages (`v2.0.0`) on build. Packages land in `nupkgs/`.

---

## Usage

### Entities

Inherit from `BaseEntity` to get an auto-generated `Guid.CreateVersion7()` primary key, domain-event support, and value-based equality.

```csharp
using OroKernel.Domain.Entities;
using OroKernel.Domain.Interfaces;

public class Product : BaseEntity, IAggregateRoot
{
    public string Sku { get; init; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    public Product(string sku, string name, decimal price)
    {
        Sku = sku;
        Name = name;
        Price = price;
    }

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
        RaiseDomainEvent(new PriceChanged(Id, newPrice));
    }
}
```

For entities with a custom ID type (e.g. `int`):

```csharp
public class Category : BaseEntity<Category, int>, IAggregateRoot
{
    public string Name { get; set; } = string.Empty;

    public Category(int id)
    {
        Id = id;
    }
}
```

---

### Value Objects

Model all value objects as `sealed record`. Equality, immutability, and hashing are intrinsic.

Use a static `Create(...)` factory for validation. Keep the primary constructor accessible so EF Core value converters work out of the box.

```csharp
namespace MyApp.Domain.ValueObjects;

public sealed record Email(string Value)
{
    private static readonly Regex Pattern = new(
        @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$",
        RegexOptions.Compiled);

    /// <summary>
    /// Factory that validates and normalizes the email value.
    /// </summary>
    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be null or empty.", nameof(value));

        if (value.Length > 255)
            throw new ArgumentException("Email cannot exceed 255 characters.", nameof(value));

        var normalized = value.Trim().ToLowerInvariant();

        if (!Pattern.IsMatch(normalized))
            throw new ArgumentException("Invalid email format.", nameof(value));

        return new Email(normalized);
    }

    public override string ToString() => Value;

    public static implicit operator string(Email e) => e.Value;
    public static explicit operator Email(string v) => Create(v);
}
```

Composite value objects:

```csharp
public sealed record FullName(string FirstName, string LastName)
{
    public string DisplayName => $"{FirstName} {LastName}";

    public static FullName Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
        if (firstName.Length > 100 || lastName.Length > 100)
            throw new ArgumentException("Name parts cannot exceed 100 characters.");

        return new FullName(firstName.Trim(), lastName.Trim());
    }

    public override string ToString() => DisplayName;
    public static implicit operator string(FullName n) => n.DisplayName;
}
```

EF Core value converter for a record VO:

```csharp
// In your DbContext.OnModelCreating:
entity.Property(e => e.Email).HasConversion(
    v => v.Value,            // Email → string
    v => Email.Create(v));   // string → Email
```

---

### Specification Pattern

Define reusable, composable query filters:

```csharp
using OroKernel.Domain.Specification;

public sealed class ActiveProductSpec : BaseSpecification<Product>
{
    public override Expression<Func<Product, bool>> ToExpression()
        => p => p.IsActive;
}

public sealed class ProductBySkuSpec(string sku) : BaseSpecification<Product>
{
    public override Expression<Func<Product, bool>> ToExpression()
        => p => p.Sku == sku;
}
```

Compose specifications with `And`, `Or`, and `Not`:

```csharp
var spec = new ActiveProductSpec()
    .And(new ProductBySkuSpec("ABC-123"));

var isSatisfied = spec.IsSatisfiedBy(product);          // in-memory
var matches = products.Where(spec.ToExpression());       // LINQ / EF
```

---

### Business Rules

Encapsulate domain validation logic:

```csharp
using OroKernel.Domain.Entities;
using OroKernel.Domain.Interfaces;

public sealed class SkuMustBeUniqueRule : IBusinessRule
{
    private readonly string _sku;
    private readonly IEnumerable<Product> _existing;

    public SkuMustBeUniqueRule(string sku, IEnumerable<Product> existing)
    {
        _sku = sku;
        _existing = existing;
    }

    public Error? Error { get; private set; }

    public bool IsSatisfied()
    {
        if (_existing.Any(p => p.Sku == _sku))
        {
            Error = Error.Conflict($"SKU '{_sku}' is already in use.");
            return false;
        }
        return true;
    }
}
```

Usage:

```csharp
var rule = new SkuMustBeUniqueRule("ABC-123", existingProducts);
if (!rule.IsSatisfied())
    return Result.Failure(rule.Error!);
```

---

### Domain Events

Define a domain event:

```csharp
public sealed record PriceChanged(Guid ProductId, decimal NewPrice) : DomainEventBase;
```

Raise it from an entity (inherits from `BaseEntity` → `WithDomainEventBase`):

```csharp
product.UpdatePrice(29.99m);
// PriceChanged event is now in product.DomainEvents
```

Dispatch events from the infrastructure layer:

```csharp
var eventsWithEvents = changeTracker.Entries()
    .Select(e => e.Entity as IWithDomainEvents)
    .Where(e => e?.DomainEvents.Count > 0)
    .SelectMany(e => e!.DomainEvents)
    .ToList();

foreach (var domainEvent in eventsWithEvents)
    await dispatcher.DispatchAsync(domainEvent);
```

Implement `IDomainEventHandler<T>` for processing:

```csharp
public class PriceChangedHandler : IDomainEventHandler<PriceChanged>
{
    public Task Handle(PriceChanged notification, CancellationToken ct)
    {
        // Send notification, invalidate cache, etc.
        return Task.CompletedTask;
    }
}
```

---

### Auditable DbContext

Replace the standard `DbContext` with `AuditableDbContext` to get automatic audit trails.

```csharp
using OroKernel.Infrastructure.Audit;
using OroKernel.Infrastructure.Options;

public class MyDbContext : AuditableDbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options, IOptions<UserInfo> userInfo)
        : base(options, userInfo) { }

    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Sku).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            // …
        });
    }
}
```

Every `SaveChangesAsync` call automatically writes `AuditEntry` rows recording who (`UserId`, `UserName`), what (`Action`), when (`Timestamp`), and the changed values.

---

### Dependency Injection

Register the full stack:

```csharp
using OroKernel.Infrastructure.Interfaces;
using OroKernel.Infrastructure.Options;
using OroKernel.Infrastructure.Services;

// ── User Info for Auditing ──────────────────────────────────
services.Configure<UserInfo>(opts =>
{
    opts.Id = Guid.Empty;
    opts.UserName = "System";
    opts.Email = "system@example.com";
});

// Populates UserInfo from the current HTTP request's claims
services.AddTransient<IPostConfigureOptions<UserInfo>, ClaimsUserInfoService>();

// Provider consumed by AuditableDbContext on every save
services.AddScoped<IUserInfoProvider, DefaultUserInfoProvider>();

// ── EF Core DbContext ──────────────────────────────────────
services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

// ── (Optional) Identity HTTP Client with retry ────────────
services.AddTransient<RetryDelegatingHandler>();
services.AddHttpClient<IIdentityClientService, IdentityClientService>((_, client) =>
{
    client.BaseAddress = new Uri("https://identity.yourdomain.com/");
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddHttpMessageHandler<RetryDelegatingHandler>();
```

---

## Examples

The `examples/` folder contains four runnable console demos:

| Example | Pattern | Highlighted Features |
|---|---|---|
| `examples/UserManagement` | Simple `BaseEntity` + `Guid` | `IBusinessRule` (unique email, active guard), `BaseSpecification<T>` (AND/OR/NOT combinators), `AuditableDbContext` |
| `examples/IdentityManagement` | `BaseEntity<T, TId>` + `int` | Custom `int` primary key, EF Core value converters, audit trail |
| `examples/UserManagement.DDD` | Layered DDD | Domain / Application / Infrastructure / Presentation separation. Value objects as records (`Email`, `FullName`, `UserName`). Application services, commands, queries, DTOs |
| `examples/IdentityManagement.DDD` | DDD + CQRS | Seven value objects migrated from `BaseValueObject` to `sealed record`. Repository pattern, application services, validation factories |

Run any example from the repository root:

```bash
dotnet run --project examples/UserManagement/UserManagement.csproj
dotnet run --project examples/IdentityManagement/IdentityManagement.csproj
dotnet run --project examples/UserManagement.DDD/UserManagement.DDD.csproj
dotnet run --project examples/IdentityManagement.DDD/IdentityManagement.DDD.csproj
```

---

## Testing

```bash
dotnet test
```

Tests use **xUnit**, **Moq**, and **Microsoft.EntityFrameworkCore.InMemory**. Each test runs against both `net10.0` and `net11.0` target frameworks.

Coverage includes:
- Entity identity and equality (`BaseEntity`)
- Value object equality and hashing (via `record`)
- Domain event lifecycle (`WithDomainEventBase`)
- Specification combinators (`And`, `Or`, `Not`)
- `Result` / `Error` patterns
- `AuditableDbContext` (Add, Modify, Delete audits)
- Claims extraction (`ClaimsUserInfoService`)
- HTTP identity client (`IdentityClientService`)
- Default user-info provider

---

## Dependencies

Major dependencies are versioned centrally in `Directory.Packages.props`:

| Package | Version | Scope |
|---|---|---|
| `Microsoft.EntityFrameworkCore.*` | 10.0.x | Infrastructure only |
| `Microsoft.Extensions.*` | 10.0.x | Infrastructure only |
| `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore` | 10.0.x | Infrastructure only |
| `xunit`, `xunit.runner.visualstudio` | 2.9.x | Tests only |
| `Moq` | 4.20.x | Tests only |
| `coverlet.collector` | 10.0.x | Tests only |

> **`OroKernel.Domain` has zero NuGet dependencies.** It relies exclusively on the .NET BCL (`System.*` namespaces).

---

## Contributing

1. Create a branch: `git checkout -b feature/my-feature`
2. Make changes and add tests
3. Run all tests: `dotnet test`
4. Submit a pull request with a summary, security impact, and testing evidence

---

## License

This project is licensed under the GNU AGPL v3.0 or later. See the [LICENSE](./LICENSE) file for details.
