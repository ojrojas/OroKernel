// CreateIdentificationTypeCommand.cs - Application Command
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

namespace IdentityManagement.DDD.Application.Commands;

/// <summary>
/// Command to create a new identification type
/// </summary>
public record CreateIdentificationTypeCommand(
    Guid? Id,
    string Name,
    string Description,
    string CountryCode,
    int MaxLength,
    string ValidationPattern
);