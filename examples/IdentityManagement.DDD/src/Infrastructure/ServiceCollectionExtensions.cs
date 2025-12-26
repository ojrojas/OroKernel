// ServiceCollectionExtensions.cs - Infrastructure Service Registration
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using IdentityManagement.DDD.Application.Services;
using IdentityManagement.DDD.Domain.Repositories;
using IdentityManagement.DDD.Infrastructure.Data;
using IdentityManagement.DDD.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityManagement.DDD.Infrastructure;

/// <summary>
/// Extension methods for registering IdentityManagement services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds IdentityManagement services to the DI container
    /// </summary>
    public static IServiceCollection AddIdentityManagement(this IServiceCollection services, string connectionString)
    {
        // Infrastructure
        services.AddDbContext<IdentityManagementDbContext>(options =>
            options.UseInMemoryDatabase("IdentityManagementDb"));

        services.AddScoped<IIdentificationTypeRepository, IdentificationTypeRepository>();

        // Application
        services.AddScoped<IdentificationTypeService>();

        return services;
    }

    /// <summary>
    /// Adds IdentityManagement services with in-memory database for testing
    /// </summary>
    public static IServiceCollection AddIdentityManagementInMemory(this IServiceCollection services)
    {
        // Infrastructure
        services.AddDbContext<IdentityManagementDbContext>(options =>
            options.UseInMemoryDatabase("IdentityManagementDb"));

        services.AddScoped<IIdentificationTypeRepository, IdentificationTypeRepository>();

        // Application
        services.AddScoped<IdentificationTypeService>();

        return services;
    }
}