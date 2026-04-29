// OroKernel
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Exceptions;

public class DomainException : Exception
{
    public string Code { get; }
    public DomainException(
        string code, string message) : base(message) => Code = code;
    public DomainException(
        string code, string? message, Exception? innerException) : base(message, innerException) => Code = code;
}
