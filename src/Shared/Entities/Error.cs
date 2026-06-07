// OroKernel
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Entities;

public record Error
{
    public Error(string code, string description, Dictionary<string, object>? metadata = null)
    {
        Code = code;
        Description = description;
        Metadata = metadata ?? [];
    }

    public string Code { get; }
    public string Description { get; }
    public Dictionary<string, object> Metadata { get; }

    public static Error NotFound(string description) =>
        new("NOT_FOUND", description);

    public static Error Validation(string description) =>
        new("VALIDATION", description);

    public static Error Conflict(string description) =>
        new("CONFLICT", description);

    public static Error Unauthorized(string description) =>
        new("UNAUTHORIZED", description);
}

