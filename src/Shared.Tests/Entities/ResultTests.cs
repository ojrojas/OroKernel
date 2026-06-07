namespace Shared.Tests.Entities;

public class ResultTests
{
    [Fact]
    public void Success_ReturnsSuccessfulResult()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
    }

    [Fact]
    public void Failure_ReturnsFailedResult()
    {
        var error = Error.NotFound("Not found.");
        var result = Result.Failure(error);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void SuccessWithValue_ReturnsSuccessfulResultWithValue()
    {
        var result = Result.Success(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void FailureWithValue_ReturnsFailedResult()
    {
        var error = Error.Validation("Invalid.");
        var result = Result.Failure<int>(error);

        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void ImplicitConversion_FromValue_CreatesSuccessResult()
    {
        Result<int> result = 42;

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void ImplicitConversion_FromError_CreatesFailureResult()
    {
        var error = Error.NotFound("Not found.");
        Result<int> result = error;

        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void AccessingValueOnFailure_ThrowsException()
    {
        var error = Error.Validation("Invalid.");
        var result = Result.Failure<int>(error);

        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}
