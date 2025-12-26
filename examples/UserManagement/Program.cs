// UserManagement
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OroKernel.Shared.Data;
using OroKernel.Shared.Options;
using OroKernel.Shared.Services;
using UserManagement;
using UserManagement.Data;

Console.WriteLine("=== User Management Example using BaseEntity with EF Core ===");
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
        services.AddDbContext<UserManagementDbContext>(options =>
            options.UseInMemoryDatabase("UserManagementDb"));
    })
    .Build();

// Get services
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

var dbContext = services.GetRequiredService<UserManagementDbContext>();

// Ensure database is created
await dbContext.Database.EnsureCreatedAsync();

Console.WriteLine("Database initialized successfully!");
Console.WriteLine();

// Create users with custom Guid IDs
var user1 = new User(Guid.NewGuid())
{
    UserName = "john_doe",
    FirstName = "John",
    LastName = "Doe",
    Email = "john.doe@example.com"
};

var user2 = new User(Guid.NewGuid())
{
    UserName = "jane_smith",
    FirstName = "Jane",
    LastName = "Smith",
    Email = "jane.smith@example.com"
};

// Create a user with auto-generated ID (Guid.CreateVersion7())
var user3 = new User
{
    UserName = "bob_wilson",
    FirstName = "Bob",
    LastName = "Wilson",
    Email = "bob.wilson@example.com"
};

// Add users to database
Console.WriteLine("Adding users to database...");
dbContext.Users.Add(user1);
dbContext.Users.Add(user2);
dbContext.Users.Add(user3);
await dbContext.SaveChangesAsync();

Console.WriteLine("Users added successfully!");
Console.WriteLine();

// Query and display all users
Console.WriteLine("Users in database:");
var allUsers = await dbContext.Users.ToListAsync();
foreach (var user in allUsers)
{
    Console.WriteLine($"ID: {user.Id}, Name: {user.FullName}, Username: {user.UserName}, Email: {user.Email}, Active: {user.IsActive}");
}
Console.WriteLine();

// Demonstrate updating user information
Console.WriteLine("Updating user information...");
user1.UpdateInfo("Johnny", "Doe Jr.", "johnny.doe@example.com");
dbContext.Users.Update(user1);
await dbContext.SaveChangesAsync();
Console.WriteLine($"Updated User 1: {user1.FullName}, Email: {user1.Email}");
Console.WriteLine();

// Demonstrate activating/deactivating users
Console.WriteLine("User status management:");
user2.Deactivate();
dbContext.Users.Update(user2);
await dbContext.SaveChangesAsync();
Console.WriteLine($"User 2 deactivated: {user2.IsActive}");

user2.Activate();
dbContext.Users.Update(user2);
await dbContext.SaveChangesAsync();
Console.WriteLine($"User 2 reactivated: {user2.IsActive}");
Console.WriteLine();

// Demonstrate querying by ID
Console.WriteLine("Querying user by ID...");
var foundUser = await dbContext.Users.FindAsync(user1.Id);
if (foundUser != null)
{
    Console.WriteLine($"Found user: {foundUser.FullName} ({foundUser.UserName})");
}
Console.WriteLine();

// Demonstrate soft delete (if implemented)
Console.WriteLine("Demonstrating soft delete...");
dbContext.Users.Remove(user3);
await dbContext.SaveChangesAsync();

var remainingUsers = await dbContext.Users.ToListAsync();
Console.WriteLine($"Remaining active users: {remainingUsers.Count}");
Console.WriteLine();

// Show audit entries
Console.WriteLine("Audit entries:");
var auditEntries = await dbContext.AuditEntries.ToListAsync();
foreach (var audit in auditEntries.OrderBy(a => a.Timestamp))
{
    Console.WriteLine($"[{audit.Timestamp:yyyy-MM-dd HH:mm:ss}] {audit.Action} on {audit.EntityName} by {audit.UserId}");
}
Console.WriteLine();

// Show that all users inherit from BaseEntity
Console.WriteLine("All users inherit from BaseEntity:");
Console.WriteLine($"user1 is BaseEntity: {user1 is OroKernel.Shared.Entities.BaseEntity}");
Console.WriteLine($"user2 is BaseEntity: {user2 is OroKernel.Shared.Entities.BaseEntity}");
Console.WriteLine($"user3 is BaseEntity: {user3 is OroKernel.Shared.Entities.BaseEntity}");
Console.WriteLine();

// Demonstrate ID management
Console.WriteLine("ID Management:");
Console.WriteLine($"User 1 ID: {user1.Id} (Type: {user1.Id.GetType().Name})");
Console.WriteLine($"User 2 ID: {user2.Id} (Type: {user2.Id.GetType().Name})");
Console.WriteLine($"User 3 ID: {user3.Id} (Type: {user3.Id.GetType().Name})");

Console.WriteLine("\n=== Example completed successfully! ===");