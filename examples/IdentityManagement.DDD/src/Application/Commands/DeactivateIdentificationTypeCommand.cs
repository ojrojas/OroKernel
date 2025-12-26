// DeactivateIdentificationTypeCommand.cs - Application Command
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

using IdentityManagement.DDD.Domain.ValueObjects;

namespace IdentityManagement.DDD.Application.Commands;

/// <summary>
/// Command to deactivate an identification type
/// </summary>
public record DeactivateIdentificationTypeCommand(IdentificationTypeId Id);