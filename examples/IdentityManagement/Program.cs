// IdentityManagement
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OroKernel.Shared.Options;
using IdentityManagement;
using IdentityManagement.Data;

Console.WriteLine("=== Identity Management Example using BaseEntity<T, TId> with EF Core ===");
Console.WriteLine();

// Setup dependency injection
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Configure UserInfo for auditing
        services.Configure<UserInfo>(options =>
        {
            options.Id = Guid.NewGuid();
            options.UserName = "System User";
            options.Email = "system@example.com";
        });

        // Configure EF Core with In-Memory database
        services.AddDbContext<IdentityManagementDbContext>(options =>
            options.UseInMemoryDatabase("IdentityManagementDb"));
    })
    .Build();

// Get services
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

var dbContext = services.GetRequiredService<IdentityManagementDbContext>();

// Ensure database is created
await dbContext.Database.EnsureCreatedAsync();

Console.WriteLine("Database initialized successfully!");
Console.WriteLine();

// Create identification types with custom int IDs
var passportType = new IdentificationType(1)
{
    Name = "Passport",
    Description = "International travel document",
    CountryCode = "INT",
    MaxLength = 20,
    ValidationPattern = @"^[A-Z]{1,2}\d{6,9}$"
};

var nationalIdType = new IdentificationType(2)
{
    Name = "National ID",
    Description = "National identification card",
    CountryCode = "US",
    MaxLength = 11,
    ValidationPattern = @"^\d{3}-\d{2}-\d{4}$"
};

var driversLicenseType = new IdentificationType(3)
{
    Name = "Driver's License",
    Description = "Motor vehicle operator's license",
    CountryCode = "US",
    MaxLength = 15,
    ValidationPattern = @"^[A-Z]\d{7,14}$"
};

// Create an identification type with auto-generated ID
var taxIdType = new IdentificationType(4)
{
    Name = "Tax ID",
    Description = "Taxpayer identification number",
    CountryCode = "US",
    MaxLength = 9,
    ValidationPattern = @"^\d{9}$"
};

// Add identification types to database
Console.WriteLine("Adding identification types to database...");
dbContext.IdentificationTypes.Add(passportType);
dbContext.IdentificationTypes.Add(nationalIdType);
dbContext.IdentificationTypes.Add(driversLicenseType);
dbContext.IdentificationTypes.Add(taxIdType);
await dbContext.SaveChangesAsync();

Console.WriteLine("Identification types added successfully!");
Console.WriteLine();

// Query and display all identification types
Console.WriteLine("Identification Types in database:");
var allTypes = await dbContext.IdentificationTypes.ToListAsync();
foreach (var type in allTypes)
{
    Console.WriteLine($"ID: {type.Id}, Name: {type.Name}, Country: {type.CountryCode}, Max Length: {type.MaxLength}, Active: {type.IsActive}");
}
Console.WriteLine();

// Demonstrate updating identification type information
Console.WriteLine("Updating identification type information...");
passportType.UpdateInfo("International Passport", "Official international travel document", 25);
dbContext.IdentificationTypes.Update(passportType);
await dbContext.SaveChangesAsync();
Console.WriteLine($"Updated Passport Type: {passportType.Name}, Max Length: {passportType.MaxLength}");
Console.WriteLine();

// Demonstrate validation
Console.WriteLine("Identification Number Validation:");
Console.WriteLine($"Passport 'A123456789' valid: {passportType.ValidateIdentificationNumber("A123456789")}");
Console.WriteLine($"Passport 'INVALID' valid: {passportType.ValidateIdentificationNumber("INVALID")}");
Console.WriteLine($"National ID '123-45-6789' valid: {nationalIdType.ValidateIdentificationNumber("123-45-6789")}");
Console.WriteLine($"National ID 'INVALID' valid: {nationalIdType.ValidateIdentificationNumber("INVALID")}");
Console.WriteLine($"Tax ID '123456789' valid: {taxIdType.ValidateIdentificationNumber("123456789")}");
Console.WriteLine($"Tax ID '12345678' valid: {taxIdType.ValidateIdentificationNumber("12345678")}");
Console.WriteLine();

// Demonstrate activating/deactivating types
Console.WriteLine("Identification type status management:");
driversLicenseType.Deactivate();
dbContext.IdentificationTypes.Update(driversLicenseType);
await dbContext.SaveChangesAsync();
Console.WriteLine($"Driver's License deactivated: {driversLicenseType.IsActive}");

driversLicenseType.Activate();
dbContext.IdentificationTypes.Update(driversLicenseType);
await dbContext.SaveChangesAsync();
Console.WriteLine($"Driver's License reactivated: {driversLicenseType.IsActive}");
Console.WriteLine();

// Demonstrate querying by ID
Console.WriteLine("Querying identification type by ID...");
var foundType = await dbContext.IdentificationTypes.FindAsync(2);
if (foundType != null)
{
    Console.WriteLine($"Found type: {foundType.Name} ({foundType.CountryCode})");
}
Console.WriteLine();

// Show audit entries
Console.WriteLine("Audit entries:");
var auditEntries = await dbContext.AuditEntries.ToListAsync();
foreach (var audit in auditEntries.OrderBy(a => a.Timestamp))
{
    Console.WriteLine($"[{audit.Timestamp:yyyy-MM-dd HH:mm:ss}] {audit.Action} on {audit.EntityName} by {audit.UserId}");
}
Console.WriteLine();

// Show that all identification types inherit from BaseEntity<IdentificationType, int>
Console.WriteLine("All identification types inherit from BaseEntity<IdentificationType, int>:");
Console.WriteLine($"passportType is BaseEntity<IdentificationType, int>: {passportType is OroKernel.Shared.Entities.BaseEntity<IdentificationType, int>}");
Console.WriteLine($"nationalIdType is BaseEntity<IdentificationType, int>: {nationalIdType is OroKernel.Shared.Entities.BaseEntity<IdentificationType, int>}");
Console.WriteLine($"driversLicenseType is BaseEntity<IdentificationType, int>: {driversLicenseType is OroKernel.Shared.Entities.BaseEntity<IdentificationType, int>}");
Console.WriteLine();

// Demonstrate ID management
Console.WriteLine("ID Management:");
Console.WriteLine($"Passport Type ID: {passportType.Id} (Type: {passportType.Id.GetType().Name})");
Console.WriteLine($"National ID Type ID: {nationalIdType.Id} (Type: {nationalIdType.Id.GetType().Name})");
Console.WriteLine($"Driver's License Type ID: {driversLicenseType.Id} (Type: {driversLicenseType.Id.GetType().Name})");
Console.WriteLine($"Tax ID Type ID: {taxIdType.Id} (Type: {taxIdType.Id.GetType().Name})");

Console.WriteLine("\n=== Example completed successfully! ===");