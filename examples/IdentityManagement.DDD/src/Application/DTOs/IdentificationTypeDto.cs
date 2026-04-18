// IdentificationTypeDto.cs - Application DTO
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

namespace IdentityManagement.DDD.Application.DTOs;

/// <summary>
/// Data Transfer Object for IdentificationType
/// </summary>
public record IdentificationTypeDto(
    Guid Id,
    string Name,
    string Description,
    string CountryCode,
    int MaxLength,
    string ValidationPattern,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? LastModifiedAt
);