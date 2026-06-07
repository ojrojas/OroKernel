// OroKernel
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Entities;

/// <summary>
/// Represents the result of a operation that can either succeed with a value or fail with an error
/// </summary>
public class Result
{
    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null)
            throw new InvalidOperationException("Success result cannot have an error.");
        if (!isSuccess && error is null)
            throw new InvalidOperationException("Failure result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    /// <summary>
    /// Create a successfult result without a value
    /// </summary>
    /// <returns>Result without value</returns>
    public static Result Success() => new(true, null);
    /// <summary>
    /// Create a failed result with the given error
    /// </summary>
    /// <param name="error">Error instead</param>
    /// <returns>Result with error</returns>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Create a sucessful result with a value or a failed result with a error
    /// </summary>
    /// <typeparam name="TValue">Type value</typeparam>
    /// <param name="value">Value response</param>
    /// <returns>Success response</returns>
    public static Result<TValue> Success<TValue>(TValue value) => new(value);
    public static Result<TValue> Failure<TValue>(Error error) => new(error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    internal Result(TValue value) : base(true, null) => _value = value;

    internal Result(Error error) : base(false, error) => _value = default;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of a failed result.");

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new(error);
}