// UserDto.cs - Application DTO
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.

namespace UserManagement.DDD.Application.DTOs;

/// <summary>
/// Data Transfer Object for User
/// </summary>
public record UserDto(
    Guid Id,
    string UserName,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? LastModifiedAt
);