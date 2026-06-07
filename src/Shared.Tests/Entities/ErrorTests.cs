namespace Shared.Tests.Entities;

public class ErrorTests
{
    [Fact]
    public void NotFound_CreatesErrorWithNotFoundCode()
    {
        var error = Error.NotFound("Entity not found.");

        Assert.Equal("NOT_FOUND", error.Code);
        Assert.Equal("Entity not found.", error.Description);
    }

    [Fact]
    public void Validation_CreatesErrorWithValidationCode()
    {
        var error = Error.Validation("Invalid input.");

        Assert.Equal("VALIDATION", error.Code);
        Assert.Equal("Invalid input.", error.Description);
    }

    [Fact]
    public void Conflict_CreatesErrorWithConflictCode()
    {
        var error = Error.Conflict("Duplicate entry.");

        Assert.Equal("CONFLICT", error.Code);
        Assert.Equal("Duplicate entry.", error.Description);
    }

    [Fact]
    public void Unauthorized_CreatesErrorWithUnauthorizedCode()
    {
        var error = Error.Unauthorized("Access denied.");

        Assert.Equal("UNAUTHORIZED", error.Code);
        Assert.Equal("Access denied.", error.Description);
    }

    [Fact]
    public void Constructor_SetsCodeAndDescription()
    {
        var metadata = new Dictionary<string, object> { { "key", "value" } };
        var error = new Error("CUSTOM", "Custom error.", metadata);

        Assert.Equal("CUSTOM", error.Code);
        Assert.Equal("Custom error.", error.Description);
        Assert.Equal("value", error.Metadata["key"]);
    }

    [Fact]
    public void Metadata_DefaultsToEmptyDictionary()
    {
        var error = Error.NotFound("Test");

        Assert.NotNull(error.Metadata);
        Assert.Empty(error.Metadata);
    }
}
