// Program.cs - Presentation Layer
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using IdentityManagement.DDD.Application.Commands;
using IdentityManagement.DDD.Application.Queries;
using IdentityManagement.DDD.Application.Services;
using IdentityManagement.DDD.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityManagement.DDD.Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            await RunDemoAsync(services);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Add IdentityManagement services with in-memory database
                services.AddIdentityManagementInMemory();
            });

    private static async Task RunDemoAsync(IServiceProvider services)
    {
        var identificationTypeService = services.GetRequiredService<IdentificationTypeService>();

        Console.WriteLine("=== IdentityManagement.DDD Demo ===\n");

        // Create identification types
        Console.WriteLine("Creating identification types...");

        var passportCommand = new CreateIdentificationTypeCommand(
            null,
            "Passport",
            "Official travel document",
            "USA",
            9,
            @"^[A-Z0-9]{9}$"
        );

        var passport = await identificationTypeService.CreateIdentificationTypeAsync(passportCommand);
        Console.WriteLine($"Created: {passport.Name} (ID: {passport.Id})");

        var driversLicenseCommand = new CreateIdentificationTypeCommand(
            null,
            "Driver's License",
            "State-issued driver's license",
            "USA",
            15,
            @"^[A-Z0-9]{1,15}$"
        );

        var driversLicense = await identificationTypeService.CreateIdentificationTypeAsync(driversLicenseCommand);
        Console.WriteLine($"Created: {driversLicense.Name} (ID: {driversLicense.Id})");

        var ssnCommand = new CreateIdentificationTypeCommand(
            null,
            "Social Security Number",
            "US Social Security Number",
            "USA",
            11,
            @"^\d{3}-\d{2}-\d{4}$"
        );

        var ssn = await identificationTypeService.CreateIdentificationTypeAsync(ssnCommand);
        Console.WriteLine($"Created: {ssn.Name} (ID: {ssn.Id})");

        // List all identification types
        Console.WriteLine("\nListing all identification types:");
        var allTypes = await identificationTypeService.GetAllIdentificationTypesAsync(new GetAllIdentificationTypesQuery());
        foreach (var type in allTypes)
        {
            Console.WriteLine($"- {type.Name} ({type.CountryCode}): {type.Description} [Max: {type.MaxLength}]");
        }

        // Update an identification type
        Console.WriteLine("\nUpdating Passport description...");
        var updateCommand = new UpdateIdentificationTypeCommand(
            passport.Id,
            "Passport",
            "Official passport document for international travel",
            "USA",
            9,
            @"^[A-Z0-9]{9}$"
        );

        var updatedPassport = await identificationTypeService.UpdateIdentificationTypeAsync(updateCommand);
        Console.WriteLine($"Updated: {updatedPassport.Name} - {updatedPassport.Description}");

        // Get specific identification type
        Console.WriteLine("\nGetting Driver's License by ID:");
        var getByIdQuery = new GetIdentificationTypeByIdQuery(driversLicense.Id);
        var retrievedType = await identificationTypeService.GetIdentificationTypeByIdAsync(getByIdQuery);
        if (retrievedType != null)
        {
            Console.WriteLine($"- {retrievedType.Name}: {retrievedType.Description}");
        }

        // Deactivate an identification type
        Console.WriteLine("\nDeactivating Social Security Number...");
        var deactivateCommand = new DeactivateIdentificationTypeCommand(ssn.Id);
        await identificationTypeService.DeactivateIdentificationTypeAsync(deactivateCommand);
        Console.WriteLine("Deactivated SSN");

        // List only active identification types
        Console.WriteLine("\nListing active identification types:");
        var activeTypes = await identificationTypeService.GetActiveIdentificationTypesAsync();
        foreach (var type in activeTypes)
        {
            Console.WriteLine($"- {type.Name} ({type.CountryCode})");
        }

        Console.WriteLine("\n=== Demo completed successfully! ===");
    }
}