// Program.cs - Presentation Layer
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
using UserManagement.DDD.Application.Commands;
using UserManagement.DDD.Application.Queries;
using UserManagement.DDD.Application.Services;
using UserManagement.DDD.Domain.Repositories;
using UserManagement.DDD.Infrastructure.Data;
using UserManagement.DDD.Infrastructure.Repositories;

Console.WriteLine("=== User Management DDD Example ===");
Console.WriteLine();

// Setup dependency injection with DDD architecture
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

        // Add ClaimsUserInfoService for user info
        services.AddTransient<IPostConfigureOptions<UserInfo>, ClaimsUserInfoService>();

        // Infrastructure Layer
        services.AddDbContext<UserManagementDbContext>(options =>
            options.UseInMemoryDatabase("UserManagementDb"));

        // Domain Layer
        services.AddScoped<IUserRepository, UserRepository>();

        // Application Layer
        services.AddScoped<UserApplicationService>();
    })
    .Build();

// Get services
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

var userService = services.GetRequiredService<UserApplicationService>();
var dbContext = services.GetRequiredService<UserManagementDbContext>();

// Ensure database is created
await dbContext.Database.EnsureCreatedAsync();

Console.WriteLine("Database initialized successfully!");
Console.WriteLine();

// Create users using application services
Console.WriteLine("Creating users via Application Service...");

var user1 = await userService.CreateUserAsync(new CreateUserCommand(
    Guid.NewGuid(), "john_doe", "John", "Doe", "john.doe@example.com"));

var user2 = await userService.CreateUserAsync(new CreateUserCommand(
    Guid.NewGuid(), "jane_smith", "Jane", "Smith", "jane.smith@example.com"));

var user3 = await userService.CreateUserAsync(new CreateUserCommand(
    null, "bob_wilson", "Bob", "Wilson", "bob.wilson@example.com"));

Console.WriteLine("Users created successfully!");
Console.WriteLine();

// Query and display all users
Console.WriteLine("Users in database:");
var allUsers = await userService.GetAllUsersAsync(new GetAllUsersQuery());
foreach (var user in allUsers)
{
    Console.WriteLine($"ID: {user.Id}, Name: {user.FullName}, Username: {user.UserName}, Email: {user.Email}, Active: {user.IsActive}");
}
Console.WriteLine();

// Demonstrate updating user information
Console.WriteLine("Updating user information...");
var updatedUser = await userService.UpdateUserAsync(new UpdateUserCommand(
    user1.Id, "Johnny", "Doe Jr.", "johnny.doe@example.com"));
Console.WriteLine($"Updated User: {updatedUser.FullName}, Email: {updatedUser.Email}");
Console.WriteLine();

// Demonstrate activating/deactivating users
Console.WriteLine("User status management:");
await userService.DeactivateUserAsync(new DeactivateUserCommand(user2.Id));
Console.WriteLine($"User 2 deactivated");

await userService.ActivateUserAsync(user2.Id);
Console.WriteLine($"User 2 reactivated");
Console.WriteLine();

// Demonstrate querying by ID
Console.WriteLine("Querying user by ID...");
var foundUser = await userService.GetUserByIdAsync(new GetUserByIdQuery(user1.Id));
if (foundUser != null)
{
    Console.WriteLine($"Found user: {foundUser.FullName} ({foundUser.UserName})");
}
Console.WriteLine();

// Demonstrate soft delete
Console.WriteLine("Demonstrating soft delete...");
await userService.DeleteUserAsync(user3.Id);

var remainingUsers = await userService.GetAllUsersAsync(new GetAllUsersQuery());
Console.WriteLine($"Remaining active users: {remainingUsers.Count()}");
Console.WriteLine();

// Show audit entries
Console.WriteLine("Audit entries:");
var auditEntries = await dbContext.AuditEntries.ToListAsync();
foreach (var audit in auditEntries.OrderBy(a => a.Timestamp))
{
    Console.WriteLine($"[{audit.Timestamp:yyyy-MM-dd HH:mm:ss}] {audit.Action} on {audit.EntityName} by {audit.UserId}");
}
Console.WriteLine();

// Demonstrate domain invariants and validation
Console.WriteLine("Domain validation examples:");
try
{
    // This should fail - duplicate username
    await userService.CreateUserAsync(new CreateUserCommand(
        Guid.NewGuid(), "john_doe", "Another", "User", "another@example.com"));
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Expected error: {ex.Message}");
}

try
{
    // This should fail - invalid email
    await userService.CreateUserAsync(new CreateUserCommand(
        Guid.NewGuid(), "valid_user", "Test", "User", "invalid-email"));
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Expected validation error: {ex.Message}");
}

Console.WriteLine("\n=== DDD Example completed successfully! ===");